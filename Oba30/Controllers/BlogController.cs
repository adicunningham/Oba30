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

        public ViewResult Posts(int pageNo =2)
        {
            var viewModel = new ListViewModel(_blogRepository, pageNo);

            ViewBag.Title = "Latest Posts";
            return View("List", viewModel);
        }

    }
}
