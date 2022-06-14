using System;
using System.Collections;
using System.IO;
using System.Net;
using System.Web.Script.Serialization; // requires the reference 'System.Web.Extensions'
using System.Net.Sockets;
using System.Threading;
using System.Text;

/**
 * This program is derived from the works created by David Jeske.
 * Original source code: https://www.codeproject.com/articles/137979/simple-http-server-in-c
 *
 * Demo program by WhatsMate.net.
 * 
 * You will need to sign up for the WhatsApp Gateway for Enterprise plan in order for this program to work for you.
 *  https://www.whatsmate.net/whatsapp-gateway-enterprise.html
 *
 * This is a simplistic application that demonstrates how you can receive WhatsApp messages
 * from your dedicated Enterprise WhatsApp Gateway and reply to them accordingly.
 * 
 * The following diagram illustrates how messages flow between your application and your customer.
 *     .-----------------------------------------------------------------------------------------.
 *     | This application      |      The Enterprise WhatsApp GW      |      Customer's WhatsApp |
 *     '-----------------------------------------------------------------------------------------'
 *  (1)                                                              <---     "Hello"
 *  (2)                       <---     "Hello"
 *  (3)   "Choose a product"  ---> 
 *  (4)                                "Choose a product"            ---> 
 *  (5)                                                              <---     "1"
 *  (6)                       <---     "1"
 *  (7)   "Price: 90.0 "      ---> 
 *  (8)                                "Price: 90.0 "                ---> 
 */
namespace Bend.Util
{

    public class HttpProcessor
    {
        public TcpClient socket;
        public HttpServer srv;

        private Stream inputStream;
        public StreamWriter outputStream;

        public String http_method;
        public String http_url;
        public String http_protocol_versionstring;
        public Hashtable httpHeaders = new Hashtable();


        private static int MAX_POST_SIZE = 10 * 1024 * 1024; // 10MB

        public HttpProcessor(TcpClient s, HttpServer srv)
        {
            this.socket = s;
            this.srv = srv;
        }


        private string streamReadLine(Stream inputStream)
        {
            int next_char;
            string data = "";
            while (true)
            {
                next_char = inputStream.ReadByte();
                if (next_char == '\n') { break; }
                if (next_char == '\r') { continue; }
                if (next_char == -1) { Thread.Sleep(1); continue; };
                data += Convert.ToChar(next_char);
            }
            return data;
        }

        public void process()
        {
            // we can't use a StreamReader for input, because it buffers up extra data on us inside it's
            // "processed" view of the world, and we want the data raw after the headers
            inputStream = new BufferedStream(socket.GetStream());

            // we probably shouldn't be using a streamwriter for all output from handlers either
            outputStream = new StreamWriter(new BufferedStream(socket.GetStream()));
            try
            {
                parseRequest();
                readHeaders();
                if (http_method.Equals("GET"))
                {
                    handleGETRequest();
                }
                else if (http_method.Equals("POST"))
                {
                    handlePOSTRequest();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.ToString());
                writeFailure();
            }
            outputStream.Flush();
            // bs.Flush(); // flush any remaining output
            inputStream = null; outputStream = null; // bs = null;            
            socket.Close();
        }

        public void parseRequest()
        {
            String request = streamReadLine(inputStream);
            string[] tokens = request.Split(' ');
            if (tokens.Length != 3)
            {
                throw new Exception("invalid http request line");
            }
            http_method = tokens[0].ToUpper();
            http_url = tokens[1];
            http_protocol_versionstring = tokens[2];

            // Console.WriteLine("starting: " + request);
        }

        public void readHeaders()
        {
            // Console.WriteLine("readHeaders()");
            String line;
            while ((line = streamReadLine(inputStream)) != null)
            {
                if (line.Equals(""))
                {
                    // Console.WriteLine("got headers");
                    return;
                }

                int separator = line.IndexOf(':');
                if (separator == -1)
                {
                    throw new Exception("invalid http header line: " + line);
                }
                String name = line.Substring(0, separator);
                int pos = separator + 1;
                while ((pos < line.Length) && (line[pos] == ' '))
                {
                    pos++; // strip any spaces
                }

                string value = line.Substring(pos, line.Length - pos);
                // Console.WriteLine("header: {0}:{1}",name,value);
                httpHeaders[name] = value;
            }
        }

        public void handleGETRequest()
        {
            srv.handleGETRequest(this);
        }

        private const int BUF_SIZE = 4096;
        public void handlePOSTRequest()
        {
            // this post data processing just reads everything into a memory stream.
            // this is fine for smallish things, but for large stuff we should really
            // hand an input stream to the request processor. However, the input stream 
            // we hand him needs to let him see the "end of the stream" at this content 
            // length, because otherwise he won't know when he's seen it all! 

            // Console.WriteLine("POST data start");
            int content_len = 0;
            MemoryStream ms = new MemoryStream();
            if (this.httpHeaders.ContainsKey("Content-Length"))
            {
                content_len = Convert.ToInt32(this.httpHeaders["Content-Length"]);
                if (content_len > MAX_POST_SIZE)
                {
                    throw new Exception(
                        String.Format("POST Content-Length({0}) too big for this simple server",
                          content_len));
                }
                byte[] buf = new byte[BUF_SIZE];
                int to_read = content_len;
                while (to_read > 0)
                {
                    // Console.WriteLine("starting Read, to_read={0}",to_read);

                    int numread = this.inputStream.Read(buf, 0, Math.Min(BUF_SIZE, to_read));
                    // Console.WriteLine("read finished, numread={0}", numread);
                    if (numread == 0)
                    {
                        if (to_read == 0)
                        {
                            break;
                        }
                        else
                        {
                            throw new Exception("client disconnected during post");
                        }
                    }
                    to_read -= numread;
                    ms.Write(buf, 0, numread);
                }
                ms.Seek(0, SeekOrigin.Begin);
            }
            // Console.WriteLine("POST data end");
            srv.handlePOSTRequest(this, new StreamReader(ms));

        }

