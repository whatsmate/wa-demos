import java.io.BufferedReader;
import java.io.OutputStream;
import java.io.InputStreamReader;
import java.net.HttpURLConnection;
import java.net.URL;

public class WaUrlSender {
  // TODO: Replace the following with your instance ID, Forever Green Client ID and Secret:
  private static final String INSTANCE_ID = "YOUR_INSTANCE_ID_HERE";
  private static final String CLIENT_ID = "YOUR_CLIENT_ID_HERE";
  private static final String CLIENT_SECRET = "YOUR_CLIENT_SECRET_HERE";
  private static final String WA_GATEWAY_URL = "http://api.whatsmate.net/v3/whatsapp/single/url/message/" + INSTANCE_ID;

  /**
   * Entry Point
   */
  public static void main(String[] args) throws Exception {
    String number = "12025550108";  //  TODO: Specify the recipient's number here. NOT the gateway number
    String url = "https://www.pinterest.com/pin/861032022481631898";  // TODO

    WaUrlSender.sendUrl(number, url);
  }

  /**
   * Sends out a URL via WhatsMate WA Gateway.
   */
  public static void sendUrl(String number, String url) throws Exception {
    // TODO: Should have used a 3rd party library to make a JSON string from an object
    String jsonPayload = new StringBuilder()
      .append("{")
      .append("\"number\":\"")
      .append(number)
      .append("\",")
      .append("\"url\":\"")
      .append(url)
      .append("\"")
      .append("}")
      .toString();

    URL url = new URL(WA_GATEWAY_URL);
    HttpURLConnection conn = (HttpURLConnection) url.openConnection();
    conn.setDoOutput(true);
    conn.setRequestMethod("POST");
    conn.setRequestProperty("X-WM-CLIENT-ID", CLIENT_ID);
    conn.setRequestProperty("X-WM-CLIENT-SECRET", CLIENT_SECRET);
    conn.setRequestProperty("Content-Type", "application/json");

    OutputStream os = conn.getOutputStream();
    os.write(jsonPayload.getBytes());
    os.flush();
    os.close();

    int statusCode = conn.getResponseCode();
    System.out.println("Response from WA Gateway: \n");
    System.out.println("Status Code: " + statusCode);
    BufferedReader br = new BufferedReader(new InputStreamReader(
        (statusCode == 200) ? conn.getInputStream() : conn.getErrorStream()
      ));
    String output;
    while ((output = br.readLine()) != null) {
        System.out.println(output);
    }
    conn.disconnect();
  }

}
