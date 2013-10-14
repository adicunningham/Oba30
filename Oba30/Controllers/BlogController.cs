using System;
using System.Web;
using System.Web.Mvc;
using Oba30.Infrastructure;
using Oba30.Models;

namespace Oba30.Controllers
{
    public class BlogController : Controller
    {
        private readonly IBlogRepository _blogRepository;

        public BlogController(IBlogRepository blogRepository)
        {
            _blogRepository = blogRepository;
        }

        public ViewResult Posts(int pageNo = 1)
        {
            var viewModel = new ListViewModel(_blogRepository, pageNo);

            ViewBag.Title = "Latest Posts";
            return View("List", viewModel);
        }

        public ViewResult Category(string category, int p = 1)
        {
            var viewModel = new ListViewModel(_blogRepository, category, "Category", p);

            if(viewModel.Category == null)
                throw new HttpException(404, "Category not Found");

            ViewBag.Title = String.Format(@"Latest posts on category ""{0}""", viewModel.Category.Name);
            return View("List", viewModel);
        }

        public ViewResult Tag(string tag, int p = 1)
        {
            var viewModel = new ListViewModel(_blogRepository, tag, "Tag", p);

            if (viewModel.Tag == null)
                throw new HttpException(404, "Tag not found");

            ViewBag.Title = String.Format(@"Latest posts tagged on ""{0}""", viewModel.Tag.Name);

            return View("List", viewModel);

        }

        /// <summary>
        /// Search action to find Posts by search term.
        /// </summary>
        /// <param name="s"></param>
        /// <param name="p"></param>
        /// <returns></returns>
        public ViewResult Search(string s, int p = 1)
        {
            ViewBag.Title = String.Format(@"Lists of posts found for search text ""{0}""", s);

            var viewModel = new ListViewModel(_blogRepository, s, "Search", p);
            return View("List", viewModel);
        }

        /// <summary>
        /// Post action to return post by year, month and title
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="title"></param>
        /// <returns></returns>
        public ViewResult Post(int year, int month, string title)
        {
            var post = _blogRepository.Post(year, month, title);

            if (post == null)
                throw new HttpException(404, "Post not found");

            if(post.Published == false && User.Identity.IsAuthenticated == false)
                throw new HttpException(401, "The post is not published");

            return View(post);
        }
    }
}
