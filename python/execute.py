#!/usr/bin/env python

import BaseHTTPServer
import CGIHTTPServer
import cgitb

# activar reporte de errores
cgitb.enable()

server = BaseHTTPServer.HTTPServer
handler = CGIHTTPServer.CGIHTTPRequestHandler
server_addres= ("", 8001)
handler.cgi_directories = ["/cgi-bin"]

httpd = server(server_addres, handler)
httpd.serve_forever()
