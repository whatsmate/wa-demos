#!/usr/bin/env python

import base64
import requests


# TODO: When you have your own Client ID and secret, put down their values here:
instanceId = "YOUR_GATEWAY_INSTANCE_ID_HERE"
clientId = "YOUR_OWN_ID_HERE"
clientSecret = "YOUR_OWN_SECRET_HERE"

# TODO: Customize the following 3 lines
number = '12025550108'  # FIXME
fullpath_to_document = "../assets/subwaymap.pdf"
fn = "anyname.pdf"

# Encode the document in base64 format
doc_base64 = None
with open(fullpath_to_document, 'rb') as doc:
    doc_base64 = base64.b64encode(doc.read())

headers = {
    'X-WM-CLIENT-ID': clientId, 
    'X-WM-CLIENT-SECRET': clientSecret
}

jsonBody = {
    'number': number,
    'document': doc_base64,
    'filename': fn
}

r = requests.post("http://api.whatsmate.net/v3/whatsapp/single/document/message/%s" % instanceId, 
    headers=headers,
    json=jsonBody)

print("Status code: " + str(r.status_code))
print("RESPONSE : " + str(r.content))
