using System;
using System.Net;
using System.Text.Json;
using System.IO;
using System.Text;

class WaImageSender
{
    // TODO: Replace the following with your gateway instance ID, client ID and secret!
    private static string INSTANCE_ID = "YOUR_INSTANCE_ID";
    private static string CLIENT_ID = "YOUR_CLIENT_ID_HERE";
    private static string CLIENT_SECRET = "YOUR_CLIENT_SECRET_HERE";

    private static string IMAGE_SINGLE_API_URL = "https://api.whatsmate.net/v3/whatsapp/single/image/message/" + INSTANCE_ID;

    static void Main(string[] args)
    {
        WaImageSender imgSender = new WaImageSender();
        // TODO: Put down your recipient's number (e.g. your own cell phone number)
        string recipient = "12025550105";
        // TODO: Remember to copy the JPG from ..\assets to the TEMP directory!
        string base64Content = convertFileToBase64("C:\\TEMP\\cute-girl.jpg");
        string caption = "Lovely Gal";
        
        imgSender.sendImage(recipient, base64Content, caption);

        Console.WriteLine("Press Enter to exit.");
        Console.ReadLine();
    }

    // http://stackoverflow.com/questions/25919387/c-sharp-converting-file-into-base64string-and-back-again
    static public string convertFileToBase64(string fullPathToImage)
    {
        Byte[] bytes = File.ReadAllBytes(fullPathToImage);
        String base64Encoded = Convert.ToBase64String(bytes);
        return base64Encoded;
    }

    public bool sendImage(string number, string base64Content, string caption)
    {
        bool success = true;

        try
        {
            HttpWebRequest httpRequest = (HttpWebRequest)WebRequest.Create(IMAGE_SINGLE_API_URL);
            httpRequest.Method = "POST";
            httpRequest.Accept = "application/json";
            httpRequest.ContentType = "application/json";
            httpRequest.Headers["X-WM-CLIENT-ID"] = CLIENT_ID;
            httpRequest.Headers["X-WM-CLIENT-SECRET"] = CLIENT_SECRET;

            SingleImagePayload payloadObj = new SingleImagePayload() { number = number, caption = caption, image = base64Content};
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

    public class SingleImagePayload
    {
        public string number { get; set; }
        public string caption { get; set; }
        public string image { get; set; }
    }

}
