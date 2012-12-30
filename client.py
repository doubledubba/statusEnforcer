import requests

r = requests.post('http://localhost:8000/check_in', {'clientId': 1})
print r
