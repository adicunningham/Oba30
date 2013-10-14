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


        public IList<Post> PostsForCategory(string categorySlug, int pageNo, int pageSize)
        {
            var query = _session.Query<Post>()
                .Where(p => p.Published && p.Category.UrlSlug.Equals(categorySlug))
                .OrderByDescending(p => p.PostedOn)
                .Skip(pageNo*pageSize)
                .Take(pageSize)
                .Fetch(p => p.Category);

            query.FetchMany(p => p.Tags).ToFuture();

            return query.ToFuture().ToList();

        }

        public int TotalPostsForCategory(string categorySlug)
        {
            return _session.Query<Post>()
                .Where(p => p.Published && p.Category.UrlSlug.Equals(categorySlug))
                .Count();
        }

        public Category Category(string categorySlug)
        {
            return _session.Query<Category>()
                .FirstOrDefault(t => t.UrlSlug.Equals(categorySlug));
        }


        public IList<Post> PostsForTag(string tagSlug, int pageNo, int pageSize)
        {
            var query = _session.Query<Post>()
                .Where(p => p.Published && p.Tags.Any(t => t.UrlSlug.Equals(tagSlug)))
                .OrderByDescending(p => p.PostedOn)
                .Skip(pageNo*pageSize)
                .Fetch(p => p.Category);

            query.FetchMany(p => p.Tags).ToFuture();

            return query.ToFuture().ToList();
        }

        public int TotalPostsForTag(string tagSlug)
        {
            return _session.Query<Post>().Count(p => p.Published && p.Tags.Any(t => t.UrlSlug.Equals(tagSlug)));
        }

        public Tag Tag(string tagslug)
        {
            return _session.Query<Tag>()
                .FirstOrDefault(t => t.UrlSlug.Equals(tagslug));
        }


        public IList<Post> PostsForSearch(string search, int pageNo, int pageSize)
        {
            var query = _session.Query<Post>()
                .Where(p => p.Published && (p.Title.Contains(search)
                                            || p.Category.Name.Equals(search)
                                            || p.Tags.Any(t => t.Name.Equals(search))))
                .OrderByDescending(p => p.PostedOn)
                .Skip(pageNo*pageSize)
                .Take(pageSize)
                .Fetch(p => p.Category);

            query.FetchMany(p => p.Tags).ToFuture();

            return query.ToFuture().ToList();
        }

        public int TotalPostsForSearch(string search)
        {
            return _session.Query<Post>().Count(p => p.Published &&
                                                     (p.Title.Contains(search) || p.Category.Name.Equals(search) ||
                                                      p.Tags.Any(t => t.Name.Equals(search))));
        }
    }
}