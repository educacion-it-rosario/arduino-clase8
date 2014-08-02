#!/usr/bin/env python

from __future__ import print_function, absolute_import

import cgi
import cgitb
import serial

import common

# activar debugging
cgitb.enable()


def main():
    arguments = common.extraer_arguments("port")
    salida = {
        "port": int(arguments["port"]),
        "random": int(arguments["random"]),
    }

    # abrir puerto
    puerto = None
    try:
        puerto = common.abrir_puerto()
    except Exception, e:
        salida["error"] = "No hay placa "
        common.generar_json(salida)
        return

    # tirar todo lo que este pendiente
    puerto.flush()

    # solicitar estado
    puerto.write("S\n")

    # leer estado
    ret = puerto.readline()
    salida["status"] = int(ret)

    common.generar_json(salida)

if __name__ ==  '__main__':
    main()
