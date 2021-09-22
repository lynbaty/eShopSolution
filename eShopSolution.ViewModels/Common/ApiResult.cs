using System;
using System.Collections.Generic;
using System.Text;

namespace eShopSolution.ViewModels.Common
{
    public class ApiResult<T>
    {
        public T Obj { set; get; }

        public string Messenger { set; get; }

        public ApiResult(T obj, string messenger)
        {
            Obj = obj;
            Messenger = messenger;
        }
    }
}