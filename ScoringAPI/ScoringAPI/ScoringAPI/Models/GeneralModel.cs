using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScoringAPI.Models
{
    public class GeneralModel
    {
        public class ParamsModel
        {
            public static string DBCon { get; set; }
        }

        public class ResponseModel<T>
        {
            public int StatusCode { get; set; }
            public string Message { get; set; }
            public string Error { get; set; }
            public T Data { get; set; }
        }
    }
}
