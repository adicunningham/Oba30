using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using NHibernate.Dialect;
using Oba30.Infrastructure.Objects;

namespace Oba30.Infrastructure
{
    public interface IBlogRepository
    {
        // Get Posts
        IList<Post> Posts(int pageNo, int pageSize);
        int TotalPosts(bool checkIsPublished = true);

        // Get posts by category
        IList<Post> PostsForCategory(string categorySlug, int pageNo, int pageSize);
        int TotalPostsForCategory(string categorySlug);
        Category Category(string categorySlug);
        Category Category(int categoryId);

        // Get post by tag
        IList<Post> PostsForTag(string tagSlug, int pageNo, int pageSize);
        int TotalPostsForTag(string tagSlug);
        Tag Tag(string tagslug);
        Tag Tag(int tagId);

        // Search for posts
        IList<Post> PostsForSearch(string search, int pageNo, int pageSize);
        int TotalPostsForSearch(string search);

        Post Post(int year, int month, string titleSlug);

        // Return all categories
        IList<Category> Categories();


        // Return all tags
        IList<Tag> Tags();

        
        /// <summary>
        /// Admin Page Posts method.
        /// </summary>
        /// <param name="pageNo"></param>
        /// <param name="pageSize"></param>
        /// <param name="sortColumn"></param>
        /// <param name="sortByAscending"></param>
        /// <returns></returns>
        IList<Post> Posts(int pageNo, int pageSize, string sortColumn, bool sortByAscending);

        int AddPost(Post post);

        /// <summary>
        /// Edit an existing post
        /// </summary>
        /// <param name="post"></param>
        void EditPost(Post post);

        /// <summary>
        /// Delete a post.
        /// </summary>
        /// <param name="id"></param>
        void DeletePost(int id);


        /// <summary>
        /// Add a Category
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        int AddCategory(Category category);

        /// <summary>
        /// Edit a category
        /// </summary>
        /// <param name="category"></param>
        void EditCategory(Category category);


        /// <summary>
        /// Delete a category
        /// </summary>
        /// <param name="id"></param>
        void DeleteCategory(int id);
    }
}
