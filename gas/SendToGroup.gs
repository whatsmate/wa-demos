// ==================================================================
// DEMO: Send an image in Google Drive to a Group
// ==================================================================
function demoSendImageInDriveToGroup() {
  var groupName = "GAS Demo Group";  // TODO: Name of a WhatsApp group that the gateway is part of
  var imageFilename = "butterfly_flower.jpg";  // TODO: This file must be uniquely named and, of course, exist in your Google Drive!!!
  
  var imageBase64 = base64encodeFileByName(imageFilename);
  if (imageBase64 == null) {
    Logger.log("Abort! Image file error: " + imageFilename);
    return;
  }
  
  sendWhatsappImageToGroup(groupName, imageBase64);
}

// ==================================================================
// DEMO: Send a PDF in Google Drive to a Group
// ==================================================================
function demoSendDocInDriveToGroup() {
  var groupName = "GAS Demo Group";  // TODO: Name of a WhatsApp group that the gateway is part of
  var docFilename = "subwaymap.pdf";   // TODO: This file must be uniquely named and, of course, exist in your Google Drive!!!
  var displayFilename = "anyname1.pdf";  // TODO: this is the name of the file that is displayed to the receiver
  
  var docBase64 = base64encodeFileByName(docFilename);
  if (docBase64 == null) {
    Logger.log("Abort! Image file error: " + docFilename);
    return;
  }
  
  sendWhatsappDocToGroup(groupName, docBase64, displayFilename);
}


/**
 * Generic function to send an image encoded in BASE64 to a group.
 */
function sendWhatsappImageToGroup(groupName, imageBase64) {
  var instanceId = "YOUR_INSTANCE_ID_HERE";  // TODO: Replace it with your gateway instance ID here
  var clientId = "YOUR_CLIENT_ID_HERE";  // TODO: Replace it with your Forever Green client ID here
  var clientSecret = "YOUR_CLIENT_SECRET_HERE";   // TODO: Replace it with your Forever Green client secret here
  
  var jsonPayload = JSON.stringify({
    group_name: groupName,
    image: imageBase64
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
  
  Logger.log("Calling API to send image to this group  " + groupName);
  UrlFetchApp.fetch("http://api.whatsmate.net/v3/whatsapp/group/image/message/" + instanceId, options);
}


/**
 * Generic function to send a DOC encoded in BASE64 to a group.
 */
function sendWhatsappDocToGroup(groupName, docBase64, filename) {
  var instanceId = "YOUR_INSTANCE_ID_HERE";  // TODO: Replace it with your gateway instance ID here
  var clientId = "YOUR_CLIENT_ID_HERE";  // TODO: Replace it with your Forever Green client ID here
  var clientSecret = "YOUR_CLIENT_SECRET_HERE";   // TODO: Replace it with your Forever Green client secret here
  
  var jsonPayload = JSON.stringify({
    group_name: groupName,
    document: docBase64,
    filename: filename    
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
  
  Logger.log("Calling API to send doc to this group  " + groupName);
  UrlFetchApp.fetch("http://api.whatsmate.net/v3/whatsapp/group/document/message/" + instanceId, options);
}


function base64encodeFileByName(filename) {  
  var file = retrieveFileByName(filename);
  if (file == null) {
    Logger.log("File cannot be found. Aborting...");
    return null;
  }
  
  var base64Content = encodeBlobAsBase64(file.getBlob());
  return base64Content;
}


function encodeBlobAsBase64(blob) {
  var base64String = Utilities.base64Encode(blob.getBytes());
  return base64String;
}


function retrieveFileById(fileId) {
  var targetFile = null;
  
  try {
    targetFile = DriveApp.getFileById(fileId);
  }
  catch(err) {
    Logger.log("File not found. ID: " + fileId); 
  }
  
  return targetFile;
}


function retrieveFileByName(filename) {  
  var matchedFiles = DriveApp.getFilesByName(filename);
  
  // assume the first match is the target
  var targetFile = null;
  if (matchedFiles.hasNext()) {
    targetFile = matchedFiles.next();
    Logger.log("Target file found. Name is  " + targetFile.getName());
    Logger.log("Target file ID is " + targetFile.getId());
  } else {
    Logger.log("File not found: " + filename);
  }
  
  return targetFile;
}

