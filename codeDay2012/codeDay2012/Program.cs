using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using System.Timers;
using System.Threading;

namespace codeDay2012

{
    class Program
    {
        public static void Main(string[] args)
        {    
            int secs = 5;
            
            check(); //initial check of computer name and id # retreival from server

            //===========TIMER=========
            while(true)
            {
                //code here
                WebRequest request = WebRequest.Create("http://10.100.58.69/check_in");
                request.Method = "POST";

                //prepare data to send
                StreamReader r = new StreamReader("config.txt");        //read id from the file
                String id = r.ReadToEnd().Split('~')[0];
                Console.WriteLine(id);
                r.Close();

                string postData = string.Format("clientId=" + id);
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
                // Display the content.
                Console.WriteLine(responseFromServer); //THIS IS THE COMMAND!!!!!!!!!!!
                // Clean up the streams.
                reader.Close();
                dataStream.Close();
                response.Close();
                ServerCommand(responseFromServer);
                //WebResponse response = request.GetResponse();
                Thread.Sleep(secs * 1000);
            }
        }

        public static void check()
        {
            StreamReader r = new StreamReader("config.txt");
            string line = r.ReadToEnd();
            r.Close();
            String[] stringarray = line.Split('~');
            if (Convert.ToInt32(stringarray[0]) == 0)
            {
                Console.WriteLine("What is your computer name?");
                string name = Console.ReadLine();

                WebRequest request = WebRequest.Create("http://10.100.58.69/check_in");
                request.Method = "POST";
                string postData = "name=" + name;
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
                StreamWriter write = new StreamWriter("config.txt");
                write.Write(String.Format(responseFromServer + "~{0}~{1}", stringarray[1], name));  
                // Display the content.
                // Clean up the streams.
                reader.Close();
                dataStream.Close();
                response.Close();
                write.Close();
            }
        
        
        }

        //need method to decide what to do with input
        public static void ServerCommand(string command)
        {
            switch (command)
            {
                case "shutdown":
                    Console.WriteLine("Would you like to shut down?");
                    string n = Console.ReadLine();
                    if (n.ToUpper() == "Y")
                        System.Diagnostics.Process.Start("shutdown", "/s /t 0");
                        break;
                case "hibernate":
                    System.Diagnostics.Process.Start("shutdown.exe", "/h /t 0");
                    break;
                case "restart":
                    System.Diagnostics.Process.Start("shutdown.exe", "/r /t 0");
                    break;
                case "logoff":
                    System.Diagnostics.Process.Start("shutdown.exe", "/l /t 0");
                    break;
                default: //ok
                    break;

            }
        }
        }
    }

