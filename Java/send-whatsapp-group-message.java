import java.io.BufferedReader;
import java.io.OutputStream;
import java.io.InputStreamReader;
import java.net.HttpURLConnection;
import java.net.URL;

public class WhatsappSender {
  // TODO: Replace the following with your gateway instance ID, Forever Green Client ID and Secret below.
  private static final String INSTANCE_ID = "YOUR_INSTANCE_ID_HERE";
  private static final String CLIENT_ID = "YOUR_CLIENT_ID_HERE";
  private static final String CLIENT_SECRET = "YOUR_CLIENT_SECRET_HERE";
  private static final String WA_GATEWAY_URL = "https://api.whatsmate.net/v3/whatsapp/group/text/message/" + INSTANCE_ID;

  /**
   * Entry Point
   */
  public static void main(String[] args) throws Exception {
    String groupAdmin = "12025550108";  // TODO: Specify the WhatsApp number of the group creator, including the country code
    String groupName = "Happy Club";    // TODO: Specify the name of the group
    String message = "Guys, let's party tonight!";  // TODO: Specify the content of your message

    WhatsappSender.sendGroupMessage(groupAdmin, groupName, message);
  }

  /**
   * Sends out a WhatsApp message to a group
   */
  public static void sendGroupMessage(String groupAdmin, String groupName, String message) throws Exception {
    // TODO: Should have used a 3rd party library to make a JSON string from an object
    String jsonPayload = new StringBuilder()
      .append("{")
      .append("\"group_admin\":\"")
      .append(groupAdmin)
      .append("\",")
      .append("\"group_name\":\"")
      .append(groupName)
      .append("\",")
      .append("\"message\":\"")
      .append(message)
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