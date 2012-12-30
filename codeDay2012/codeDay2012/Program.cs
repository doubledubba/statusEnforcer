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
           
            //StreamReader SR = new StreamReader("config.txt");
            //string IPAddress = SR.ReadToEnd(); //get IP ADDRESS. may need to update once config file gets more complex

            //bool status;  //true/false value; true = on false = off
            //int clientID = 1; //unique for each CLIENT


            //WebRequest request;

            //Thread Begin; //initializing thread to pick send request every 30 secs
            //int secs = 30;

           
            //===========TIMER=========
            //while(true)
            //{//code here
            //    request = WebRequest.Create(IPAddress); //creating
            //    WebResponse response = request.GetResponse(); //will need to close somewhere later
            //    Stream dataStream = response.GetResponseStream();
            //    StreamReader reader = new StreamReader(dataStream);
            //    // Read the content.
            //    string responseFromServer = reader.ReadToEnd();
            //    response.Close();

            //    //Console.WriteLine("HI");
            //    Thread.Sleep(secs*1000);

            //}
        }
        //need method to decide what to do with input
        public static void ServerCommand(string command)
        {
            switch (command)
            {
                case "shutdown":
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
                default: //OK
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