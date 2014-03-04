using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Configuration;
using System.Timers;
using SearhService.HeartBeatService;

namespace SearhService
{
    public class SearchHost : IAddressCallback
    {
        static DateTime startTime;

        public static void Main()
        {
            ServiceHost host = new ServiceHost(typeof(Product));

            host.Open();

            AddSearch();

            Console.Read();

        }

        static void AddSearch()
        {
            startTime = DateTime.Now;

            Console.WriteLine("Search服务发送中.....\n\n*************************************************\n");

            try
            {
                var heartClient = new AddressClient(new InstanceContext(new SearchHost()));

                string search = ConfigurationManager.AppSettings["search"];

                heartClient.AddSearch(search);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Search服务发送失败：" + ex.Message);
            }
        }

        public void LiveAddress(string address)
        {
            Console.WriteLine("恭喜你," + address + "已被心跳成功接收！\n");
            Console.WriteLine("发送时间：" + startTime + "\n接收时间：" + DateTime.Now);
        }
    }
}
