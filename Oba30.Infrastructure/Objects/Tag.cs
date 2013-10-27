using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace Oba30.Infrastructure.Objects
{
    public class Tag
    {
        public virtual int TagId { get; set; }

        [Required(ErrorMessage = "Name: Field is required")]
        [StringLength(500, ErrorMessage = "Name: Length should not exceed 500 characters")]
        public virtual string Name { get; set; }

        [Required(ErrorMessage = "UrlSlug: Field is required")]
        [StringLength(500, ErrorMessage = "UrlSlug: Length should not exceed 500 characters")]

        public virtual string UrlSlug { get; set; }
        public virtual string Descripton { get; set; }
        [JsonIgnore]
        public virtual IList<Post> Posts {get; set; }
    }
}
