#!/usr/bin/env python
#
# -*- encoding: utf-8 -*-
#

#
# Autor:
#    Naranjo Manuel Francisco
#    naranjo.manuel@gmail.com
#    (t) @mnaranjo85
#

from __future__ import print_function, absolute_import

import cgi
import cgitb
import json
import serial
import os

# activar debugging
cgitb.enable()

ME = os.path.abspath(__name__)
DIR = os.path.dirname(ME)
PUERTO = os.path.join(DIR, 'cgi-bin', "arduino-puerto.cfg")
PUERTO_DEFAULT = "/dev/ttyUSB0"
PUERTO_BAUDRATE = 9600

# ---------- METODOS DE LIBRERIA -----------
def resolver_puerto():
    '''
    Metodo que devuelve el nombre del puerto a usar en funcion de la
    configuracion
    '''
    try:
        return open(PUERTO).read().strip()
    except:
        return PUERTO_DEFAULT

def generar_json(contenido):
    '''
    metodo utilizado para generar contenido json listo
    para ser retornado al cliente
    '''

    print("Content-type: application/json")
    print("")
    print(json.dumps(contenido))
    print("")

class MyStore():
    '''
    Clase sencilla para simplificar el trabajo con los argumentos de cgi
    '''

    store = None

    @classmethod
    def __sanitize__(cls):
        if getattr(cls, 'store', None) is None:
            cls.store = cgi.FieldStorage()
        return

    @classmethod
    def getValue(cls, name, default=None, cast=None):
        cls.__sanitize__()
        if name in cls.store:
            value = cls.store[name].value
            if callable(cast):
                value = cast(value)
            return value

        return default


# ----------- METODOS de IMPLEMENTACION --------------
def handle_switch_get(puerto=None, random=None, pin=None, salida=None):
    '''
    funcion que va a manejar el get para un pin
    '''

    # tirar todo lo que este pendiente
    puerto.flush()

    # solicitar estado
    puerto.write("S%s\n" % pin)

    # leer estado
    ret = puerto.readline()
    salida["status"] = int(ret)

    return generar_json(salida)

def handle_switch_set(puerto=None, random=None, pin=None, salida=None):
    '''
    funcion que va a manejar el set para un pin
    '''
    nstatus = salida['nstatus'] = MyStore.getValue('nstatus', cast=int)
    if nstatus is None:
        salida['error'] = 'nstatus tiene que ser definido y ser entero'
        return generar_json(salida)

    # tirar todo lo pendiente
    puerto.flush()

    # generar pedido
    action = "H" if nstatus else "L"
    puerto.write("%s%s\n" % (action, pin))

    return handle_switch_get(puerto=puerto, random=random, pin=pin,
                             salida=salida)

ACTIONS = {
    'get': handle_switch_get,
    'set': handle_switch_set
}

def main():
    '''
    Esta funcion es llamada para cualquier pedido web
    '''

    salida = {
        'random': None,
        'action': None,
        'pin': None,
    }

    # primero vamos a validar la entrada
    random = salida['random'] = MyStore.getValue('random', cast=int)
    if random is None:
        salida['error'] = 'random tiene que estar definido y ser entero'
        return generar_json(salida)

    action = salida['action'] = MyStore.getValue('action')
    if action is None:
        salida['error'] = 'action tiene que estar definido'
        return generar_json(salida)

    # verificar si accion es valida
    if action not in ACTIONS:
        acciones = " ".join(ACTIONS.keys())
        salida['error'] = 'action invalido, posibles: ' + acciones
        return generar_json(salida)

    pin = salida['pin'] = MyStore.getValue('pin', cast=int)
    if pin is None:
        salida['error'] =  'pin tiene que estar definido y ser entero'
        return generar_json(salida)

    # abrir el puerto serie ahora
    salida['serial'] = resolver_puerto()
    try:
        puerto = serial.Serial(salida['serial'], PUERTO_BAUDRATE)
    except:
        salida['error'] = 'No hay placa ' + salida['serial']
        return generar_json(salida)

    salida['serial_open'] = True

    # ahora despachamos la salida
    # despachar accion ahora
    return ACTIONS[action](puerto=puerto, random=random,
                           pin=pin, salida=salida)

if __name__=='__main__':
    # punto de entrada
    main()
