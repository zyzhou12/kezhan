using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TiKuService.Model
{
    public class GroupUserStatusModel
    {
        public string ActionStatus { get; set; }
        public string ErrorInfo { get; set; }
        public string ErrorCode { get; set; }
        public List<QueryResultModel> QueryResult { get; set; }

    }

    public class QueryResultModel
    {
        public string To_Account { get; set; }
        public string State { get; set; }
    }

    public class GroupDestoryResponseModel
    {

        public string ActionStatus { get; set; }
        public string ErrorInfo { get; set; }
        public string ErrorCode { get; set; }
    }
}
