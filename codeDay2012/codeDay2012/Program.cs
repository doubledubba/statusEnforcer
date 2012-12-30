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
           //IPADDRESS = 10.100.58.69
          //  StreamReader SR = new StreamReader("config.txt");
            //string IPAddress = SR.ReadToEnd(); //get IP ADDRESS. may need to update once config file gets more complex
            string IPAddress = "http://httpbin.org/post";
            bool status;  //true/false value; true = on false = off
            int clientID = 1; //unique for each CLIENT

           // HttpWebRequest request;

            Thread Begin; //initializing thread to pick send request every 30 secs
            int secs = 30;


            WebRequest request = WebRequest.Create("http://10.100.58.69/check_in");
            request.Method = "POST";
            string postData = "clientId=1";
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
            //Console.WriteLine(((HttpWebResponse)response).StatusDescription);



            //===========TIMER=========
            while(true)
            {//code here
       
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
        //public SendMessage(int ID)
        //{
        //    //send clientID to server
        //    WebRequest request = WebRequest.Create("http://www.google.com"); //placeholder
        //    request.Method = "POST";
        //}        
        }
    }

            //while(true)
            //{//code here
            //    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(IPAddress); //creating
            //    request.Method = "POST";
            //    HttpWebResponse HttpWResp = (HttpWebResponse)request.GetResponse();
            //    Stream receiveStream = HttpWResp.GetResponseStream();
            //    Encoding encode = System.Text.Encoding.GetEncoding("utf-8");
            //    StreamReader readStream = new StreamReader(receiveStream, encode);
            //    string res = readStream.ToString();
            //    readStream.Close();
            //    //WebResponse response = request.GetResponse(); //will need to close somewhere later
            //    //Stream dataStream = response.GetResponseStream();
            //    //StreamReader reader = new StreamReader(dataStream);
            //    //// Read the content.
            //    //string responseFromServer = reader.ReadToEnd();
            //    //ServerCommand(responseFromServer);
            //    //response.Close();


            //    Thread.Sleep(secs*1000);

            //}