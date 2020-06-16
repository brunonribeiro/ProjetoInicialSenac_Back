using System;
using System.ComponentModel.DataAnnotations;

namespace EmpresaApp.Domain.Attributes
{
    public class DateAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            var dateString = value as string;
            if (string.IsNullOrWhiteSpace(dateString))
            {
                return true;
            }
            return DateTime.TryParse(dateString, out var result);
        }
    }
}
