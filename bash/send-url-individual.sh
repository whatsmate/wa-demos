#!/bin/bash

# TODO: Put down your own Client ID and secret here:
INSTANCE_ID="YOUR_INSTANCE_ID_HERE"  # TODO: Replace it with your gateway instance ID here
CLIENT_ID="YOUR_CLIENT_ID_HERE"  # TODO: Replace it with your "Forever Green" client ID here
CLIENT_SECRET="YOUR_CLIENT_SECRET_HERE"  # TODO: Replace it with your "Forever Green" client secret here

# TODO: Customize the following 2 files:
number="85291396441"
url="https://www.pinterest.com/pin/861032022481631898"

cat > /tmp/jsonbody.txt << _EOM_
  {
    "number": "$number",
    "url": "$url"
  }
_EOM_

curl --show-error -X POST \
     -H "X-WM-CLIENT-ID: $CLIENT_ID" \
     -H "X-WM-CLIENT-SECRET: $CLIENT_SECRET" \
     -H "Content-Type: application/json" \
     --data-binary @/tmp/jsonbody.txt  \
     http://api.whatsmate.net/v3/whatsapp/single/url/message/$INSTANCE_ID

echo -e "\n=== END OF DEMO ==="
