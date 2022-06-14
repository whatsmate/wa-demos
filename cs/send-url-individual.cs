using System;
using System.Net;
using System.Web.Script.Serialization; // requires the reference 'System.Web.Extensions'
using System.IO;
using System.Text;

class WaUrlSender
{
    // TODO: Replace the following with your gateway instance ID, client ID and secret!
    private static string INSTANCE_ID = "YOUR_INSTANCE_ID";
    private static string CLIENT_ID = "YOUR_CLIENT_ID_HERE";
    private static string CLIENT_SECRET = "YOUR_CLIENT_SECRET_HERE";

    private static string URL_SINGLE_API_URL = "http://api.whatsmate.net/v3/whatsapp/single/url/message/" + INSTANCE_ID;

    static void Main(string[] args)
    {
        WaUrlSender urlSender = new WaUrlSender();
        // TODO: Put down your recipient's number (e.g. your own cell phone number)
        string recipient = "12025550105";
        string url = "https://www.pinterest.com/pin/861032022481631898";
        
        urlSender.sendUrl(recipient, url);

        Console.WriteLine("Press Enter to exit.");
        Console.ReadLine();
    }

    public bool sendUrl(string number, string url)
    {
        bool success = true;

        try
        {
            using (WebClient client = new WebClient())
            {
                client.Headers[HttpRequestHeader.ContentType] = "application/json";
                client.Headers["X-WM-CLIENT-ID"] = CLIENT_ID;
                client.Headers["X-WM-CLIENT-SECRET"] = CLIENT_SECRET;

                SingleUrlPayload payloadObj = new SingleUrlPayload() { number = number, url = url };
                string postData = (new JavaScriptSerializer()).Serialize(payloadObj);

                client.Encoding = Encoding.UTF8;
                string response = client.UploadString(URL_SINGLE_API_URL, postData);
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

    public class SingleUrlPayload
    {
        public string number { get; set; }
        public string url { get; set; }
    }

}
