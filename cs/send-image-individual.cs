using System;
using System.Net;
using System.Web.Script.Serialization; // requires the reference 'System.Web.Extensions'
using System.IO;
using System.Text;

class WaImageSender
{
    // TODO: Replace the following with your gateway instance ID, client ID and secret!
    private static string INSTANCE_ID = "YOUR_INSTANCE_ID";
    private static string CLIENT_ID = "YOUR_CLIENT_ID_HERE";
    private static string CLIENT_SECRET = "YOUR_CLIENT_SECRET_HERE";

    private static string IMAGE_SINGLE_API_URL = "http://api.whatsmate.net/v3/whatsapp/single/image/message/" + INSTANCE_ID;

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
            using (WebClient client = new WebClient())
            {
                client.Headers[HttpRequestHeader.ContentType] = "application/json";
                client.Headers["X-WM-CLIENT-ID"] = CLIENT_ID;
                client.Headers["X-WM-CLIENT-SECRET"] = CLIENT_SECRET;

                SingleImagePayload payloadObj = new SingleImagePayload() { number = number, caption = caption, image = base64Content};
                string postData = (new JavaScriptSerializer()).Serialize(payloadObj);

                client.Encoding = Encoding.UTF8;
                string response = client.UploadString(IMAGE_SINGLE_API_URL, postData);
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

    public class SingleImagePayload
    {
        public string number { get; set; }
        public string caption { get; set; }
        public string image { get; set; }
    }

}
