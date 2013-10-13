using System.Collections.Generic;
using Oba30.Infrastructure;
using Oba30.Infrastructure.Objects;

namespace Oba30.Models
{
    public class ListViewModel
    {
        public ListViewModel(IBlogRepository _blogRepository, int pageNo)
        {
            Posts = _blogRepository.Posts(pageNo - 1, 10);
            TotalPosts = _blogRepository.TotalPosts();
        }

        public IList<Post> Posts { get; private set; }
        public int TotalPosts { get; private set; }
    }
}