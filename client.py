import os
from sys import platform
from time import sleep

import requests

if platform not in ['darwin', 'linux2']:
    print 'Your operating system may not be supported with this version!'

import logging

logging.basicConfig(format='[%(levelname)s] [%(asctime)s]: %(message)s',
        level=logging.INFO)
logger = logging.getLogger()

command = {
        'shutdown': 'sudo shutdown -h now',
        'restart': 'sudo shutdown -r now',
        'hibernation': 'sudo pm-hibernate',
        'logoff': 'gnome-session-quit',
        'lock': 'gnome-screensaver-command -l',
}

CONFIG_PATH = os.path.join(os.environ['HOME'], '.config/statusEnforcer.txt')
HOST = 'http://10.100.58.69'
API = '%s/check_in' % HOST

key = 'tester'
SrvKey = 'fu@qy71q@_2-g_e_!3v$s5ecf)ar=ur0s@t&amp;m5_&amp;fy_elw&amp;m#%'

if not os.path.isfile(CONFIG_PATH):
    os.system('touch %s' % CONFIG_PATH)
    logger.info('Creating config file')
else:
    logger.debug('config file exists')


with open(CONFIG_PATH, 'r+') as fh:
    config = fh.read()
    if not config:
        logger.debug('Config file empty')
        name = raw_input('Name: ')
        r = requests.post(API, {'name': name, 'key': key})
        print r, r.text
        fh.write(r.text)
        
while True:
    r = requests.post(API, {'clientId': config, 'key': key})

    if r.text == 'nope':
        logger.warning('Auth failed!')

    else:
        try:
            status, reqSrvKey = r.text.split('~')
        except ValueError:
            logger.warning('Your configuration might be outdated!')
            break

        if reqSrvKey == SrvKey:
            logger.info('Received command from authentic server: %s', status)
            if status != 'ok':
                cmd = command.get(status)
                if cmd:
                    os.system(cmd)


    sleep(5)

