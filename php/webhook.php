<?php

// ###############################################################################################
//  You will need to sign up for the Enterprise plan in order for this script to work for you.
//  https://www.whatsmate.net/whatsapp-gateway-enterprise.html
// 
//  This is a simplistic application that demonstrates how you can receive WhatsApp messages
//  from your Enterprise WhatsApp Gateway for Enterprise and reply to them accordingly.
//  
//  On Ubuntu or similar Linux system, you need to have the php-curl library installed for this PHP to work:
//    sudo apt-get install php-curl 
//  
//  The following diagram illustrates how messages flow between your application and your customer.
//      .-----------------------------------------------------------------------------------------.
//      | This application      |      The Enterprise WhatsApp GW      |      Customer's WhatsApp |
//      '-----------------------------------------------------------------------------------------'
//   (1)                                                              <---     "Hello"
//   (2)                       <---     "Hello"
//   (3)   "Choose a product"  ---> 
//   (4)                                "Choose a product"            ---> 
//   (5)                                                              <---     "1"
//   (6)                       <---     "1"
//   (7)   "Price: 90.0 "      ---> 
//   (8)                                "Price: 90.0 "                ---> 
// ###############################################################################################


// #########################a
// # TODO: Please put down your own Client ID and secret here:
// ##########################
$CLIENT_ID = "YOUR_OWN_CLIENT_ID";
$CLIENT_SECRET = "YOUR_OWN_CLIENT_SECRET";


// Step 1: Parse the JSON that contains the WhatsApp text message received by the gateway
// Sample JSON:
//     {
//       "token": "XYZ",
//       "type": "individual",
//       "instance_id": "99",
//       "gateway_number": "85698765432",
//       "number": "9156785432",
//       "message": "Please give me the product info"
//     }
$webhookData = json_decode(file_get_contents('php://input'));
$token = $webhookData->{"token"};
$type = $webhookData->{"type"};
$instanceId = $webhookData->{"instance_id"};
$gatewayNumber = $webhookData->{"gateway_number"};
$senderNumber = $webhookData->{"number"};
$message = $webhookData->{"message"};

// Step 2: Decide how to respond to the user's query
$replyMessage = "Choose a product. Send \n1 for product X.\n2 for product Y and so on.";
if ($message == "1") {
    $replyMessage = "Price of X: $90";
} elseif ($message == "2") {
    $replyMessage = "Price of Y: $100";
} elseif ($message == "3") {
    $replyMessage = "Price of Y: $200";
}

// Step 3: Actually send the response to the user via the gateway
sendWaMessage($instanceId, $senderNumber, $replyMessage);

// Step 4: Send 200 OK to the webhook notifier
echo("Data received");



// ############################################a
// Convenience function to send a WA message
// ############################################a
function sendWaMessage($instanceId, $destNumber, $message) {
    global $CLIENT_ID, $CLIENT_SECRET;

    $postData = array(
      'number' => $destNumber,
      'message' => $message
    );

    $headers = array(
      'Content-Type: application/json',
      'X-WM-CLIENT-ID: '.$CLIENT_ID,
      'X-WM-CLIENT-SECRET: '.$CLIENT_SECRET
    );

    $url = 'http://enterprise.whatsmate.net/v3/whatsapp/single/text/message/' . $instanceId;

    $ch = curl_init($url);
    curl_setopt($ch, CURLOPT_POST, 1);
    curl_setopt($ch, CURLOPT_RETURNTRANSFER, true);
    curl_setopt($ch, CURLOPT_HTTPHEADER, $headers);
    curl_setopt($ch, CURLOPT_POSTFIELDS, json_encode($postData));
    $response = curl_exec($ch);
    curl_close($ch);

    return $response;
}

?>
