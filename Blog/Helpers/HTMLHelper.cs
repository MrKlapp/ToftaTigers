using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Diary.Models
{
    public class HTMLHelper
    {
        public static string HTMLDecode(string html)
        {
            return HttpUtility.HtmlDecode(html);
        }
    }
}