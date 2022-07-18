using System;
using System.Net;
using System.Text.Json;
using System.IO;
using System.Text;

class WaTextGroupSender
{
    // TODO: Replace the following with your gateway instance ID, client ID and secret!
    private static string INSTANCE_ID = "YOUR_INSTANCE_ID";
    private static string CLIENT_ID = "YOUR_CLIENT_ID_HERE";
    private static string CLIENT_SECRET = "YOUR_CLIENT_SECRET_HERE";

    private static string TEXT_GROUP_API_URL = "https://api.whatsmate.net/v3/whatsapp/group/text/message/" + INSTANCE_ID;

    static void Main(string[] args)
    {
        WaTextGroupSender urlSender = new WaTextGroupSender();
        // TODO: Put down your group's details below
        string groupAdmin = "12025550105";
        string groupName = "Happy Club";
        string message = "Guys, let's party tonight!";  // TODO: Specify the content of your message
        
        urlSender.sendGroupText(groupAdmin, groupName, message);

        Console.WriteLine("Press Enter to exit.");
        Console.ReadLine();
    }

    public bool sendGroupText(string groupAdmin, string groupName, string message)
    {
        bool success = true;

        try
        {
            HttpWebRequest httpRequest = (HttpWebRequest)WebRequest.Create(TEXT_GROUP_API_URL);
            httpRequest.Method = "POST";
            httpRequest.Accept = "application/json";
            httpRequest.ContentType = "application/json";
            httpRequest.Headers["X-WM-CLIENT-ID"] = CLIENT_ID;
            httpRequest.Headers["X-WM-CLIENT-SECRET"] = CLIENT_SECRET;

            SingleUrlPayload payloadObj = new SingleUrlPayload() { number = number, url = url };
            string postData = JsonSerializer.Serialize(payloadObj);

            using (var streamWriter = new StreamWriter(httpRequest.GetRequestStream()))
            {
                streamWriter.Write(postData);
            }

            var httpResponse = (HttpWebResponse)httpRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();
                Console.WriteLine(result);
            }
        }
        catch (WebException webExcp)
        {
            Console.WriteLine("A WebException has been caught.");
            Console.WriteLine(webExcp.ToString());
            WebExceptionStatus status = webExcp.Status;
            if (status == WebExceptionStatus.ProtocolError)
            {
                Console.Write("The REST API server returned a protocol error: ");
                HttpWebResponse? httpResponse = webExcp.Response as HttpWebResponse;
                Stream stream = httpResponse.GetResponseStream();
                StreamReader reader = new StreamReader(stream);
                String body = reader.ReadToEnd();
                Console.WriteLine((int)httpResponse.StatusCode + " - " + body);
                success = false;
            }
        }
        catch (Exception e)
        {
            Console.WriteLine("A general exception has been caught.");
            Console.WriteLine(e.ToString());
            success = false;
        }


        return success;
    }

    public class GroupTextPayload
    {
        public string group_admin{ get; set; }
        public string group_name { get; set; }
        public string message { get; set; }
    }

}
