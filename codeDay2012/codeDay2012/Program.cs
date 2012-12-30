using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using System.Timers;
using System.Threading;
using codeDay2012;
using Twilio;

namespace codeDay2012

{
    class Program
    {
        public static void Main(string[] args)
        {
            string password, username;
            Console.Write("Enter the password: ");
            password = Console.ReadLine();
            redo:
            Console.Write("Enter the username (must be 8 or more characters long): ");
            username = Console.ReadLine();  //must be 8+char
            if (username.Length < 8)
                goto redo;

            int secs = 5;
            
            check(username, password); //initial check of computer name and id # retreival from server
            //getting all the information needed
            String[] info = getConfig(username, password).Split('~');

            String id = info[0];
            String clientPws = info[1];
            String serverIp = info[2];
            String serverPws = info[3];
            
            //===========TIMER=========
            while(true)
            {                
                WebRequest request = WebRequest.Create("http://" + serverIp + "/check_in");
                request.Method = "POST";

                string postData = string.Format("clientId=" + id + "&key=" + clientPws);    //include the key into the request for authentication
                byte[] byteArray = Encoding.UTF8.GetBytes(postData);
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = byteArray.Length;

                Stream dataStream = request.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);


                // Close the Stream object.
                dataStream.Close();

                // Get the response.
                WebResponse response = request.GetResponse();
                // Display the status.
                Console.WriteLine(((HttpWebResponse)response).StatusDescription);
                // Get the stream containing content returned by the server.
                dataStream = response.GetResponseStream();
                // Open the stream using a StreamReader for easy access.
                StreamReader reader = new StreamReader(dataStream);
                // Read the content.
                String[] responseFromServer = reader.ReadToEnd().Split('~');
                String status = responseFromServer[0];
                if (status != "nope")
                {
                    String resServerPws = responseFromServer[1];

                    // Clean up the streams.
                    reader.Close();
                    dataStream.Close();
                    response.Close();

                    if (resServerPws == serverPws)
                    {
                        ServerCommand(status);
                    }
                }

                Thread.Sleep(secs * 1000);
            }
        }

        public static void check(string username, string password)
        {
            String[] stringarray;
            String clientPws;
            String serverIp;
            String serverPws;
            String id;

            if (File.Exists("config.txt"))
            {
                stringarray = getConfig(username, password).Split('~');
                id = stringarray[0];
                clientPws = stringarray[1];
                serverIp = stringarray[2];
                serverPws = stringarray[3];
            }
            else
            {
                id = "0";

                Console.Write("Enter client password: ");
                clientPws = Console.ReadLine();
                Console.Write("Enter server address: ");
                serverIp = Console.ReadLine();

                serverPws = "fu@qy71q@_2-g_e_!3v$s5ecf)ar=ur0s@t&amp;m5_&amp;fy_elw&amp;m#%";
            }

            if (Convert.ToInt32(id) == 0)
            {
                Console.WriteLine("What is your computer name?");
                string name = Console.ReadLine();
                Console.WriteLine("What's your 10 digit phone number? (area code first)");
                string phone = "+1" + Console.ReadLine();

                WebRequest request = WebRequest.Create("http://" + serverIp + "/check_in");
                request.Method = "POST";
                string postData = "name=" + name + "&key=" + clientPws;
                byte[] byteArray = Encoding.UTF8.GetBytes(postData);
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = byteArray.Length;
                Stream dataStream = request.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                // Close the Stream object.
                dataStream.Close();
                // Get the response.
                WebResponse response = request.GetResponse();
                // Display the status.
                Console.WriteLine(((HttpWebResponse)response).StatusDescription);
                // Get the stream containing content returned by the server.
                dataStream = response.GetResponseStream();
                // Open the stream using a StreamReader for easy access.
                StreamReader reader = new StreamReader(dataStream);
                // Read the content.
                string responseFromServer = reader.ReadToEnd();

                //Fix the config file
                encConfig(String.Format("{0}~{1}~{2}~{3}", responseFromServer, clientPws, serverIp, serverPws), username, password);

                // Clean up the streams.
                reader.Close();
                dataStream.Close();
                response.Close();
            }
        }

        //need method to decide what to do with input
        public static void ServerCommand(string command)
        {
            //setting up twilio connection
            const string TWIL_SID = "AC1130c4bcbb22281c15b499e38bcca193";
            const string AUTH_TOKEN = "6b3593bbc180afe713ffd43baae78c51";
            Account acc = new Account();
            acc.AuthToken = AUTH_TOKEN;
            acc.Sid = TWIL_SID;
            var twilio = new TwilioRestClient(TWIL_SID, AUTH_TOKEN);
            string t_msg = "";
            //reading config file
            StreamReader getPhone = new StreamReader("config.txt");
            string content = getPhone.ReadToEnd();
            string[] cArr = content.Split('~');
            string phone = cArr[4].ToString();
            SMSMessage msg;

            switch (command)
            {
                case "shutdown":
                    Console.WriteLine("Would you like to shut down?");
                    string n = Console.ReadLine();
                    t_msg = "Your computer will shut down";
                    if (n.ToUpper() == "Y")
                    {
                        msg = twilio.SendSmsMessage("+13603394722", phone, t_msg);
                        System.Diagnostics.Process.Start("shutdown.exe", "/s /t 0");
                    }
                    break;
                case "hibernate":
                    t_msg = "Your computer will now hibernate";
                    msg = twilio.SendSmsMessage("+13603394722", phone, t_msg);
                    System.Diagnostics.Process.Start("shutdown.exe", "/h");
                    break;
                case "restart":
                    t_msg = "Your computer will now reboot";
                    msg = twilio.SendSmsMessage("+13603394722", phone, t_msg);
                    System.Diagnostics.Process.Start("shutdown.exe", "/r");
                    break;
                case "logoff":
                    t_msg = "Your computer will now logoff";
                    msg = twilio.SendSmsMessage("+13603394722", phone, t_msg);
                    System.Diagnostics.Process.Start("shutdown.exe", "/l");
                    break;
                case "lock":
                    t_msg = "Your computer will now lock";
                    msg = twilio.SendSmsMessage("+13603394722", phone, t_msg);
                    System.Diagnostics.Process.Start(@"C:\WINDOWS\system32\rundll32.exe", "user32.dll,LockWorkStation");
                    break;
                default: //ok
                    break;
            }
        }

        public static string getConfig(string username, string password)
        {
            FileEncryptor decryptor = new FileEncryptor(username, password);
            return decryptor.decrypt("config.txt");
        }

        public static void encConfig(string str, string username, string password)
        {
            FileEncryptor encryptor = new FileEncryptor(username, password);
            encryptor.encrypt(str, "config.txt");
        }
   }
}

