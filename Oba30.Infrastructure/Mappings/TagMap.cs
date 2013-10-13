using FluentNHibernate.Mapping;
using Oba30.Infrastructure.Objects;

namespace Oba30.Infrastructure.Mappings
{
    public class TagMap : ClassMap<Tag>
    {
        public TagMap()
        {
            Id(x => x.TagId);

            Map(x => x.Name)
                .Length(50)
                .Not.Nullable();

            Map(x => x.UrlSlug)
                .Length(50)
                .Not.Nullable();

            Map(x => x.Descripton)
                .Length(200);

            HasManyToMany(x => x.Posts)
                .Cascade.All().Inverse()
                .Table("PostTagMap");
        }
    }
}