using System.Collections.Generic;
using Oba30.Infrastructure;
using Oba30.Infrastructure.Objects;

namespace Oba30.Models
{
    public class ListViewModel
    {
        public ListViewModel(IBlogRepository blogRepository, int pageNo)
        {
            Posts = blogRepository.Posts(pageNo - 1, 10);
            TotalPosts = blogRepository.TotalPosts();
        }

        public ListViewModel(IBlogRepository blogRepository, string categorySlug, int p)
        {
            Posts = blogRepository.PostsForCategory(categorySlug, p - 1, 10);
            TotalPosts = blogRepository.TotalPostsForCategory(categorySlug);
            Category = blogRepository.Category(categorySlug);
        }

        public IList<Post> Posts { get; private set; }
        public int TotalPosts { get; private set; }
        public Category Category { get; private set; }
    }
}