using FluentNHibernate.Mapping;
using Oba30.Infrastructure.Objects;

namespace Oba30.Infrastructure.Mappings
{
    public class CategoryMap : ClassMap<Category>
    {
        public CategoryMap()
        {
            Id(x => x.CategoryId);

            Map(x => x.Name)
                .Length(50)
                .Not.Nullable();

            Map(x => x.UrlSlug)
                .Length(50)
                .Not.Nullable();

            Map(x => x.Description)
                .Length(200);

            HasMany(x => x.Posts)
                .Inverse()
                .Inverse()
                .Cascade.All()
                .KeyColumn("CategoryId");

        }
    }
}