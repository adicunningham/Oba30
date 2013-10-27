using System.Collections.Generic;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;


namespace Oba30.Infrastructure.Objects
{
    public class Category
    {
        public virtual int CategoryId { get; set; }

        [Required(ErrorMessage = "Name: Field is required")]
        [StringLength(500, ErrorMessage = "Name: Length should not exceed 500 characters")]
        public virtual string Name { get; set; }

        [Required(ErrorMessage = "UrlSlug: Field is required")]
        [StringLength(500, ErrorMessage = "Name: Length should not exceed 500 characters")]
        public virtual string UrlSlug { get; set; }
        public virtual string Description { get; set; }
        [JsonIgnore]
        public virtual IList<Post> Posts { get; set; } 
    }
}
