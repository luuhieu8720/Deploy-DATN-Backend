using DATN_Back_end.Exceptions;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DATN_Back_end.Handlings
{
    public class ValidateModelHandling : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                var errors = context.ModelState.Values.SelectMany(modelState => modelState.Errors).ToList();

                var errorMessage = errors.Select(x => x.ErrorMessage).FirstOrDefault();

                throw new BadRequestException(errorMessage);
            }
        }
    }
}
