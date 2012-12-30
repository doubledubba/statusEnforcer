import os
os.environ['DJANGO_SETTINGS_MODULE'] = 'statusEnforcer.settings'

import json
from server.models import Computer

#get all the computers' name and their ids
data = {}

computers = Computer.objects.all()
for computer in computers:
	data["name_" + str(computer.pk)] = computer.name
	data["id_" + str(computer.pk)] = computer.pk
1	
print(json.dumps(data))
