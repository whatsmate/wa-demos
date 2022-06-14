$number = "12025550108"  #  Specify the recipient's number here. NOT the gateway number
$message = "Howdy, this is a message from PowerShell."
# To send multi-line messages, do this:
# $message = "Howdy, line one `n Line two"

$instanceId = "YOUR_INSTANCE_ID_HERE"  # TODO: Replace it with your gateway instance ID
$clientId = "YOUR_CLIENT_ID_HERE"  # TODO: Replace it with your Forever Green client ID here
$clientSecret = "YOUR_CLIENT_SECRET_HERE"   # TODO: Replace it with your Forever Green client secret here

$jsonObj = @{'number'=$number;
             'message'=$message;}

Try {
  $res = Invoke-WebRequest -Uri "http://api.whatsmate.net/v3/whatsapp/single/text/message/$instanceId" `
                          -Method Post   `
                          -Headers @{"X-WM-CLIENT-ID"=$clientId; "X-WM-CLIENT-SECRET"=$clientSecret;} `
                          -ContentType "application/json; charset=utf-8" `
                          -Body (ConvertTo-Json $jsonObj)

  Write-host "Status Code: "  $res.StatusCode
  Write-host $res.Content
}
Catch {
  $result = $_.Exception.Response.GetResponseStream()
  $reader = New-Object System.IO.StreamReader($result)
  $reader.BaseStream.Position = 0
  $reader.DiscardBufferedData()
  $responseBody = $reader.ReadToEnd();

  Write-host "Status Code: " $_.Exception.Response.StatusCode
  Write-host $responseBody
}

