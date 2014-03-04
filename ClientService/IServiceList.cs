using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;

namespace ClientService
{
    [ServiceContract]
    public interface IServiceList
    {
        [OperationContract]
        void AddSearchList(List<string> search);
    }
}
