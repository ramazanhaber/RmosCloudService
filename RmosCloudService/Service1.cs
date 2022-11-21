using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace RmosCloudService
{
    public partial class Service1 : ServiceBase
    {
        Timer timer = new Timer(); // name space(using System.Timers;)  

        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            WriteToFile("Service is started at " + DateTime.Now);

            timer.Elapsed += new ElapsedEventHandler(OnElapsedTime);

            timer.Interval = 5000; //number in milisecinds  

            timer.Enabled = true;
        }

        protected override void OnStop()
        {
            WriteToFile("Service is stopped at " + DateTime.Now);
        }
        private void OnElapsedTime(object source, ElapsedEventArgs e)
        {
            try
            {
                string filepath = AppDomain.CurrentDomain.BaseDirectory + "RmosAdres.exe";
                WriteToFile(filepath);

                



                if (File.Exists(filepath) && System.Diagnostics.Process.GetProcessesByName("RmosAdres").Length == 0)
                {
                    Process p = new Process();
                    ProcessStartInfo psi = new ProcessStartInfo(filepath);
                    //psi.WindowStyle = ProcessWindowStyle.Hidden;
                    psi.Verb = "runas";
                    p.StartInfo = psi;
                    p.Start();
                }
                else
                {

                }
            }catch(Exception ex)
            {

            }

            this.timer.Stop(); // bunu ben ekledim eğer 5 sn de işi bitmezse girmesin diye
            WriteToFile("Service is recall at " + DateTime.Now);
           this.timer.Start(); // bunu ben ekledim eğer 5 sn de işi bitmezse girmesin diye

        }

        public void WriteToFile(string Message)
        {

            string path = AppDomain.CurrentDomain.BaseDirectory + "\\Logs";

            if (!Directory.Exists(path))
            {

                Directory.CreateDirectory(path);

            }
            string filepath = AppDomain.CurrentDomain.BaseDirectory + "\\Logs\\ServiceLog_" + DateTime.Now.Date.ToShortDateString().Replace('/', '_') + ".txt";
            if (!File.Exists(filepath))
            {

                // Create a file to write to.   

                using (StreamWriter sw = File.CreateText(filepath))
                {
                    sw.WriteLine(Message);
                }
            }
            else
            {
                using (StreamWriter sw = File.AppendText(filepath))
                {
                    sw.WriteLine(Message);
                }
            }

        }
    }
}
