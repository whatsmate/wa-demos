
function demoSendPdfInDrive() {
  var destNumber = "12025550108";  // TODO: Specify the recipients number here. NOT THE GATEWAY NUMBER!
  var pdfFilename = "subwaymap.pdf";  // TODO: This file must be uniquely named and, of course, exist in your Google Drive!!!
  
  var pdfBase64 = base64encodeFileByName(pdfFilename);
  if (pdfBase64 == null) {
    Logger.log("Abort! PDF file error: " + pdfFilename);
    return;
  }
  
  sendWhatsappPdf(destNumber, pdfBase64);
}


function sendWhatsappPdf(destNumber, pdfBase64) {
  var instanceId = "YOUR_INSTANCE_ID_HERE";  // TODO: Replace it with your gateway instance ID here
  var clientId = "YOUR_CLIENT_ID_HERE";  // TODO: Replace it with your Forever Green client ID here
  var clientSecret = "YOUR_CLIENT_SECRET_HERE";   // TODO: Replace it with your Forever Green client secret here
  
  var jsonPayload = JSON.stringify({
    number: destNumber,
    document: pdfBase64,
    filename: "anyname.pdf"
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
  
  Logger.log("Calling API to send PDF to this number  " + destNumber);
  UrlFetchApp.fetch("http://api.whatsmate.net/v3/whatsapp/single/document/message/" + instanceId, options);
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

