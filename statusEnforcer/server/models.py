from django.db import models

from datetime import datetime
from statusEnforcer.settings import threshold

class Computer(models.Model):
    name = models.CharField(max_length=255)
    lastConnection = models.DateTimeField()
    status = models.CharField(max_length=80)
    connected = models.BooleanField(default=False)

    def withinThreshold(self):
        '''Return True or False.'''
    
        if self.activityLength() < threshold:
            return True
        else:
            return False

    def activityLength(self, now=datetime.now()):
        '''Return an integer of how long it has been since the last
        connection'''

        return abs(now - self.lastConnection)
		#comment

