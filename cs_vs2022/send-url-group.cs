using System;
using System.Net;
using System.Text.Json;
using System.Web.Script.Serialization; // requires the reference 'System.Web.Extensions'
using System.IO;
using System.Text;

class WaUrlGroupSender
{
    // TODO: Replace the following with your gateway instance ID, client ID and secret!
    private static string INSTANCE_ID = "YOUR_INSTANCE_ID";
    private static string CLIENT_ID = "YOUR_CLIENT_ID_HERE";
    private static string CLIENT_SECRET = "YOUR_CLIENT_SECRET_HERE";

    private static string URL_GROUP_API_URL = "https://api.whatsmate.net/v3/whatsapp/group/url/message/" + INSTANCE_ID;

    static void Main(string[] args)
    {
        WaUrlGroupSender urlSender = new WaUrlGroupSender();
        // TODO: Put down your group's details below
        string groupAdmin = "12025550105";
        string groupName = "Happy Club";
        string url = "https://www.pinterest.com/pin/861032022481631898";
        
        urlSender.sendUrl(groupAdmin, groupName, url);

        Console.WriteLine("Press Enter to exit.");
        Console.ReadLine();
    }

    public bool sendUrl(string groupAdmin, string groupName, string url)
    {
        bool success = true;

        try
        {
            HttpWebRequest httpRequest = (HttpWebRequest)WebRequest.Create(URL_GROUP_API_URL);
            httpRequest.Method = "POST";
            httpRequest.Accept = "application/json";
            httpRequest.ContentType = "application/json";
            httpRequest.Headers["X-WM-CLIENT-ID"] = CLIENT_ID;
            httpRequest.Headers["X-WM-CLIENT-SECRET"] = CLIENT_SECRET;

            GroupUrlPayload payloadObj = new GroupUrlPayload() { group_admin = groupAdmin, group_name = groupName, url = url };
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

    public class GroupUrlPayload
    {
        public string group_admin{ get; set; }
        public string group_name { get; set; }
        public string url { get; set; }
    }

}
