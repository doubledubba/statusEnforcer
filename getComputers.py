import json
from server.models import Computer

#get all the computers' name and their ids
data = {}

computers = Computer.objects.all()
for computer in computers:
	data["name_" + computer.pk] = computer.name
	data["id_" + computer.pk] = computer.pk
	
print(json.dumps(data))