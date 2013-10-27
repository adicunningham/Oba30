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

        [HttpPost, ValidateInput(false)]
        public ContentResult EditPost(Post post)
        {
            string json;
            
            ModelState.Clear();

            if (TryValidateModel(post))
            {
                _blogRepository.EditPost(post);
                json = JsonConvert.SerializeObject(new 
                {
                    id = post.PostId,
                    success = true,
                    message = "Changes saved successfully."
                });
            }
            else
            {
                json = JsonConvert.SerializeObject(new
                {
                    id = 0,
                    success = false,
                    message = "Failed to save the changes."
                });
            }

            return Content(json, "application/json");
        }

        [HttpPost]
        public ContentResult DeletePost(int id)
        {
            _blogRepository.DeletePost(id);

            var json = JsonConvert.SerializeObject(new
            {
                id = 0,
                success = true,
                message = "Post deleted successfully."
            });

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

        #region Admin Categories

        /// <summary>
        /// Returns a list of cateogories in Json format.
        /// </summary>
        /// <returns></returns>
        public ContentResult Categories()
        {
            var categories = _blogRepository.Categories();

            return Content(JsonConvert.SerializeObject(new
            {
                page = 1,
                records = categories.Count,
                rows = categories,
                total = 1
            }), "application/json");
        }

        [HttpPost]
        public ContentResult AddCategory([Bind(Exclude = "CategoryId")] Category category)
        {
            string json;

            if (ModelState.IsValid)
            {
                var id = _blogRepository.AddCategory(category);
                json = JsonConvert.SerializeObject(new
                {
                    id = id,
                    success = true,
                    message = "Category added successfully."
                });
            }
            else
            {
                {
                    json = JsonConvert.SerializeObject(new
                    {
                        id = 0,
                        success = false,
                        message = "Failed to add the category."
                    });
                }
            }
            return Content(json, "application/json");
        }

        [HttpPost]
        public ContentResult EditCategory(Category category)
        {
            string json;

            if (ModelState.IsValid)
            {
                _blogRepository.EditCategory(category);
                json = JsonConvert.SerializeObject(new
                {
                    id = category.CategoryId,
                    success = true,
                    message = "Changes saved successfully"
                });
            }
            else
            {
                json = JsonConvert.SerializeObject(new
                {
                    id = 0,
                    success = false,
                    message = "Failed to save the changes."
                });
            }

            return Content(json, "application/json");
        }


        [HttpPost]
        public ContentResult DeleteCategory(int id)
        {
            _blogRepository.DeleteCategory(id);

            var json = JsonConvert.SerializeObject(new
            {
                id = 0,
                success = true,
                message = "Category deleted successfully."
            });

            return Content(json, "application/json");
        }

        public ContentResult Tags()
        {
            var tags = _blogRepository.Tags();

            return Content(JsonConvert.SerializeObject(new
            {
                page = 1,
                records = tags.Count,
                rows = tags,
                total = 1
            }), "application/json");
        }

        [HttpPost]
        public ContentResult AddTag([Bind(Exclude = "TagId")] Tag tag)
        {
            string json;

            if (ModelState.IsValid)
            {
                var id = _blogRepository.AddTag(tag);
                json = JsonConvert.SerializeObject(new
                {
                    id = id,
                    success = true,
                    message = "Tag added successfully."
                });
            }
            else
            {
                json = JsonConvert.SerializeObject(new
                {
                    id = 0,
                    success = false,
                    message = "Failed to add the tag."
                });
            }

            return Content(json, "application/json");
        }

        [HttpPost]
        public ContentResult EditTag(Tag tag)
        {
            string json;

            if (ModelState.IsValid)
            {
                _blogRepository.EditTag(tag);
                json = JsonConvert.SerializeObject(new
                {
                    id = tag.TagId,
                    success = true,
                    message = "Changes saved successfully."
                });
            }
            else
            {
                json = JsonConvert.SerializeObject(new
                {
                    id = 0,
                    success = false,
                    message = "Failed to save the changes."
                });
            }

            return Content(json, "application/json");
        }

        [HttpPost]
        public ContentResult DeleteTag(int id)
        {
            _blogRepository.DeleteTag(id);

            var json = JsonConvert.SerializeObject(new
            {
                id = 0,
                success = true,
                message = "Tag deleted successfully."
            });

            return Content(json, "application/json");
        }
        #endregion
    }
}
