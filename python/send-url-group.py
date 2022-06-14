#!/usr/bin/env python

import requests

# TODO: When you have your own Client ID and secret, put down their values here:
instanceId = "YOUR_GATEWAY_INSTANCE_ID_HERE"
clientId = "YOUR_OWN_ID_HERE"
clientSecret = "YOUR_OWN_SECRET_HERE"

# TODO: Customize the following 3 lines
group_admin = "12025550108"  # FIXME
group_name = "Happy Club"   # FIXME
url = "https://www.pinterest.com/pin/861032022481631898"  # FIXME

headers = {
    'X-WM-CLIENT-ID': clientId, 
    'X-WM-CLIENT-SECRET': clientSecret
}

jsonBody = {
    'group_admin': group_admin,
    'group_name': group_name,
    'url': url
}

r = requests.post("http://api.whatsmate.net/v3/whatsapp/group/url/message/%s" % instanceId, 
    headers=headers,
    json=jsonBody)

print("Status code: " + str(r.status_code))
print("RESPONSE : " + str(r.content))
