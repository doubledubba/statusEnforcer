import os
import requests

CONFIG_PATH = os.path.join(os.environ['HOME'], '.config/statusEnforcer.txt')
HOST = 'http://10.100.58.69'
API = '%s/check_in' % HOST

key = 'tester'
SrvKey = 'fu@qy71q@_2-g_e_!3v$s5ecf)ar=ur0s@t&amp;m5_&amp;fy_elw&amp;m#%'


os.system('touch %s' % CONFIG_PATH)

with open(CONFIG_PATH, 'r+a') as fh:
    config = fh.read()
    if not config:
        name = raw_input('Name: ')
        r = requests.post(API, {'name': name, 'key': key})
        print r, r.text
        fh.write(r.text)
        

r = requests.post(API, {'clientId': 4, 'key': key})

if r.text == 'nope':
    print 'Auth failed!'

else:
    print r.text
    status, reqSrvKey = r.text.split('~')

    if reqSrvKey == SrvKey:
        print 'The server is authentic.'
        print 'Executing:', status


