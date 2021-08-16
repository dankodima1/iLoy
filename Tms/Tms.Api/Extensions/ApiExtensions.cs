using System;
using System.Linq;

using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Tms.Api.Extensions
{
    public static class ApiExtensions
    {
        public static string GetErrors(this ModelStateDictionary model)
        {
            return string.Join(Environment.NewLine, model.Values
                .Where(x => x.Errors.Any())
                .Select(y =>
                {
                    return string.Join(", ", y.Errors.Select(z => z.ErrorMessage));
                })
            );
        }

        public static string GetFullMessage(this Exception ex)
        {
            if (ex.InnerException == null)
                return ex.Message;
            else
                return $"{ex.Message}{Environment.NewLine}{Environment.NewLine}inner exception:{Environment.NewLine}{ex.InnerException.Message}";
        }
    }
}
