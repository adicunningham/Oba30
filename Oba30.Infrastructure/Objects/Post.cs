using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.ComponentModel.DataAnnotations;


namespace Oba30.Infrastructure.Objects
{
    public class Post
    {
        [Required(ErrorMessage = "Id: Field is required")]
        public virtual int PostId { get; set; }

        [Required(ErrorMessage = "Title: Field is required")]
        public virtual string Title { get; set; }

        [Required(ErrorMessage = "ShortDescription: Field is required")]
        public virtual string ShortDescription { get; set; }

        [Required(ErrorMessage = "Description: Field is required")]
        public virtual string Description { get; set; }

        [Required(ErrorMessage = "Meta: Field is required")]
        [StringLength(1000, ErrorMessage = "Meta: Length should not exceed 1000 characters")]
        public virtual string Meta { get; set; }

        [Required(ErrorMessage = "UrlSlug: Field is required")]
        [StringLength(50, ErrorMessage = "UrlSlug should not exceed 50 characters")]
        public virtual string UrlSlug { get; set; }


        public virtual bool Published { get; set; }

        [Required(ErrorMessage = "PostedOn: Field is required")]
        public virtual DateTime PostedOn { get; set; }
        public virtual DateTime? ModifiedOn { get; set; }
        public virtual Category Category { get; set; }
        public virtual IList<Tag> Tags { get; set; }
    }
}