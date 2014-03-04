using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Timers;
using System.IO;
using HeartBeatService;

namespace HeartHost
{
    public class HeartBeatHost
    {
        static void Main()
        {
            ServiceHost host = new ServiceHost(typeof(Address));

            host.Open();

            Console.WriteLine("大家好，我是心跳，我的目的就是看下Search那些娃是不是都战死了！");

            Console.Read();

        }
    }
}