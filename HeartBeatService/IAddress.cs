using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace HeartBeatService
{
    //CallbackContract：这个就是Client实现此接口方面服务器端通知客户端
    [ServiceContract(CallbackContract = typeof(ILiveAddressCallback))]
    public interface IAddress
    {
        /// <summary>
        /// 此方法用于Search启动后，将Search地址插入到此处
        /// </summary>
        /// <param name="address"></param>
        [OperationContract(IsOneWay = true)]
        void AddSearch(string address);

        /// <summary>
        /// 此方法用于IIS端获取search地址
        /// </summary>
        /// <param name="address"></param>
        [OperationContract(IsOneWay = true)]
        void GetService(string address);
    }
}
