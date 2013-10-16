using System;
using System.Web.Mvc;
using Newtonsoft.Json;
using Oba30.Infrastructure;
using Oba30.Models;
using Oba30.Providers;

namespace Oba30.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        private readonly IAuthProvider _authProvider;
        private readonly IBlogRepository _blogRepository;

        public AdminController(IAuthProvider authProvider, IBlogRepository blogRepository)
        {
            _authProvider = authProvider;
            _blogRepository = blogRepository;
        }

        /// <summary>
        /// Checks if user is authenticated and redirects as necessary.
        /// </summary>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            if (_authProvider.IsLoggedIn)
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
            return RedirectToAction("Manage");
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

        #region Posts

        public ContentResult Posts(JqInViewModel jqParams)
        {
            var posts = _blogRepository.Posts(jqParams.page - 1, jqParams.rows, jqParams.sidx, jqParams.sord == "asc");
            var totalPosts = _blogRepository.TotalPosts(false);

            return Content(JsonConvert.SerializeObject(new 
            {
                page = jqParams.page,
                records = totalPosts,
                rows = posts,
                total = Math.Ceiling(Convert.ToDouble(totalPosts)/jqParams.rows)
            }), "application/json");


        }

        #endregion

    }
}
