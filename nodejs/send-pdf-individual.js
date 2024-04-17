#!/usr/bin/env node

var http = require('http');
var fs = require('fs');

// Put down your own client ID and secret here:
var instanceId = "YOUR_OWN_GATEWAY_INSTANCE_ID";
var clientId = "YOUR_OWN_CLIENT_ID";
var clientSecret = "YOUR_OWN_SECRET_ID";

var jsonPayload = JSON.stringify({
    number: "12025550108",  // FIXME
    document: fs.readFileSync("../assets/subwaymap.pdf").toString('base64'),  // FIXME
    filename: "anyname.pdf", // FIXME
    caption: "Hope you like it"  // FIXME. caption is optional. Can be null.
});

var options = {
    hostname: "api.whatsmate.net",
    port: 80,
    path: "/v3/whatsapp/single/document/message/" + instanceId,
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
    console.log('Heard back from WhatsMate WhatsApp Gateway:\n');
    console.log('Status code: ' + response.statusCode);
    response.setEncoding('utf8');
    response.on('data', function (chunk) {
        console.log(chunk);
    });
});


