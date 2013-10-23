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
        public int TotalPosts(bool checkIsPublished = true)
        {
            return _session.Query<Post>()
                .Where(p => checkIsPublished || p.Published == true)
                .Count();
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
        /// Returns Category by category slug.
        /// </summary>
        /// <param name="categorySlug"></param>
        /// <returns></returns>
        public Category Category(string categorySlug)
        {
            return _session.Query<Category>()
                .FirstOrDefault(t => t.UrlSlug.Equals(categorySlug));
        }


        /// <summary>
        /// Returns Category by CategoryId
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        public Category Category(int categoryId)
        {
            return _session.Query<Category>().FirstOrDefault(c => c.CategoryId == categoryId);
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
        /// Returns Tag by TagId
        /// </summary>
        /// <param name="tagId"></param>
        /// <returns></returns>
        public Tag Tag(int tagId)
        {
            return _session.Query<Tag>().FirstOrDefault(t => t.TagId == tagId);
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


        /// <summary>
        /// Returns a list of categories
        /// </summary>
        /// <returns></returns>
        public IList<Category> Categories()
        {
            return _session.Query<Category>().OrderBy(p => p.Name).ToList();
        }

        /// <summary>
        /// Returns a list of Tags
        /// </summary>
        /// <returns></returns>
        public IList<Tag> Tags()
        {
            return _session.Query<Tag>().OrderBy(p => p.Name).ToList();
        }


        /// <summary>
        /// Posts for Admin Controller
        /// </summary>
        /// <param name="pageNo"></param>
        /// <param name="pageSize"></param>
        /// <param name="sortColumn"></param>
        /// <param name="sortByAscending"></param>
        /// <returns></returns>
        public IList<Post> Posts(int pageNo, int pageSize, string sortColumn, bool sortByAscending)
        {
            IQueryable<Post> query = null;

            switch (sortColumn)
            {
                case "Title":
                    if (sortByAscending)
                        query = _session.Query<Post>()
                            .OrderBy(p => p.Title)
                            .Skip(pageNo*pageSize)
                            .Take(pageSize)
                            .Fetch(p => p.Category);
                    else
                    {
                        query = _session.Query<Post>()
                            .OrderByDescending(p => p.Title)
                            .Skip(pageNo*pageSize)
                            .Take(pageSize)
                            .Fetch(p => p.Category);
                    }
                    break;
                case "Published":
                    if (sortByAscending)
                        query = _session.Query<Post>()
                            .OrderBy(p => p.Published)
                            .Skip(pageNo*pageSize)
                            .Take(pageSize)
                            .Fetch(p => p.Category);
                    else
                    {
                        query = _session.Query<Post>()
                            .OrderByDescending(p => p.Published)
                            .Skip(pageNo*pageSize)
                            .Take(pageSize)
                            .Fetch(p => p.Category);
                    }
                    break;
                case "PostedOn":
                    if (sortByAscending)
                        query = _session.Query<Post>()
                            .OrderBy(p => p.PostedOn)
                            .Skip(pageNo*pageSize)
                            .Take(pageSize)
                            .Fetch(p => p.Category);
                    else
                    {
                        query = _session.Query<Post>()
                            .OrderByDescending(p => p.PostedOn)
                            .Skip(pageNo*pageSize)
                            .Take(pageSize)
                            .Fetch(p => p.Category);
                    }
                    break;
                case "Modified":
                    if (sortByAscending)
                        query = _session.Query<Post>()
                            .OrderBy(p => p.ModifiedOn)
                            .Skip(pageNo*pageSize)
                            .Take(pageSize)
                            .Fetch(p => p.Category);
                    else
                    {
                        query = _session.Query<Post>()
                            .OrderByDescending(p => p.ModifiedOn)
                            .Skip(pageNo*pageSize)
                            .Take(pageSize)
                            .Fetch(p => p.Category);
                    }
                    break;
                case "Category":
                    if (sortByAscending)
                        query = _session.Query<Post>()
                            .OrderBy(p => p.Category)
                            .Skip(pageNo*pageSize)
                            .Take(pageSize)
                            .Fetch(p => p.Category);
                    else
                    {
                        query = _session.Query<Post>()
                            .OrderByDescending(p => p.Category)
                            .Skip(pageNo*pageSize)
                            .Take(pageSize)
                            .Fetch(p => p.Category);
                    }
                    break;
            }

            query.FetchMany(p => p.Tags).ToFuture();

            return query.ToFuture().ToList();
        }

        /// <summary>
        /// Add Post to database
        /// </summary>
        /// <param name="post"></param>
        /// <returns></returns>
        public int AddPost(Post post)
        {
            using (var tran = _session.BeginTransaction())
            {
                _session.Save(post);
                tran.Commit();
                return post.PostId;
            }
        }
    }
}