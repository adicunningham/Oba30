using System.Collections.Generic;
using Newtonsoft.Json;

namespace Oba30.Infrastructure.Objects
{
    public class Category
    {
        public virtual int CategoryId { get; set; }
        public virtual string Name { get; set; }
        public virtual string UrlSlug { get; set; }
        public virtual string Description { get; set; }
        [JsonIgnore]
        public virtual IList<Post> Posts { get; set; } 
    }
}
