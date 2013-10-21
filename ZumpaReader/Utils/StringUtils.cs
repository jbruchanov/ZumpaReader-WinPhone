using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace ZumpaReader.Utils
{
    public class StringUtils
    {
        private static int TIME_OFFSET = 10000;

        private static string THREAD_ID_PREFIX = "&t=";
        private static string AMPERSAND = "&";

        /// <summary>
        /// Extract Thread Id from thread url
        /// </summary>
        /// <param name="threadUrl"></param>
        /// <returns></returns>
        public static string ExtractThreadId(string threadUrl)
        {
            int start = threadUrl.IndexOf("&t=") + THREAD_ID_PREFIX.Length;
            int end = threadUrl.IndexOf(AMPERSAND, start);            
            return end > -1 ? threadUrl.Substring(start, end - start) : threadUrl.Substring(start);
        }

        /// <summary>
        /// Convert java time value into readable string
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ConvertDateTime(long value)
        {
            long t = (value + (DateTimeOffset.Now.Offset.Hours * 3600000)) * TIME_OFFSET;
            return (value > 86400000)
                    ? new DateTime(t).AddYears(1969).AddDays(-1).ToString("d.MM.yyyy HH:mm.ss", CultureInfo.InvariantCulture)
                    : new DateTime(t).ToString("HH:mm");
        }

        /// <summary>
        /// Convert data size into readable string
        /// </summary>
        /// <param name="len"></param>
        /// <returns></returns>
        public static string ConvertToReadableSize(long len)
        {
            string[] sizes = { "B", "KiB", "MiB" };
            int order = 0;
            while (len >= 1024 && order + 1 < sizes.Length)
            {
                order++;
                len = len / 1024;
            }
            return String.Format("{0:0.##} {1}", len, sizes[order]);
        }
    }
}
