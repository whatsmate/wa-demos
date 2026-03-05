$groupAdmin = "12025550108" # TODO: Specify the WhatsApp number of the group creator, including the country code
$groupName = "Happy Club"   # TODO: Specify the name of the group
$message = "Guys, let's party tonight!" # TODO: Specify the content of your message

$instanceId = "YOUR_INSTANCE_ID_HERE"  # TODO: Replace it with your gateway instance ID here
$clientId = "YOUR_CLIENT_ID_HERE"  # TODO: Replace it with your Forever Green client ID here
$clientSecret = "YOUR_CLIENT_SECRET_HERE"  # TODO: Replace it with your Forever Green client secret here

$jsonObj = @{'group_admin'=$groupAdmin;
             'group_name'=$groupName;
             'message'=$message;}

Try {
  $res = Invoke-WebRequest -Uri "https://api.whatsmate.net/v3/whatsapp/group/text/message/$instanceId" `
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