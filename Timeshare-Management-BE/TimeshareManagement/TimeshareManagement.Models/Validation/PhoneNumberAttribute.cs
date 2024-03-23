using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeshareManagement.Models.Validation
{
    public class PhoneNumberAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value == null)
                return false;

            string phoneNumber = value.ToString();

            // Check if the phone number starts with "0"
            if (phoneNumber.StartsWith("0"))
                return true;

            return false;
        }

        public override string FormatErrorMessage(string name)
        {
            return "Phone number must begin with '0'";
        }
    }
}
