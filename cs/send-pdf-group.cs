using System;
using System.Net;
using System.Web.Script.Serialization; // requires the reference 'System.Web.Extensions'
using System.IO;
using System.Text;

class WaPdfGroupSender
{
    // TODO: Replace the following with your gateway instance ID, client ID and secret!
    private static string INSTANCE_ID = "YOUR_INSTANCE_ID";
    private static string CLIENT_ID = "YOUR_CLIENT_ID_HERE";
    private static string CLIENT_SECRET = "YOUR_CLIENT_SECRET_HERE";

    private static string DOCUMENT_SINGLE_API_URL = "http://api.whatsmate.net/v3/whatsapp/group/document/message/" + INSTANCE_ID;

    static void Main(string[] args)
    {
        WaPdfGroupSender groupDocSender = new WaPdfGroupSender();
        // TODO: Put down the unique name of your group here
        string group = "YOUR UNIQUE GROUP NAME HERE";
        // TODO: Remember to copy the JPG from ..\assets to the TEMP directory!
        string base64Content = convertFileToBase64("C:\\TEMP\\subwaymap.pdf");
        string fn = "anyname.pdf";
        string caption = "You will find the map handy.";
        
        groupDocSender.sendGroupDocument(group, base64Content, fn, caption);

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

    public bool sendGroupDocument(string group, string base64Content, string fn, string caption)
    {
        bool success = true;

        try
        {
            using (WebClient client = new WebClient())
            {
                client.Headers[HttpRequestHeader.ContentType] = "application/json";
                client.Headers["X-WM-CLIENT-ID"] = CLIENT_ID;
                client.Headers["X-WM-CLIENT-SECRET"] = CLIENT_SECRET;

                GroupDocumentPayload payloadObj = new GroupDocumentPayload() { group_name = group, document = base64Content, filename = fn, caption = caption};
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

    public class GroupDocumentPayload
    {
        public string group_name { get; set; }
        public string document { get; set; }
        public string filename { get; set; }
        public string caption { get; set; }
    }

}
