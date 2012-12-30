from twilio.rest import TwilioRestClient

ACCOUNT_SID = 'AC704c52f848ac4a38e1c79c261fb5be1a'
AUTH_TOKEN = 'df21f1a9fdc7e380b9e121c702dfb954'

client = TwilioRestClient(ACCOUNT_SID, AUTH_TOKEN)

for message in client.sms.messages.list():
    print message.body
