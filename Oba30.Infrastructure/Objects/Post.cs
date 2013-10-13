using System;
using System.Collections.Generic;

namespace Oba30.Infrastructure.Objects
{
    public class Post
    {
        public virtual int PostId { get; set; }
        public virtual string Title { get; set; }
        public virtual string ShortDescription { get; set; }
        public virtual string Description { get; set; }
        public virtual string Meta { get; set; }
        public virtual string UrlSlug { get; set; }
        public virtual bool Published { get; set; }
        public virtual DateTime PostedOn { get; set; }
        public virtual DateTime? ModifiedOn { get; set; }
        public virtual Category Category { get; set; }
        public virtual IList<Tag> Tags { get; set; }
    }
}