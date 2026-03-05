Sub Main_Routine()
  ''' The first parameter is the recipient's number, including the country code.
  ''' The second paramter is the content of the message.
  WhatsAppMessage_Send "12025550108", "God Loves You"   ''' TODO: Specify the recipient's number here. NOT the gateway number
End Sub


Sub WhatsAppMessage_Send(ByRef strNumber As String, ByRef strMessage As String)
  Dim INSTANCE_ID As String, CLIENT_ID As String, CLIENT_SECRET As String, API_URL As String
  Dim strJson As Variant
  Dim sHTML As String
  Dim oHttp As Object
  
  ''' TODO: Replace the following with your gateway instance ID, your Forever Green client ID and secret:
  INSTANCE_ID = "YOUR_INSTANCE_ID_HERE"
  CLIENT_ID = "YOUR_CLIENT_ID_HERE"
  CLIENT_SECRET = "YOUR_CLIENT_SECRET_HERE"
  API_URL = "https://api.whatsmate.net/v3/whatsapp/single/text/message/" & INSTANCE_ID

  strJson = "{""number"": """ & strNumber & """, ""message"": """ & strMessage & """}"

  Set oHttp = CreateObject("Msxml2.XMLHTTP")
  oHttp.Open "POST", API_URL, False
  oHttp.setRequestHeader "Content-type", "application/json"
  oHttp.setRequestHeader "X-WM-CLIENT-ID", CLIENT_ID
  oHttp.setRequestHeader "X-WM-CLIENT-SECRET", CLIENT_SECRET
  oHttp.Send strJson

  sHTML = oHttp.ResponseText
  MsgBox sHTML
End Sub