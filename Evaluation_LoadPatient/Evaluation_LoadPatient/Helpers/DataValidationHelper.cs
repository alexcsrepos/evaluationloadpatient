using System;
using System.Data.SqlTypes;

namespace Evaluation_LoadPatient.Helpers
{
    internal static class DataValidationHelper
    {
        public static bool ValidateDate(DateTime dateTime)
        {
            return (bool)(dateTime >= (DateTime)SqlDateTime.MinValue && dateTime <= (DateTime)SqlDateTime.MaxValue);
        }
    }
}
