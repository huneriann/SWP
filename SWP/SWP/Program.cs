namespace SWP
{
    using System;
    using System.Diagnostics;
    using System.Threading;
    using System.Collections.Generic;

    class Program
    {
        static void Main(string[] args)
        {
            Proc a = new Proc();
            a.process();
            Console.ReadLine();
        }
    }
    class Proc
    {
        Process netsh = new Process();

        public const string app_name = "netsh.exe";
        public const string command = "wlan show profile";

        public readonly string strAUP;
        public readonly string strPass;

        List<string> Names = new List<string>();

        public void process()
        {
            getNames();
            getPass();
        }

        private void getNames()
        {
            string s = netsh.StandardOutput.ReadToEnd();
            var s1 = s.Split('\n');
            for (int i = 0; i < s1.Length; i++)
            {
                if (s1[i].Contains(strAUP))
                {
                    Names.Add(s1[i].Split(':')[1].Trim());
                }
            }
        }
        private void getPass()
        {
            Console.WriteLine(" {0,-20} | {1,5:N1}", "SSID", "Password");
            Console.WriteLine();
            foreach (var item in Names)
            {
                netsh.StartInfo.Arguments = command + " " + item + " key=\"clear\"";
                netsh.Start();
                string s = netsh.StandardOutput.ReadToEnd();
                var s1 = s.Split('\n');
                for(int i = 0; i < s1.Length; i++)
                {
                    if (s1[i].Contains(strPass))
                    {
                        Console.WriteLine(" {0,-20} | {1,5}", item , s1[i].Split(':')[1].Trim());
                    }
                }
            }
        }

        public Proc()
        {
            strAUP = Thread.CurrentThread.CurrentCulture.Name;

            if (strAUP == "ru-RU")
            {
                strAUP = "Все профили пользователей";
                strPass = "Содержимое ключа";
            }
            if(strAUP == "en-EN" || strAUP == "en-US")
            {
                strAUP = "All User Profile";
                strPass = "Key Content";
            }

            netsh.StartInfo.FileName = app_name;
            netsh.StartInfo.Arguments = command;
            netsh.StartInfo.UseShellExecute = false;
            netsh.StartInfo.RedirectStandardOutput = true;

            netsh.Start();

        }
    }
}
