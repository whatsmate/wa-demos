#!/usr/bin/env python3

import requests

# TODO: When you have your own Client ID and secret, put down their values here:
instanceId = "YOUR_GATEWAY_INSTANCE_ID_HERE"
clientId = "YOUR_OWN_ID_HERE"
clientSecret = "YOUR_OWN_SECRET_HERE"

# TODO: Customize the following 3 lines
groupName = 'Very Unique Group'  # FIXME
groupAdmin = "13238471232"  # FIXME
message = "This is exciting, isn't it!"  # FIXME

headers = {
    'X-WM-CLIENT-ID': clientId, 
    'X-WM-CLIENT-SECRET': clientSecret
}

jsonBody = {
    'group_name': groupName,
    'group_admin': groupAdmin,
    'message': message
}

r = requests.post("http://api.whatsmate.net/v3/whatsapp/group/text/message/%s" % instanceId, 
    headers=headers,
    json=jsonBody)

print("Status code: " + str(r.status_code))
print("RESPONSE : " + str(r.content))

