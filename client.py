import os
import requests

CONFIG_PATH = os.path.join(os.environ['HOME'], '.config/statusEnforcer.txt')
HOST = 'http://10.100.58.69'
API = '%s/check_in' % HOST

os.system('touch %s' % CONFIG_PATH)

with open(CONFIG_PATH, 'r+a') as fh:
    config = fh.read()
    if not config:
        name = raw_input('Name: ')
        r = requests.post(API, {'name': name})
        print r, r.text
        fh.write(r.text)
        

r = requests.post(API, {'clientId': 1})

print r
print 'Connection ok:', r.ok
print 'Server says:', r.text
