#!/usr/bin/env python3

import requests

# TODO: When you have your own Client ID and secret, put down their values here:
instanceId = "YOUR_GATEWAY_INSTANCE_ID_HERE"
clientId = "YOUR_OWN_ID_HERE"
clientSecret = "YOUR_OWN_SECRET_HERE"


# TODO: Customize the following 2 lines
number = '12025550108'  # FIXME: Put down your target number here.
message = "This is exciting, isn't it!"  # FIXME: Put down your own text message here.

headers = {
    'X-WM-CLIENT-ID': clientId, 
    'X-WM-CLIENT-SECRET': clientSecret
}

jsonBody = {
    'number': number,
    'message': message
}

r = requests.post("https://api.whatsmate.net/v3/whatsapp/single/text/message/%s" % instanceId, 
    headers=headers,
    json=jsonBody)

print("Status code: " + str(r.status_code))
print("RESPONSE : " + str(r.content))
