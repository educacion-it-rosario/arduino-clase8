from __future__ import print_function, absolute_import

import cgi
import json
import serial
import os

ME = os.path.abspath(__name__)
DIR = os.path.dirname(ME)

def abrir_puerto():
    try:
        puerto = open(os.path.join(DIR, "arduino-puerto")).read().strip()
    except:
        puerto = "/dev/ttyUSB0"

    out = serial.Serial(puerto, 9600)
    return out

def generar_json(contenido):
    print("Content-type: application/json")
    print("")
    print(json.dumps(contenido))
    print("")

def extraer_arguments(*args):
    store = cgi.FieldStorage()
    out = {}

    # primero sacar random, sin eso el pedido no es bueno
    out["random"] = store["random"].value

    for n in args:
        if n in store.keys():
            out[n] = store[n].value
        else:
            out[n] = None

    return out
