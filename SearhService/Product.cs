using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Common;
using System.Xml;
using System.IO;
using System.Xml.Serialization;

namespace SearhService
{
    public class Product : IProduct
    {
        public List<int> GetShopListByUserID(int userID)
        {
            //模拟从MemCache中读取索引
            SerializableDictionary<int, List<int>> dic = new SerializableDictionary<int, List<int>>();

            byte[] bytes = Encoding.UTF8.GetBytes(File.ReadAllText("F://1.txt", Encoding.UTF8));

            var memoryStream = new MemoryStream();

            memoryStream.Write(bytes, 0, bytes.Count());

            memoryStream.Seek(0, SeekOrigin.Begin);

            XmlSerializer xml = new XmlSerializer(dic.GetType());

            var obj = xml.Deserialize(memoryStream) as Dictionary<int, List<int>>;

            return obj[userID];
        }

        public void TestSearch() { }
    }
}
