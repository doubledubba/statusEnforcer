import os
os.environ['DJANGO_SETTINGS_MODULE'] = 'statusEnforcer.settings'

from server.models import Computer
import time

computers = Computer.objects.all()
for computer in computers:
    if not computer.withinThreshold():	#if the request is not within the threshold specified, then update the database
        computer.connected = False
        computer.status = "ok"
        computer.save()
        print 'Not within threshold'
