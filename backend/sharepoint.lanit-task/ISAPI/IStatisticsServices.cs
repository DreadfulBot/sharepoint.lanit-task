using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Web;
using sharepoint.lanit_task.App.Models;

// http://social.technet.microsoft.com/wiki/contents/articles/24194.sharepoint-2013-create-a-custom-wcf-rest-service-hosted-in-sharepoint-and-deployed-in-a-wsp.aspx
namespace sharepoint.lanit_task.Services
{
    [ServiceContract]
    interface IStatisticsServices
    {
        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, UriTemplate = "HelloWorld")]
        string HelloWorld();

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, UriTemplate = "GetUserStatistics/{userId}")]
        List<StatisticsItem> GetUserStatistics(string userId);

        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, UriTemplate = "GetCurrentUserStatistics")]
        List<StatisticsItem> GetCurrentUserStatistics();
    }
}
