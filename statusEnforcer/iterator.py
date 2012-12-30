#!/usr/bin/env python

import os
os.environ['DJANGO_SETTINGS_MODULE'] = 'statusEnforcer.settings'

from server.models import Computer
import time
import logging

logging.basicConfig(format='[%(levelname)s] [%(asctime)s]: %(message)s',
        level=logging.INFO)
logger = logging.getLogger()

while True:
    computers = Computer.objects.all()
    for computer in computers:
        if not computer.withinThreshold():	#if the request is not within the threshold specified, then update the database
            computer.connected = False
            computer.status = "ok"
            computer.save()
            logger.info('%s not within threshold' % computer)

    time.sleep(5)
