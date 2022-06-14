#!/bin/bash

INSTANCE_ID="YOUR_INSTANCE_ID_HERE"  # TODO: Replace it with your gateway instance ID here
CLIENT_ID="YOUR_CLIENT_ID_HERE"  # TODO: Replace it with your "Forever Green" client ID here
CLIENT_SECRET="YOUR_CLIENT_SECRET_HERE"  # TODO: Replace it with your "Forever Green" client secret here

# TODO: Specify the group creator, group name and the message content on lines 9 through 12
read -r -d '' jsonPayload << _EOM_
  {
    "group_admin": "12025550108",
    "group_name": "Happy Club",
    "url": "https://www.pinterest.com/pin/861032022481631898"
  }
_EOM_

curl -X POST \
     -H "X-WM-CLIENT-ID: $CLIENT_ID" \
     -H "X-WM-CLIENT-SECRET: $CLIENT_SECRET" \
     -H "Content-Type: application/json" \
     -d "$jsonPayload"   \
     http://api.whatsmate.net/v3/whatsapp/group/url/message/$INSTANCE_ID


