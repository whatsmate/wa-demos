Sub Main_Routine() 
  Dim groupAdmin As String = "12025550108"  ''' TODO: Specify the WhatsApp number of the group creator, including the country code
  Dim groupName As String = "Happy Club"  ''' TODO: Specify the name of the group
  Dim message As String = "Let's party tonight!" ''' TODO: Specify the content of your message
  
  WhatsAppMessage_SendGroup groupAdmin, groupName, message
End Sub


Sub WhatsAppMessage_SendGroup(ByRef strGroupAdmin As String, ByRef strGroupName As String, ByRef strMessage As String)
  Dim INSTANCE_ID As String, CLIENT_ID As String, CLIENT_SECRET As String, API_URL As String
  Dim strJson As String
  Dim sHTML As String
  Dim oHttp As Object
  
  ''' TODO: Replace the following with your gateway instance ID, Forever Green client ID and secret:
  INSTANCE_ID = "YOUR_INSTANCE_ID_HERE"
  CLIENT_ID = "YOUR_CLIENT_ID_HERE"
  CLIENT_SECRET = "YOUR_CLIENT_SECRET_HERE"
  API_URL = "https://api.whatsmate.net/v3/whatsapp/group/text/message/" & INSTANCE_ID

  strJson = "{""group_admin"": """ & strGroupAdmin & """, ""group_name"": """ & strGroupName & """, ""message"": """ & strMessage & """}"

  Set oHttp = CreateObject("Msxml2.XMLHTTP")
  oHttp.Open "POST", API_URL, False
  oHttp.setRequestHeader "Content-type", "application/json"
  oHttp.setRequestHeader "X-WM-CLIENT-ID", CLIENT_ID
  oHttp.setRequestHeader "X-WM-CLIENT-SECRET", CLIENT_SECRET
  oHttp.Send strJson

  sHTML = oHttp.ResponseText
  MsgBox sHTML
End Sub