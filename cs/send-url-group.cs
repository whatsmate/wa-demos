using System;
using System.Net;
using System.Web.Script.Serialization; // requires the reference 'System.Web.Extensions'
using System.IO;
using System.Text;

class WaUrlGroupSender
{
    // TODO: Replace the following with your gateway instance ID, client ID and secret!
    private static string INSTANCE_ID = "YOUR_INSTANCE_ID";
    private static string CLIENT_ID = "YOUR_CLIENT_ID_HERE";
    private static string CLIENT_SECRET = "YOUR_CLIENT_SECRET_HERE";

    private static string URL_GROUP_API_URL = "http://api.whatsmate.net/v3/whatsapp/group/url/message/" + INSTANCE_ID;

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
            using (WebClient client = new WebClient())
            {
                client.Headers[HttpRequestHeader.ContentType] = "application/json";
                client.Headers["X-WM-CLIENT-ID"] = CLIENT_ID;
                client.Headers["X-WM-CLIENT-SECRET"] = CLIENT_SECRET;

                GroupUrlPayload payloadObj = new GroupUrlPayload() { group_admin = groupAdmin, group_name = groupName, url = url };
                string postData = (new JavaScriptSerializer()).Serialize(payloadObj);

                client.Encoding = Encoding.UTF8;
                string response = client.UploadString(URL_GROUP_API_URL, postData);
                Console.WriteLine(response);
            }
        }
        catch (WebException webEx)
        {
            Console.WriteLine(((HttpWebResponse)webEx.Response).StatusCode);
            Stream stream = ((HttpWebResponse)webEx.Response).GetResponseStream();
            StreamReader reader = new StreamReader(stream);
            String body = reader.ReadToEnd();
            Console.WriteLine(body);
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
