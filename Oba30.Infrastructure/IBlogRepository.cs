using System.Collections.Generic;
using Oba30.Infrastructure.Objects;

namespace Oba30.Infrastructure
{
    public interface IBlogRepository
    {
        IList<Post> Posts(int pageNo, int pageSize);
        int TotalPosts();
    }
}
