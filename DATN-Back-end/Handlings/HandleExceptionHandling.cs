﻿using DATN_Back_end.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DATN_Back_end.Handlings
{
    public class HandleExceptionHandling : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            if (context.Exception is BaseException exception)
            {
                context.Result = new ObjectResult(exception.Message)
                {
                    StatusCode = (int)exception.StatusCode
                };
            }
        }
    }
}
