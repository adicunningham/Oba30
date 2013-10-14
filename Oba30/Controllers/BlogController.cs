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

        public ViewResult Search(string s, int p = 1)
        {
            ViewBag.Title = String.Format(@"Lists of posts found for search text ""{0}""", s);

            var viewModel = new ListViewModel(_blogRepository, s, "Search", p);
            return View("List", viewModel);
        }

    }
}
