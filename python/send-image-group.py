#!/usr/bin/env python

import base64
import requests


# TODO: When you have your own Client ID and secret, put down their values here:
instanceId = "YOUR_GATEWAY_INSTANCE_ID_HERE"
clientId = "YOUR_OWN_ID_HERE"
clientSecret = "YOUR_OWN_SECRET_HERE"

group = 'YOUR UNIQUE GROUP NAME HERE'  # TODO Specify your unique group name here
fullpath_to_photo = "../assets/cute-girl.jpg"
caption = 'Lovely Gal'

# Encode photo in base64 format
image_base64 = None
with open(fullpath_to_photo, 'rb') as image:
    image_base64 = base64.b64encode(image.read())

headers = {
    'X-WM-CLIENT-ID': clientId, 
    'X-WM-CLIENT-SECRET': clientSecret
}

jsonBody = {
    'group_name': group,
    'image': image_base64,
    'caption': caption
}

r = requests.post("http://api.whatsmate.net/v3/whatsapp/group/image/message/%s" % instanceId, 
    headers=headers,
    json=jsonBody)

print("Status code: " + str(r.status_code))
print("RESPONSE : " + str(r.content))
