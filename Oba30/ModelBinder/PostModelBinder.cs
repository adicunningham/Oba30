using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;
using Ninject;
using Ninject.Planning.Bindings;
using Oba30.Infrastructure;
using Oba30.Infrastructure.Objects;

namespace Oba30.ModelBinder
{
    public class PostModelBinder : DefaultModelBinder
    {
        private readonly IKernel _kernel ;

        public PostModelBinder(IKernel kernel)
        {
            _kernel = kernel;
        }

        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var post = (Post)base.BindModel(controllerContext, bindingContext);

            var _blogRepository = _kernel.Get<IBlogRepository>();

            if (post.Category != null)
            {
                post.Category = _blogRepository.Category(post.Category.CategoryId);
            }

            var tags = bindingContext.ValueProvider.GetValue("Tags").AttemptedValue.Split(',');

            if (tags.Length > 0)
            {
                post.Tags = new List<Tag>();

                foreach (var tag in tags)
                {
                    post.Tags.Add(_blogRepository.Tag(int.Parse(tag.Trim())));
                }
            }

            if (bindingContext.ValueProvider.GetValue("oper").AttemptedValue.Equals("edit"))
            {
                post.ModifiedOn = DateTime.UtcNow;
            }
            else
            {
                post.PostedOn = DateTime.UtcNow;
            }

            return post;
        }
    }
}