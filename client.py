import os
import requests

CONFIG_PATH = os.path.join(os.environ['HOME'], '.config/statusEnforcer.txt')
API = 'http://%s/check_in'
SrvKey = 'fu@qy71q@_2-g_e_!3v$s5ecf)ar=ur0s@t&amp;m5_&amp;fy_elw&amp;m#%'

os.system('touch %s' % CONFIG_PATH)

with open(CONFIG_PATH, 'w+') as fh:
    text = fh.readlines()
    print text
    if not text:
        name = raw_input('Name: ')
        clientKey = raw_input('Personal Key: ')
        hostName = raw_input('Host name: ')
        API = API % hostName
        r = requests.post(API, {'name': name, 'key': clientKey})
        fh.write(r.text + '\n')
        fh.write(clientKey + '\n')
        fh.write(hostName + '\n')
    else:
        clientId = text[0]
        clientKey = text[1]
        hostName = text[2]
        API = API % hostName
        r = requests.post(API, {'clientId': clientId, 'key': clientKey})
        if r.text == 'nope':
            print 'Auth failed!'
        else:
            status, reqSrvKey = r.text.split('~')
            if reqSrvKey == SrvKey:
                print 'Authentic!'
