using System;
using System.IO;

/// <summary>
/// Summary description for TextFile
/// </summary>
public class TextFile
{
    public static void CreateFile(string fileurl)
    {
        try
        {
            var sw = File.CreateText(fileurl);
            sw.Close();
        }
        catch (Exception)
        {
        }
    }

    public static string ReadFile(string fileurl)
    {
        var str = "";
        try {
            StreamReader sr = new StreamReader(fileurl, System.Text.Encoding.Default, true);
            //var sr = File.OpenText(fileurl);
            string input;
            while ((input = sr.ReadLine()) != null)
                str += input;
            sr.Close();
        }
        catch (Exception e)
        {
        }
        return str;
    }

    public static string ReadFileWithoutEncoding(string fileurl)
    {
        var str = "";
        try {
            var sr = File.OpenText(fileurl);
            string input;
            while ((input = sr.ReadLine()) != null)
                str += input;
            sr.Close();
        }
        catch (Exception e)
        {
        }
        return str;
    }

    public static void AppendToFile(string fileurl, string text)
    {
        try
        {
            var sw = File.AppendText(fileurl);
            sw.WriteLine(text);
            sw.Close();
        }
        catch (Exception)
        {
        }
    }

    public static void DeleteFile(string fileurl)
    {
        try
        {
            File.Delete(fileurl);
        }
        catch (Exception)
        {
        }
    }
}