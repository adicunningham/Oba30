using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Oba30.Models;
using Oba30.Providers;

namespace Oba30.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        private readonly IAuthProvider _authProvider;

        public AdminController(IAuthProvider authProvider)
        {
            _authProvider = authProvider;
        }

        /// <summary>
        /// Checks if user is authenticated and redirects as necessary.
        /// </summary>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            if (_authProvider.IsLoggiedIn)
            {
                return Redirect(returnUrl);
            }

            ViewBag.ReturnUrl = returnUrl;

            return View();
        }

        /// <summary>
        /// Authenicates the user and redirects if authenticated.
        /// </summary>
        /// <param name="model"></param>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        [HttpPost, AllowAnonymous, ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel model, string returnUrl)
        {
            if (ModelState.IsValid && _authProvider.Login(model.UserName, model.Password))
            {
                return RedirectToUrl(returnUrl);
            }
            ModelState.AddModelError("", "The user name or password provided is incorrect.");
            return View();
        }

        private ActionResult RedirectToUrl(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Manage");
            }
        }

        public ActionResult Manage()
        {
            return View();
        }

        /// <summary>
        /// Logout of the system.
        /// </summary>
        /// <returns></returns>
        public ActionResult Logout()
        {
            _authProvider.Logout();

            return RedirectToAction("Login", "Admin");
        }
    }
}
