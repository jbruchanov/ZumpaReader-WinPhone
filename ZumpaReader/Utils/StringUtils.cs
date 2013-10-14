using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZumpaReader.Utils
{
    public class StringUtils
    {
        private static string THREAD_ID_PREFIX = "&t=";
        private static string AMPERSAND = "&";

        public static string ExtractThreadId(string threadUrl)
        {
            int start = threadUrl.IndexOf("&t=") + THREAD_ID_PREFIX.Length;
            int end = threadUrl.IndexOf(AMPERSAND, start);            
            return end > -1 ? threadUrl.Substring(start, end - start) : threadUrl.Substring(start);
        }
    }
}
