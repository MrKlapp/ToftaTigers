using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Diary.Helpers
{
 public static class UrlHelperExtensions  
{   
    // for regular pages:

    public static string Home(this UrlHelper helper)   
    {   
        return helper.Content("~/");   
    }   

    public static string Day(this UrlHelper helper)   
    {
        return helper.RouteUrl("Home/Day");   
    }

    // for media and css files:

    public static string Image(this UrlHelper helper, string fileName)   
    {   
        return helper.Content("~/Content/Images/"+fileName);   
    }
     
    public static string Style(this UrlHelper helper, string fileName)   
    {   
        return helper.Content("~/Content/CSS/"+fileName);
    } 

    public static string Script(this UrlHelper helper, string fileName)   
    {   
        return helper.Content("~/Content/Scripts/"+fileName);
    } 

    public static string NoAvatar(this UrlHelper helper)   
    {   
        return Image(helper, "NoAvatar.png");
    }
}
}