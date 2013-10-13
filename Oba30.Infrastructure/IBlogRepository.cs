using System.Collections.Generic;
using NHibernate.Dialect;
using Oba30.Infrastructure.Objects;

namespace Oba30.Infrastructure
{
    public interface IBlogRepository
    {
        IList<Post> Posts(int pageNo, int pageSize);
        int TotalPosts();
        IList<Post> PostsForCategory(string categorySlug, int pageNo, int pageSize);
        int TotalPostsForCategory(string categorySlug);
        Category Category(string categorySlug);
    }
}
