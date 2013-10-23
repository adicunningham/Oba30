using System;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Newtonsoft.Json;
using Oba30.Helper;
using Oba30.Infrastructure;
using Oba30.Infrastructure.Objects;
using Oba30.Models;
using Oba30.Providers;

namespace Oba30.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        private readonly IAuthProvider _authProvider;
        private readonly IBlogRepository _blogRepository;

        public AdminController(IAuthProvider authProvider, IBlogRepository blogRepository = null)
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

        #region Admin Posts

        /// <summary>
        /// Returns Posts in Json.
        /// </summary>
        /// <param name="jqParams">Input parameters from the JQGrid.</param>
        /// <returns></returns>
        public ContentResult Posts(JqInViewModel jqParams)
        {
            var posts = _blogRepository.Posts(jqParams.page - 1, jqParams.rows, jqParams.sidx, jqParams.sord == "asc");
            var totalPosts = _blogRepository.TotalPosts(false);

            return Content(JsonConvert.SerializeObject(new 
            {
                page = jqParams.page,
                records = totalPosts,
                rows = posts,
                total = Math.Ceiling(Convert.ToDouble(totalPosts)/jqParams.rows),
            }, new CustomDateTimeConverter()), "application/json");
        }

        /// <summary>
        /// Adds a new post to the database.
        /// </summary>
        /// <param name="post"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        public ContentResult AddPost(Post post)
        {
            string json;

            ModelState.Clear();

            if (TryValidateModel(post))
            {
                var id = _blogRepository.AddPost(post);

                json = JsonConvert.SerializeObject(new
                {
                    id = id,
                    success = true,
                    message = "Post added successfully."
                });
            }
            else
            {
                json = JsonConvert.SerializeObject(new
                {
                    id = 0,
                    success = false,
                    message = "Failed to add the post."
                });
            }

            return Content(json, "application/json");
        }

        /// <summary>
        /// Returns Categories Html for the Categories dropdown on the tinyMCE Add post form.
        /// </summary>
        /// <returns></returns>
        public ContentResult GetCategoriesHtml()
        {
            var categories = _blogRepository.Categories().OrderBy(s => s.Name);

            var sb = new StringBuilder();
            sb.AppendLine(@"<select>");

            foreach (var category in categories)
            {
                sb.AppendLine(string.Format(@"<option value=""{0}"">{1}</option>", category.CategoryId, category.Name));
            }

            sb.AppendLine("</select>");

            return Content(sb.ToString(), "text/html");
        }

        public ContentResult GetTagsHtml()
        {
            var tags = _blogRepository.Tags().OrderBy(s => s.Name);

            var sb = new StringBuilder();
            sb.AppendLine(@"<select multiple=""multiple"">");

            foreach (var tag in tags)
            {
                sb.AppendLine(string.Format(@"<option value=""{0}"">{1}</option>", tag.TagId, tag.Name));
            }

            sb.AppendLine("</select>");
            return Content(sb.ToString(), "text/html");
        }

        #endregion

    }
}
