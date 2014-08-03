#!/usr/bin/env python

import BaseHTTPServer
import CGIHTTPServer
import os
import sys

def main():
    handler = CGIHTTPServer.CGIHTTPRequestHandler
    handler.cgi_directories = ['/cgi-bin']

    if sys.argv[1:]:
        os.chdir(sys.argv[1])
        sys.argv.pop(1)

    BaseHTTPServer.test(HandlerClass=handler)

if __name__ == '__main__':
    main()
