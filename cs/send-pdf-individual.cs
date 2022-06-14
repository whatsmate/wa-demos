using System;
using System.Net;
using System.Web.Script.Serialization; // requires the reference 'System.Web.Extensions'
using System.IO;
using System.Text;

class WaPdfSender
{
    // TODO: Replace the following with your gateway instance ID, client ID and secret!
    private static string INSTANCE_ID = "YOUR_INSTANCE_ID";
    private static string CLIENT_ID = "YOUR_CLIENT_ID_HERE";
    private static string CLIENT_SECRET = "YOUR_CLIENT_SECRET_HERE";

    private static string DOCUMENT_SINGLE_API_URL = "http://api.whatsmate.net/v3/whatsapp/single/document/message/" + INSTANCE_ID;

    static void Main(string[] args)
    {
        WaPdfSender pdfSender = new WaPdfSender();
        // TODO: Put down your recipient's number (e.g. your own cell phone number)
        string recipient = "12025550105";
        // TODO: Remember to copy the PDF from ..\assets to the TEMP directory!
        string base64Content = convertFileToBase64("C:\\TEMP\\subwaymap.pdf");
        string fn = "anyname.pdf";
        
        pdfSender.sendDocument(recipient, base64Content, fn);

        Console.WriteLine("Press Enter to exit.");
        Console.ReadLine();
    }

    // http://stackoverflow.com/questions/25919387/c-sharp-converting-file-into-base64string-and-back-again
    static public string convertFileToBase64(string fullPathToDoc)
    {
        Byte[] bytes = File.ReadAllBytes(fullPathToDoc);
        String base64Encoded = Convert.ToBase64String(bytes);
        return base64Encoded;
    }

    public bool sendDocument(string number, string base64Content, string fn)
    {
        bool success = true;

        try
        {
            using (WebClient client = new WebClient())
            {
                client.Headers[HttpRequestHeader.ContentType] = "application/json";
                client.Headers["X-WM-CLIENT-ID"] = CLIENT_ID;
                client.Headers["X-WM-CLIENT-SECRET"] = CLIENT_SECRET;

                SingleDocPayload payloadObj = new SingleDocPayload() { number = number, document = base64Content, filename = fn};
                string postData = (new JavaScriptSerializer()).Serialize(payloadObj);

                client.Encoding = Encoding.UTF8;
                string response = client.UploadString(DOCUMENT_SINGLE_API_URL, postData);
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

    public class SingleDocPayload
    {
        public string number { get; set; }
        public string document { get; set; }
        public string filename { get; set; }
    }

}
