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

        /// <summary>
        /// Blog Repository Constructor
        /// </summary>
        /// <param name="session"></param>
        public BlogRepository(ISession session)
        {
            _session = session;
        }

        /// <summary>
        /// Gets a list of Posts by page number and page size
        /// </summary>
        /// <param name="pageNo"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Returns total number of posts
        /// </summary>
        /// <returns></returns>
        public int TotalPosts()
        {
            return _session.Query<Post>().Where(p => p.Published).Count();
        }

        /// <summary>
        /// Returns list of posts by Category
        /// </summary>
        /// <param name="categorySlug"></param>
        /// <param name="pageNo"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Returns total number of posts by category
        /// </summary>
        /// <param name="categorySlug"></param>
        /// <returns></returns>
        public int TotalPostsForCategory(string categorySlug)
        {
            return _session.Query<Post>()
                .Where(p => p.Published && p.Category.UrlSlug.Equals(categorySlug))
                .Count();
        }

        /// <summary>
        /// Returns Category
        /// </summary>
        /// <param name="categorySlug"></param>
        /// <returns></returns>
        public Category Category(string categorySlug)
        {
            return _session.Query<Category>()
                .FirstOrDefault(t => t.UrlSlug.Equals(categorySlug));
        }

        /// <summary>
        /// Returns list of posts by Tag
        /// </summary>
        /// <param name="tagSlug"></param>
        /// <param name="pageNo"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Retruns total number of posts by tag
        /// </summary>
        /// <param name="tagSlug"></param>
        /// <returns></returns>
        public int TotalPostsForTag(string tagSlug)
        {
            return _session.Query<Post>().Count(p => p.Published && p.Tags.Any(t => t.UrlSlug.Equals(tagSlug)));
        }

        /// <summary>
        /// Returns Tag
        /// </summary>
        /// <param name="tagslug"></param>
        /// <returns></returns>
        public Tag Tag(string tagslug)
        {
            return _session.Query<Tag>()
                .FirstOrDefault(t => t.UrlSlug.Equals(tagslug));
        }

        /// <summary>
        /// Returns list of posts by search term
        /// </summary>
        /// <param name="search"></param>
        /// <param name="pageNo"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Returns total number of posts by search term
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        public int TotalPostsForSearch(string search)
        {
            return _session.Query<Post>().Count(p => p.Published &&
                                                     (p.Title.Contains(search) || p.Category.Name.Equals(search) ||
                                                      p.Tags.Any(t => t.Name.Equals(search))));
        }

        /// <summary>
        /// Returns Post by year, month and title.
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="titleSlug"></param>
        /// <returns></returns>
        public Post Post(int year, int month, string titleSlug)
        {
            var query = _session.Query<Post>()
                .Where(p => p.PostedOn.Year == year && p.PostedOn.Month == month && p.UrlSlug.Equals(titleSlug))
                .Fetch(p => p.Category);

            query.FetchMany(p => p.Tags).ToFuture();

            return query.ToFuture().FirstOrDefault();
        }
    }
}