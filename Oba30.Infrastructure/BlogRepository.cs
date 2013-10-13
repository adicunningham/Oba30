using System.Collections.Generic;
using System.Linq;
using NHibernate;
using NHibernate.Linq;
using Oba30.Infrastructure.Objects;

namespace Oba30.Infrastructure
{
    public class BlogRepository : IBlogRepository
    {

        // NHiberate Object
        private readonly ISession _session;

        public BlogRepository(ISession session)
        {
            _session = session;
        }

        public IList<Post> Posts(int pageNo, int pageSize)
        {
            var query = _session.Query<Post>()
                .Where(p => p.Published)
                .OrderByDescending(p => p.PostedOn)
                .Skip(pageNo*pageSize)
                .Take(pageSize)
                .Fetch(p => p.Category);

            query.FetchMany(p => p.Tags).ToFuture();

            return query.ToFuture().ToList();
        }

        public int TotalPosts()
        {
            return _session.Query<Post>().Where(p => p.Published).Count();
        }
    }
}