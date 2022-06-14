#!/usr/bin/env nodejs

var express = require('express')
var bodyParser = require('body-parser');
var request = require('request');


/**
 * This applicaion depends on the 3rd-party libraries specified in package.json
 * Therefore, you need to execute the following command to install the dependencies before running this application:
 *     npm install 
 *
 *    * * * * * * * * * * * * * * * * * * * * * * * * * * *
 *
 * You will need to sign up for the WhatsApp Gateway for Enterprise plan in order for this script to work for you.
 * https://www.whatsmate.net/whatsapp-gateway-enterprise.html
 *
 * This is a simplistic application that demonstrates how you can receive WhatsApp messages
 * from your dedicated Enterprise WhatsApp Gateway and reply to them accordingly.
 *
 * The following diagram illustrates how messages flow between your application and your customer.
 *     .-----------------------------------------------------------------------------------------.
 *     | This application      |      The Enterprise WhatsApp GW      |      Customer's WhatsApp |
 *     '-----------------------------------------------------------------------------------------'
 * (1)                                                              <---     "Hello"
 * (2)                       <---     "Hello"
 * (3)   "Choose a product"  ---> 
 * (4)                                "Choose a product"            ---> 
 * (5)                                                              <---     "1"
 * (6)                       <---     "1"
 * (7)   "Price: 90.0 "      ---> 
 * (8)                                "Price: 90.0 "                ---> 
 */


// TCP port that this webhook application listens on
var WEBHOOK_PORT = 9999;
// Put down your own client ID and secret here:
var CLIENT_ID = "YOUR_OWN_CLIENT_ID";
var CLIENT_SECRET = "YOUR_OWN_SECRET_ID";



// Instantiate an Express application instance
var app = express()
app.use(bodyParser.json()); // for parsing application/json


/**
 * User needs to write this function to define
 * how they would like to handle incoming messages.
 */
function handleMessageReceived(gwInstance, gwNumber, senderNumber, message) {
    console.log("\nReceived a message from gateway: " + gwNumber + "; instance: " + gwInstance)
    console.log("Message from customer: " + senderNumber)
    console.log("The message content is: " + message)

    var responseMsg = "To be defined";
    switch (message) {
      case '1':
        responseMsg = "Price: 90.0";
        break;
      case '2':
        responseMsg = "Price: 380.0";
        break;
      case '3':
        responseMsg = "Price: 990.0";
        break;
      default:
        responseMsg = "Choose a product. Send \n'1' for Product X. \n'2' for Product 'Y' and so on."
    }

    sendWhatsAppMessage(gwInstance, senderNumber, responseMsg)
}


/**
 * Sends a message to a destination number.
 */
function sendWhatsAppMessage(gwInstanceId, destinationNumber, message) {
    var jsonPayloadObj = {
        'number': destinationNumber,
        'message': message
    };

    request({
        url: 'https://enterprise.whatsmate.net/v3/whatsapp/single/text/message/' + gwInstanceId,
        method: "POST",
        json: true,
        headers: {
            "X-WM-CLIENT-ID": CLIENT_ID,
            "X-WM-CLIENT-SECRET": CLIENT_SECRET,
        },
        body: jsonPayloadObj
    }, function (error, response, body){
        console.log('\nJust called the REST API to send a reply to ' + destinationNumber);
        console.log('Status code was ' + response.statusCode);
        console.log('Content was: ' + message);
    });
}


/**
 * Define the behavior the webhook
 */
app.post('/webhook', function (req, res) {
    var srcNumber = req.body.number;
    var strMessage = req.body.message;
    var strGwInstance = req.body.instance_id;
    var strGwNumber = req.body.gateway_number;
    handleMessageReceived(strGwInstance, strGwNumber, srcNumber, strMessage);
    res.send('Data received')
})


/**
 * Start the Express application server
 */
app.listen(WEBHOOK_PORT, function () {
    console.log('Demo app listening on port ' + WEBHOOK_PORT)
})

