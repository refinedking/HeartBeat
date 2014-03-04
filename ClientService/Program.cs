using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Timers;
using System.Diagnostics;
using BaseClass;
using ClientService;
using ClientService.HeartBeatService;
using System.Configuration;
using SearhService;

namespace ClientService
{
    class Program : IAddressCallback
    {
        static void Main(string[] args)
        {

            ServiceHost host = new ServiceHost(typeof(ServiceList));

            host.Open();

            var client = new AddressClient(new InstanceContext(new Program()));

            //配置文件中获取iis的地址
            var iis = ConfigurationManager.AppSettings["iis"];

            //将iis的地址告诉心跳
            client.GetService(iis);

            //从集群中获取search地址来对Search服务进行调用
            var factory = new ChannelFactory<IProduct>(new NetTcpBinding(SecurityMode.None), new EndpointAddress(ServiceList.Search));

            //根据userid获取了shopID的集合
            //比如说这里的ShopIDList是通过索引交并集获取的分页的一些shopID
            var shopIDList = factory.CreateChannel().GetShopListByUserID(15);

            var strSql = string.Join(",", shopIDList);

            Stopwatch watch = new Stopwatch();

            watch.Start();
            SqlHelper.Query("select s.ShopID,u.UserName,s.ShopName  from [User] as u ,Shop as s where s.ShopID in(" + strSql + ")");
            watch.Stop();

            Console.WriteLine("通过wcf索引获取的ID >>>花费时间:" + watch.ElapsedMilliseconds);

            //普通的sql查询花费的时间
            StringBuilder builder = new StringBuilder();

            builder.Append("select * from ");
            builder.Append("(select  ROW_NUMBER() over(order by s.ShopID) as NumberID, ");
            builder.Append(" s.ShopID, u.UserName, s.ShopName ");
            builder.Append("from Shop s left join [User] as u on u.UserID=s.UserID ");
            builder.Append("where  s.UserID=15) as array ");
            builder.Append("where NumberID>300000 and NumberID<300050");

            watch.Start();
            SqlHelper.Query(builder.ToString());
            watch.Stop();

            Console.WriteLine("普通的sql分页 >>>花费时间:" + watch.ElapsedMilliseconds);

            Console.Read();
        }

        public void LiveAddress(string address)
        {

        }
    }
}
