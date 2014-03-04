using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Timers;
using System.Diagnostics;
using BaseClass;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {

            //ServiceHost host = new ServiceHost(typeof(ServiceList));

            //host.Open();

            //var client = new AddressClient(new InstanceContext(new Program()));

            //var iis = ConfigurationManager.AppSettings["iis"];

            //client.GetService(iis);

            //while (ServiceList.searchList.Count == 0)
            //    continue;

            //var factory = new ChannelFactory<IProduct>(new NetTcpBinding(SecurityMode.None), new EndpointAddress(ServiceList.Search));

            //var productIDList = factory.CreateChannel().GetProductIDByShopID(1);

            var productIDList = new List<int>();

            //这里就默认加载了一些数据
            for (int i = 300000; i < 300050; i++)
            {
                productIDList.Add(i);
            }

            var strSql = string.Join(",", productIDList);

            StringBuilder builder = new StringBuilder();

            builder.Append("select * from ");
            builder.Append("(select  ROW_NUMBER() over(order by s.ShopID) as NumberID, ");
            builder.Append(" s.ShopID, u.UserName, s.ShopName ");
            builder.Append("from Shop s left join [User] as u on u.UserID=s.UserID ");
            builder.Append("where  s.UserID=15) as array ");
            builder.Append("where NumberID>300000 and NumberID<300050");

            Stopwatch watch = new Stopwatch();

            watch.Start();

            SqlHelper.Query("select s.ShopID,u.UserName,s.ShopName  from [User] as u ,Shop as s where s.ShopID in(" + strSql + ")");
            //SqlHelper.Query(builder.ToString());

            watch.Stop();

            Console.WriteLine("花费时间:" + watch.ElapsedMilliseconds);

            Console.WriteLine("product里面的ID有：" + productIDList.Count);

            Console.Read();
        }
    }
}
