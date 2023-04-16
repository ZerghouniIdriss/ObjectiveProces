using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OkrsEntreprise.Api.App_Start
{
    public class CommonHelperMethods
    {
        public static string ResolveAvatarPath(string avatar)
        {
            if (!string.IsNullOrEmpty(avatar))
            {
                avatar = string.Concat(HttpContext.Current.Request.Url.AbsoluteUri.Replace(HttpContext.Current.Request.Url.AbsolutePath.ToString(), ""), avatar);
            }
            else
            {
                avatar = "images/user.png";
            }

            return avatar;
        }


        public static string ResolveAvatarPathForTeam(string avatar)
        {
            if (!string.IsNullOrEmpty(avatar))
            {
                avatar = string.Concat(HttpContext.Current.Request.Url.AbsoluteUri.Replace(HttpContext.Current.Request.Url.AbsolutePath.ToString(), ""), avatar);
            }
            else
            {
                avatar = "images/team.png";
            }

            return avatar;
        }


    }
}