#!/usr/bin/env node

var http = require('http');

var instanceId = "YOUR_INSTANCE_ID_HERE"; // TODO: Replace it with your gateway instance ID here
var clientId = "YOUR_CLIENT_ID_HERE"; // TODO: Replace it with your Forever Green client ID here
var clientSecret = "YOUR_CLIENT_SECRET_HERE";  // TODO: Replace it with your Forever Green client secret here

var jsonPayload = JSON.stringify({
    group_admin: "12025550108", // TODO: Specify the WhatsApp number of the group creator, including the country code
    group_name: "Happy Club",   // TODO:  Specify the name of the group
    message: "Howdy, isn't this exciting?"  // TODO: Specify the content of your message
});

var options = {
    hostname: "api.whatsmate.net",
    port: 80,
    path: "/v3/whatsapp/group/text/message/" + instanceId,
    method: "POST",
    headers: {
        "Content-Type": "application/json",
        "X-WM-CLIENT-ID": clientId,
        "X-WM-CLIENT-SECRET": clientSecret,
        "Content-Length": Buffer.byteLength(jsonPayload)
    }
};

var request = new http.ClientRequest(options);
request.end(jsonPayload);

request.on('response', function (response) {
    console.log('Heard back from the WhatsMate WA Gateway:\n');
    console.log('Status code: ' + response.statusCode);
    response.setEncoding('utf8');
    response.on('data', function (chunk) {
        console.log(chunk);
    });
});