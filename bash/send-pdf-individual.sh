#!/bin/bash

#######################################################################
# You will need to have the utility `base64` available on your Linux
# system to run this script.
#
# To install it:
# sudo apt-get install coreutils
#######################################################################


# TODO: Put down your own Client ID and secret here:
INSTANCE_ID="YOUR_OWN_INSTANCE_ID_HERE"
CLIENT_ID="YOUR_OWN_CLIENT_ID_HERE"
CLIENT_SECRET="YOUR_OWN_SECRET_HERE"

# TODO: Customize the following 3 files:
number="12025550108"
base64_document=`base64 -w 0 ../assets/subwaymap.pdf`
fn="map.pdf"
caption="You will find it handy."  # optional field

cat > /tmp/jsonbody.txt << _EOM_
  {
    "number": "$number",
    "document": "$base64_document",
    "filename": "$fn",
    "caption": "$caption"
  }
_EOM_

curl --show-error -X POST \
     -H "X-WM-CLIENT-ID: $CLIENT_ID" \
     -H "X-WM-CLIENT-SECRET: $CLIENT_SECRET" \
     -H "Content-Type: application/json" \
     --data-binary @/tmp/jsonbody.txt  \
     http://api.whatsmate.net/v3/whatsapp/single/document/message/$INSTANCE_ID

echo -e "\n=== END OF DEMO ==="
