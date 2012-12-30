import requests

r = requests.post('http://10.100.58.69/check_in', {'clientId': 1})

print r
print 'Connection ok:', r.ok
print 'Server says:', r.text
