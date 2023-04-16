using System;

namespace OkrsEntreprise.Framework.Exceptions
{
    public class OutOfExpectedRangeException : Exception
    {
        public OutOfExpectedRangeException():base("Value does not fall within expected range")
        {
        }

        public OutOfExpectedRangeException(string value, string rangeMin,string rangeMax) : 
            base("Value does not fall within expected range.{value} should in [{rangeMin},{rangeMax},]")
        {
        }

       
    }
}