        public void writeSuccess(string content_type = "text/html")
        {
            outputStream.WriteLine("HTTP/1.0 200 OK");
            outputStream.WriteLine("Content-Type: " + content_type);
            outputStream.WriteLine("Connection: close");
            outputStream.WriteLine("");
        }

        public void writeFailure()
        {
            outputStream.WriteLine("HTTP/1.0 404 File not found");
            outputStream.WriteLine("Connection: close");
            outputStream.WriteLine("");
        }
    }

    public abstract class HttpServer
    {

        protected int port;
        TcpListener listener;
        bool is_active = true;

        public HttpServer(int port)
        {
            this.port = port;
        }

        public void listen()
        {
            // listener = new TcpListener(port);
            // The line below replaces the deprecated call above.
            listener = new TcpListener(IPAddress.Any, port);
            listener.Start();
            while (is_active)
            {
                TcpClient s = listener.AcceptTcpClient();
                HttpProcessor processor = new HttpProcessor(s, this);
                Thread thread = new Thread(new ThreadStart(processor.process));
                thread.Start();
                Thread.Sleep(1);
            }
        }

        public abstract void handleGETRequest(HttpProcessor p);
        public abstract void handlePOSTRequest(HttpProcessor p, StreamReader inputData);
    }

    public class WebhookHttpServer : HttpServer
    {
        protected WhatsAppMessageSender msgSender;

        public WebhookHttpServer(int port)
            : base(port)
        {
            msgSender = new WhatsAppMessageSender();
        }

        public override void handleGETRequest(HttpProcessor p)
        {
            Console.WriteLine("GET request: {0}", p.http_url);
            p.writeSuccess();
            p.outputStream.WriteLine("Current Time: " + DateTime.Now.ToString());
        }

        public override void handlePOSTRequest(HttpProcessor p, StreamReader inputData)
        {
            Console.WriteLine("POST request: {0}", p.http_url);
            string rawBody = inputData.ReadToEnd();

            // Consume the POST request only if it's a webhook notification
            if ("/webhook".Equals(p.http_url))
            {
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                dynamic jsonDict = serializer.DeserializeObject(rawBody);

                if (jsonDict.ContainsKey("number") && jsonDict.ContainsKey("message"))
                {
                    string usrNumber = jsonDict["number"];
                    string usrMessage = jsonDict["message"];

                    Console.WriteLine("Received a valid webhook notification!");
                    handleUserMessageRecevied(usrNumber, usrMessage);
                }
                else
                {
                    Console.WriteLine("Received an INVALID webhook notification: " + rawBody);
                }

            }

            // Return 200 OK no mater what.
            p.writeSuccess();
            p.outputStream.WriteLine("Done");
        }

        /**
         * TODO: Implement your webhook handler here!
         */
        private void handleUserMessageRecevied(string userNumber, string userMessage)
        {
            Console.WriteLine("Received a message from a customer");
            Console.WriteLine("Customer's number: " + userNumber);
            Console.WriteLine("Customer said: " + userMessage);

            string replyMessage = "Choose a product. Send \n1 for product X.\n2 for product Y and so on.";
            if ("1".Equals(userMessage))
            {
                replyMessage = "Price of X: $90";
            }
            else if ("2".Equals(userMessage))
            {
                replyMessage = "Price of Y: $100";
            }
            else if ("3".Equals(userMessage))
            {
                replyMessage = "Price of Z: $200";
            }

            msgSender.sendMessage(userNumber, replyMessage);
        }

    }


    public class WhatsAppMessageSender
    {
        // TODO: Specify your credentials here
        private static string INSTANCE_ID = "YOUR_INSTANCE_ID_HERE";
        private static string CLIENT_ID = "YOUR_CLIENT_ID_HERE";
        private static string CLIENT_SECRET = "YOUR_CLIENT_SECRET_HERE";

        private static string API_URL = "https://enterprise.whatsmate.net/v3/whatsapp/single/text/message/" + INSTANCE_ID;

        public bool sendMessage(string number, string message)
        {
            bool success = true;

            try
            {
                using (WebClient client = new WebClient())
                {
                    client.Headers[HttpRequestHeader.ContentType] = "application/json";
                    client.Headers["X-WM-CLIENT-ID"] = CLIENT_ID;
                    client.Headers["X-WM-CLIENT-SECRET"] = CLIENT_SECRET;

                    Payload payloadObj = new Payload() { number = number, message = message };
                    string postData = (new JavaScriptSerializer()).Serialize(payloadObj);

                    client.Encoding = Encoding.UTF8;
                    string response = client.UploadString(API_URL, postData);
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

        public class Payload
        {
            public string number { get; set; }
            public string message { get; set; }
        }

    }

    public class DemoMain
    {
        public static int Main(String[] args)
        {
            HttpServer httpServer;
            if (args.GetLength(0) > 0)
            {
                httpServer = new WebhookHttpServer(Convert.ToInt16(args[0]));
            }
            else
            {
                httpServer = new WebhookHttpServer(9999);
            }
            Thread thread = new Thread(new ThreadStart(httpServer.listen));
            thread.Start();
            return 0;
        }

    }

}

