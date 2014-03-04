using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Web.Script.Serialization;
using System.IO;
using System.Xml.Serialization;
using System.Xml;
using Common;

namespace LoadDBData
{
    class Program
    {
        static void Main(string[] args)
        {
            //模拟从数据库加载索引到内存中,形成内存中的数据库
            //这里的 "Dictionary" 用来表达“一个用户注册过多少店铺“，即UserID与ShopID的一对多关系
            SerializableDictionary<int, List<int>> dic = new SerializableDictionary<int, List<int>>();
            List<int> shopIDList = new List<int>();
            for (int shopID = 300000; shopID < 300050; shopID++)
                shopIDList.Add(shopID);
            int UserID = 15;
            //假设这里已经维护好了UserID与ShopID的关系
            dic.Add(UserID, shopIDList);
            XmlSerializer xml = new XmlSerializer(dic.GetType());
            var memoryStream = new MemoryStream();
            xml.Serialize(memoryStream, dic);
            memoryStream.Seek(0, SeekOrigin.Begin);
            //将Dictionary持久化，相当于模拟保存在Mencache里面
            File.AppendAllText("F://1.txt", Encoding.UTF8.GetString(memoryStream.ToArray()));
            Console.WriteLine("数据加载成功！");
            Console.Read();
        }
    }
}
