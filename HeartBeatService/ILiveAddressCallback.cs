using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;

namespace HeartBeatService
{
    /// <summary>
    /// 等客户端实现后，让客户端约束一下，只能是这个LiveAddress方法
    /// </summary>
    public interface ILiveAddressCallback
    {
        [OperationContract(IsOneWay = true)]
        void LiveAddress(string address);
    }
}
