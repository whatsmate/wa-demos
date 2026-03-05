function main() {
  var groupAdmin = "12025550108"; // TODO: Specify the WhatsApp number of the group creator, including the country code
  var groupName = "Happy Club";   // TODO: Specify the name of the group
  var message = "Guys, let's party tonight!";  // TODO: Specify the content of your message
  sendWhatsappGroup(groupAdmin, groupName, message);
}


function sendWhatsappGroup(groupAdmin, groupName, message) {
  var instanceId = "YOUR_INSTANCE_ID_HERE";  // TODO: Replace it with your gateway instance ID here
  var clientId = "YOUR_CLIENT_ID_HERE";  // TODO: Replace it with your Forever Green client ID here
  var clientSecret = "YOUR_CLIENT_SECRET_HERE";   // TODO: Replace it with your Forever Green client secret here
  
  var jsonPayload = JSON.stringify({
    group_admin: groupAdmin,
    group_name: groupName,
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
    
  UrlFetchApp.fetch("https://api.whatsmate.net/v3/whatsapp/group/text/message/" + instanceId, options);
}