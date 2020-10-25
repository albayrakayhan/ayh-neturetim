using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aceka.infrastructure.Core
{
    public static class CustomMethods
    {
        public static int acekaToInt(this object obj)
        {
            int retVal = 0;
            if (obj == null) obj = 0;//değer null da gelebilir bizim bu metodlarımız hata döndürmemeli 
            int.TryParse(obj.ToString(), out retVal);
            return retVal;
        }

        public static int? acekaToIntWithNullable(this object obj)
        {
            int? retVal = null;
            int b2;
            bool success = int.TryParse(obj.ToString(), out b2);
            if (success)
                retVal = b2;
            return retVal;
        }

        public static long acekaToLong(this object obj)
        {
            long retVal = 0;
            if (obj == null) return retVal;
            long.TryParse(obj.ToString(), out retVal);
            return retVal;
        }

        public static long? acekaToLongWithNullable(this object obj)
        {
            long? retVal = null;
            long b2;
            bool success = long.TryParse(obj.ToString(), out b2);
            if (success)
                retVal = b2;
            return retVal;
        }

        public static double acekaToDouble(this object obj)
        {
            double retVal = 0.0;
            double.TryParse(obj.ToString(), out retVal);
            return retVal;
        }

        public static double? acekaToDoubleWithNullable(this object obj)
        {
            double? retVal = null;
            double b2;
            bool success = double.TryParse(obj.ToString(), out b2);
            if (success)
                retVal = b2;
            return retVal;
        }

        public static float acekaToFloat(this object obj)
        {
            float retVal = 0;
            if (obj == null)
                return retVal;

            float.TryParse(obj.ToString(), out retVal);
            return retVal;
        }

        public static float? acekaToFloatWithNullable(this object obj)
        {
            float? retVal = null;
            float b2;
            bool success = float.TryParse(obj.ToString(), out b2);
            if (success)
                retVal = b2;
            return retVal;
        }

        public static decimal acekaToDecimal(this object obj)
        {
            decimal retVal = 0m;
            decimal.TryParse(obj.ToString(), out retVal);
            return retVal;
        }

        public static decimal? acekaToDecimalWithNullable(this object obj)
        {
            decimal? retVal = null;
            decimal b2;
            bool success = decimal.TryParse(obj.ToString(), out b2);
            if (success)
                retVal = b2;
            return retVal;
        }

        public static short acekaToShort(this object obj)
        {
            Int16 retVal = 0;
            Int16.TryParse(obj.ToString(), out retVal);
            return retVal;
        }

        public static short? acekaToShortWithNullable(this object obj)
        {
            short? retVal = null;
            short b2;
            bool success = short.TryParse(obj.ToString(), out b2);
            if (success)
                retVal = b2;
            return retVal;
        }

        public static byte acekaToByte(this object obj)
        {
            byte retVal = 0;
            byte.TryParse(obj.ToString(), out retVal);
            return retVal;
        }

        public static byte? acekaToByteWithNullable(this object obj)
        {
            byte? retVal = null;
            byte b2;
            bool success = byte.TryParse(obj.ToString(), out b2);
            if (success)
                retVal = b2;
            return retVal;
        }


        public static bool acekaToBool(this object obj)
        {
            bool retVal = false;
            Boolean.TryParse(obj.ToString(), out retVal);
            return retVal;
        }

        public static bool? acekaToBoolWithNullable(this object obj)
        {
            bool? retVal = null;
            bool d2;
            bool success = bool.TryParse(obj.ToString(), out d2);
            if (success)
                retVal = d2;
            return retVal;
        }

        public static DateTime acekaToDateTime(this object obj)
        {
            DateTime retVal = new DateTime();
            DateTime.TryParse(obj.ToString(), out retVal);
            return retVal;
        }

        public static DateTime? acekaToDateTimeWithNullable(this object obj)
        {
            DateTime? retVal = null;
            DateTime d2;
            bool success = DateTime.TryParse(obj.ToString(), out d2);
            if (success)
                retVal = d2;
            return retVal;
        }

        public static string acekaToString(this object obj)
        {
            string retVal = String.Empty;
            if (obj != null)
            {
                retVal = obj.ToString();
            }
            return retVal;
        }
    }
}
