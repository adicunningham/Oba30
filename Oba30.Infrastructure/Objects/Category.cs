using System.Collections.Generic;

namespace Oba30.Infrastructure.Objects
{
    public class Category
    {
        public virtual int CategoryId { get; set; }
        public virtual string Name { get; set; }
        public virtual string UrlSlug { get; set; }
        public virtual string Description { get; set; }
        public virtual IList<Post> Posts { get; set; } 
    }
}
