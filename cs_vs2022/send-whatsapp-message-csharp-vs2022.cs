using System;
using System.Net;
using System.Text.Json;
using System.IO;
using System.Text;

class WaMessageSender
{
    // TODO: Replace the following with your gateway instance ID, Forever Green client ID and secret:
    private static string INSTANCE_ID = "YOUR_INSTANCE_ID_HERE";
    private static string CLIENT_ID = "YOUR_CLIENT_ID_HERE";
    private static string CLIENT_SECRET = "YOUR_CLIENT_SECRET_HERE";

    private static string API_URL = "https://api.whatsmate.net/v3/whatsapp/single/text/message/" + INSTANCE_ID;

    static void Main(string[] args)
    {
        WaMessageSender msgSender = new WaMessageSender();
        msgSender.sendMessage("12025550108", "Isn't this exciting?");  // TODO: Specify the recipient's number here. NOT the gateway number
        Console.WriteLine("Press Enter to exit.");
        Console.ReadLine();
    }

    public bool sendMessage(string number, string message)
    {
        bool success = true;

        try
        {
            HttpWebRequest httpRequest = (HttpWebRequest)WebRequest.Create(API_URL);
            httpRequest.Method = "POST";
            httpRequest.Accept = "application/json";
            httpRequest.ContentType = "application/json";
            httpRequest.Headers["X-WM-CLIENT-ID"] = CLIENT_ID;
            httpRequest.Headers["X-WM-CLIENT-SECRET"] = CLIENT_SECRET;

            Payload payloadObj = new Payload() { number = number, message = message };
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

    public class Payload
    {
        public string number { get; set; }
        public string message { get; set; }
    }

}