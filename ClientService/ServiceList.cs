using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Configuration;
using System.Timers;
using System.Threading;

namespace ClientService
{
    public class ServiceList : IServiceList
    {
        public static List<string> searchList = new List<string>();

        static object obj = new object();

        public static string Search
        {
            get
            {

                //如果心跳没及时返回地址，客户端就在等候
                while (searchList.Count == 0)
                {
                    Thread.Sleep(1000);
                }
                return searchList[new Random().Next(0, searchList.Count)];

            }
            set
            {

            }
        }

        public void AddSearchList(List<string> search)
        {
            lock (obj)
            {
                searchList = search;

                Console.WriteLine("************************************");
                Console.WriteLine("当前存活的Search为:");

                foreach (var single in searchList)
                {
                    Console.WriteLine(single);
                }
            }
        }
    }
}
