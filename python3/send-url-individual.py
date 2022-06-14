#!/usr/bin/env python3


import requests

# TODO: When you have your own Client ID and secret, put down their values here:
instanceId = "YOUR_GATEWAY_INSTANCE_ID_HERE"
clientId = "YOUR_OWN_ID_HERE"
clientSecret = "YOUR_OWN_SECRET_HERE"

# TODO: Customize the following 3 lines
number = '12025550108'  # FIXME
url = "https://www.pinterest.com/pin/861032022481631898"  # FIXME

headers = {
    'X-WM-CLIENT-ID': clientId, 
    'X-WM-CLIENT-SECRET': clientSecret
}

jsonBody = {
    'number': number,
    'url': url
}

r = requests.post("http://api.whatsmate.net/v3/whatsapp/single/url/message/%s" % instanceId, 
    headers=headers,
    json=jsonBody)

print("Status code: " + str(r.status_code))
print("RESPONSE : " + str(r.content))
