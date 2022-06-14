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

# TODO: Customize the following 2 files:
group_name="YOUR UNIQUE GROUP NAME GOES HERE"
base64_image=`base64 -w 0 ../assets/cute-girl.jpg`

cat > /tmp/jsonbody.txt << _EOM_
  {
    "group_name": "$group_name",
    "caption": "Lovely Gal",
    "image": "$base64_image"
  }
_EOM_

curl --show-error -X POST \
     -H "X-WM-CLIENT-ID: $CLIENT_ID" \
     -H "X-WM-CLIENT-SECRET: $CLIENT_SECRET" \
     -H "Content-Type: application/json" \
     --data-binary @/tmp/jsonbody.txt  \
     http://api.whatsmate.net/v3/whatsapp/group/image/message/$INSTANCE_ID

echo -e "\n=== END OF DEMO ==="
