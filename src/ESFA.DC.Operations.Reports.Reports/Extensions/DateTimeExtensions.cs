using System;
using System.Collections.Generic;
using System.Text;
using ESFA.DC.Operations.Reports.Reports.Constants;

namespace ESFA.DC.Operations.Reports.Reports.Extensions
{
    public static class DateTimeExtensions
    {
        public static string LongDateStringFormat(this DateTime source)
        {
            if (source == null)
            {
                return null;
            }

            return source.ToString(FormattingConstants.LongDateTimeStringFormat);
        }
    }
}
