using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NaucnaCentralaBackend.Models.ExternalTask
{
    public class ExternalTaskResponse
    {
        public ExternalTaskResponse(bool actionSucceeded, ExternalTaskItem data = null)
        {
            Success = actionSucceeded;
            ResponseData = data;
        }

        public ExternalTaskItem ResponseData { get; set; }
        public bool Success { get; set; }
    }
}
