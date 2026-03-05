function main() {
  var destNumber = "12025550108";  // TODO: Specify the recipient's number here. NOT THE GATEWAY NUMBER!
  var message = "Aloha, this is my first message.";
  sendWhatsapp(destNumber, message);
}


function sendWhatsapp(destNumber, message) {
  var instanceId = "YOUR_INSTANCE_ID_HERE";  // TODO: Replace it with your gateway instance ID here
  var clientId = "YOUR_CLIENT_ID_HERE";  // TODO: Replace it with your Forever Green client ID here
  var clientSecret = "YOUR_CLIENT_SECRET_HERE";   // TODO: Replace it with your Forever Green client secret here
  
  var jsonPayload = JSON.stringify({
    number: destNumber,
    message: message
  });
  
  var options = {
    "method" : "post",
    "contentType": "application/json",
    "headers": {
      "X-WM-CLIENT-ID": clientId,
      "X-WM-CLIENT-SECRET": clientSecret
    },
    "payload" : jsonPayload,
    "Content-Length": jsonPayload.length
  };
    
  UrlFetchApp.fetch("https://api.whatsmate.net/v3/whatsapp/single/text/message/" + instanceId, options);
}
