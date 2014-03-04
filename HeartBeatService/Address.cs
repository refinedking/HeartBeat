using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Timers;
using System.IO;
using System.Collections.Concurrent;
using SearhService;
using ClientService;

namespace HeartBeatService
{
    //InstanceContextMode：主要是管理上下文的实例，此处是single，也就是单体
    //ConcurrencyMode：    主要是用来控制实例中的线程数，此处是Multiple，也就是多线程
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class Address : IAddress
    {
        static List<string> search = new List<string>();

        static object obj = new object();

        /// <summary>
        /// 此静态构造函数用来检测存活的Search个数
        /// </summary>
        static Address()
        {
            Timer timer = new Timer();
            timer.Interval = 6000;
            timer.Elapsed += (sender, e) =>
            {

                Console.WriteLine("\n***************************************************************************");
                Console.WriteLine("当前存活的Search为：");

                lock (obj)
                {
                    //遍历当前存活的Search
                    foreach (var single in search)
                    {
                        ChannelFactory<IProduct> factory = null;

                        try
                        {
                            //当Search存在的话，心跳服务就要定时检测Search是否死掉，也就是定时的连接Search来检测。
                            factory = new ChannelFactory<IProduct>(new NetTcpBinding(SecurityMode.None), new EndpointAddress(single));
                            factory.CreateChannel().TestSearch();
                            factory.Close();

                            Console.WriteLine(single);

                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);

                            //如果抛出异常，则说明此search已经挂掉
                            search.Remove(single);
                            factory.Abort();
                            Console.WriteLine("\n当前时间：" + DateTime.Now + " ,存活的Search有：" + search.Count() + "个");
                        }
                    }
                }

                //最后统计下存活的search有多少个
                Console.WriteLine("\n当前时间：" + DateTime.Now + " ,存活的Search有：" + search.Count() + "个");
            };
            timer.Start();
        }

        public void AddSearch(string address)
        {

            lock (obj)
            {
                //是否包含相同的Search地址
                if (!search.Contains(address))
                {
                    search.Add(address);

                    //search添加成功后就要告诉来源处，此search已经被成功载入。
                    var client = OperationContext.Current.GetCallbackChannel<ILiveAddressCallback>();
                    client.LiveAddress(address);
                }
            }
        }

        public void GetService(string address)
        {
            Timer timer = new Timer();
            timer.Interval = 1000;
            timer.Elapsed += (obj, sender) =>
            {
                try
                {
                    //这个是定时的检测IIS是否挂掉
                    var factory = new ChannelFactory<IServiceList>(new NetTcpBinding(SecurityMode.None),
                                                                   new EndpointAddress(address));

                    factory.CreateChannel().AddSearchList(search);

                    factory.Close();

                    timer.Interval = 10000;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            };
            timer.Start();
        }
    }
}
