using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace Diary.Models
{
    public class Reply
    {
        public string DateStamp { get; set; }
        public string Text { get; set; }
        public string Name { get; set; }

        public static List<Reply> GetReplies(string sPath)
        {
            var l = new List<Reply>();
            var dir = new DirectoryInfo(sPath);
            foreach (var fileInfo in dir.GetFiles("comment_*.txt"))
            {
                var fullPath = fileInfo.FullName;
                var comment = TextFile.ReadFileWithoutEncoding(fullPath);
                var dateStamp = fileInfo.ToString().Replace("comment_", "").Replace(".txt", "");
                var r = new Reply
                              {
                                  DateStamp =  dateStamp,
                                  Text = comment
                              };
                l.Add(r);
            }
            return l;
        }

        public void Create(string comment, string from, string day, string category)
        {
            var text = comment.Replace(Environment.NewLine, "<br />");
            text += "<br />Från: " + from;

            var time = DateTime.Now.ToShortTimeString();
            var date = DateTime.Now.ToShortDateString();
            var fileName = "comment_"+date + " " + time.Replace(":",".") + ".txt";
            var sPath = AppDomain.CurrentDomain.BaseDirectory + "Upload\\" + category + "\\" + day;
            var fileUrl = sPath + "\\" + fileName;
            TextFile.CreateFile(fileUrl);
            TextFile.AppendToFile(fileUrl, text);
        }

    }
}