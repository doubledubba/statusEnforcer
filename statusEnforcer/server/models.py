from django.db import models

from datetime import datetime
from statusEnforcer.settings import threshold

from pytz import utc

class Computer(models.Model):
    name = models.CharField(max_length=255)
    lastConnection = models.DateTimeField()
    status = models.CharField(max_length=80)
    connected = models.BooleanField(default=False)

    def __unicode__(self):
        return self.name

    def get_absolute_url(self):
        return '/listing/%d' % self.pk

    def withinThreshold(self):
        '''Return True or False.'''
    
        if self.activityLength() < threshold:
            return True
        else:
            return False

    def activityLength(self):
        '''Return an integer of how long it has been since the last
        connection'''
    
        now = datetime.now(utc)
        delta = now - self.lastConnection
        return delta.seconds

