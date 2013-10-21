using System.Collections.Generic;
using Newtonsoft.Json;

namespace Oba30.Infrastructure.Objects
{
    public class Tag
    {
        public virtual int TagId { get; set; }
        public virtual string Name { get; set; }
        public virtual string UrlSlug { get; set; }
        public virtual string Descripton { get; set; }
        [JsonIgnore]
        public virtual IList<Post> Posts {get; set; }
    }
}
