using Panda.DynamicWebApi;
using Panda.DynamicWebApi.Attributes;
using System;

namespace AglieFrame.Interface
{

    /*
     动态WebApi说明： 必须.Net5以上
     1、IService和Startup须引入包Panda.DynamicWebApi
     2、IService中无需编码
     3、Servcie须被Startup引用
    */
    [DynamicWebApi]
    public interface ITestService:IDynamicWebApi
    {
    }
}
