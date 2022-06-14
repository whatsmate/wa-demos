<?php
  $INSTANCE_ID = 'YOUR_INSTANCE_ID_HERE';  // TODO: Replace it with your gateway instance ID here
  $CLIENT_ID = "YOUR_CLIENT_ID_HERE";  // TODO: Replace it with your Premium client ID here
  $CLIENT_SECRET = "YOUR_CLIENT_SECRET_HERE";   // TODO: Replace it with your Premium client secret here

  $pathToImage = "/tmp/your_image.jpg";    // TODO: Replace it with the path to your image
  $imageData = file_get_contents($pathToImage);
  $base64Image = base64_encode($imageData);

  $postData = array(
    'group_name' => 'YOUR UNIQUE GROUP NAME HERE',  // TODO: Specify the unique name of your group here.
    'image' => $base64Image,
    'caption' => 'Lovely Gal'
  );

  $headers = array(
    'Content-Type: application/json',
    'X-WM-CLIENT-ID: '.$CLIENT_ID,
    'X-WM-CLIENT-SECRET: '.$CLIENT_SECRET
  );

  $url = 'http://api.whatsmate.net/v3/whatsapp/group/image/message/' . $INSTANCE_ID;
  $ch = curl_init($url);
  curl_setopt($ch, CURLOPT_POST, 1);
  curl_setopt($ch, CURLOPT_RETURNTRANSFER, true);
  curl_setopt($ch, CURLOPT_HTTPHEADER, $headers);
  curl_setopt($ch, CURLOPT_POSTFIELDS, json_encode($postData));
  $response = curl_exec($ch);
  echo "Response: ".$response;
  curl_close($ch);
?>
