import os
from sys import platform
from time import sleep

import requests

if platform not in ['darwin', 'linux2']:
    print 'Your operating system may not be supported with this version!'

CONFIG_PATH = os.path.join(os.environ['HOME'], '.config/statusEnforcer.txt')
HOST = 'http://10.100.58.69'
API = '%s/check_in' % HOST

key = 'tester'
SrvKey = 'fu@qy71q@_2-g_e_!3v$s5ecf)ar=ur0s@t&amp;m5_&amp;fy_elw&amp;m#%'

if not os.path.isfile(CONFIG_PATH):
    os.system('touch %s' % CONFIG_PATH)

with open(CONFIG_PATH, 'r+') as fh:
    config = fh.read()
    if not config:
        name = raw_input('Name: ')
        r = requests.post(API, {'name': name, 'key': key})
        print r, r.text
        fh.write(r.text)
        
while True:
    r = requests.post(API, {'clientId': 1, 'key': key})

    if r.text == 'nope':
        print 'Auth failed!'

    else:
        status, reqSrvKey = r.text.split('~')

        if reqSrvKey == SrvKey:
            print 'Received command from authentic server:',
            print status
            print '=' * 72

    sleep(5)

