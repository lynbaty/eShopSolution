using System;
using System.Collections.Generic;
using System.Text;

namespace eShopSolution.ViewModels.Common
{
    public class ApiResult<T>
    {
        public T Result { set; get; }

        public string Messenger { set; get; }

        public ApiResult(T result, string messenger)
        {
            Result = result;
            Messenger = messenger;
        }

        public ApiResult(T result)
        {
            Result = result;
            Messenger = "";
        }

        public ApiResult(string messenger)
        {
            Messenger = messenger;
        }
    }
}