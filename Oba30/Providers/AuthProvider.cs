﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace Oba30.Providers
{
    public class AuthProvider : IAuthProvider
    {
        public bool IsLoggiedIn
        {
            get
            {
                return HttpContext.Current.User.Identity.IsAuthenticated;
            }
        }

        public bool Login(string username, string password)
        {
            bool result = FormsAuthentication.Authenticate(username, password);

            if(result)
                FormsAuthentication.SetAuthCookie(username, false);

            return result;
        }

        public void Logout()
        {
            FormsAuthentication.SignOut();
        }
    }
}