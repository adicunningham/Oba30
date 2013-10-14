using System.Collections.Generic;
using NHibernate.Dialect;
using Oba30.Infrastructure.Objects;

namespace Oba30.Infrastructure
{
    public interface IBlogRepository
    {
        // Get Posts
        IList<Post> Posts(int pageNo, int pageSize);
        int TotalPosts();

        // Get posts by category
        IList<Post> PostsForCategory(string categorySlug, int pageNo, int pageSize);
        int TotalPostsForCategory(string categorySlug);
        Category Category(string categorySlug);

        // Get post by tag
        IList<Post> PostsForTag(string tagSlug, int pageNo, int pageSize);
        int TotalPostsForTag(string tagSlug);
        Tag Tag(string tagslug);

        // Search for posts
        IList<Post> PostsForSearch(string search, int pageNo, int pageSize);
        int TotalPostsForSearch(string search);
    }
}
