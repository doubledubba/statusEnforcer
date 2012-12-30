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
<<<<<<< HEAD
            //getting all the information needed
            StreamReader r = new StreamReader("config.txt");        //read id from the file
            String[] info = r.ReadToEnd().Split('~');

            String id = info[0];
            String clientPws = info[1];
            String serverIp = info[2];
            String serverPws = info[3];
            r.Close();
=======

            
>>>>>>> a81a5196f4ebec2f052b3fd735d374847197875b

            
            //===========TIMER=========
            while(true)
<<<<<<< HEAD
            {                
=======
            {
                //getting all the information needed
                StreamReader r = new StreamReader("config.txt");        //read id from the file
                String[] info = r.ReadToEnd().Split('~');

                String id = info[0];
                String clientPws = info[1];
                String serverIp = info[2];
                String serverPws = info[3];
                r.Close();

                //code here
>>>>>>> a81a5196f4ebec2f052b3fd735d374847197875b
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

        public static void check()
        {
            StreamReader r = new StreamReader("config.txt");
            string line = r.ReadToEnd();
            r.Close();

            String[] stringarray = line.Split('~');
            String clientPws = stringarray[1];
            String serverIp = stringarray[2];
            String serverPws = stringarray[3];

            if (Convert.ToInt32(stringarray[0]) == 0)
            {
                Console.WriteLine("What is your computer name?");
                string name = Console.ReadLine();

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
                StreamWriter write = new StreamWriter("config.txt");
                write.Write(String.Format("{0}~{1}~{2}~{3}", responseFromServer, clientPws, serverIp, serverPws)); 
 
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
                    System.Diagnostics.Process.Start("shutdown", "/s /t 30");
                    break;
                case "hibernate":
                    System.Diagnostics.Process.Start("shutdown.exe", "/h /t 30");
                    break;
                case "restart":
                    System.Diagnostics.Process.Start("shutdown.exe", "/r /t 30");
                    break;
                case "logoff":
                    System.Diagnostics.Process.Start("shutdown.exe", "/l /t 30");
                    break;
                default: //ok
                    break;

            }
        }
        }
    }

