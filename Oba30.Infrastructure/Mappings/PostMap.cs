using FluentNHibernate.Mapping;
using Oba30.Infrastructure.Objects;

namespace Oba30.Infrastructure.Mappings
{
    public class PostMap : ClassMap<Post>
    {
        public PostMap()
        {
            Id(x => x.PostId);

            Map(x => x.Title)
                .Length(500)
                .Not.Nullable();

            Map(x => x.ShortDescription)
                .Length(5000)
                .Not.Nullable();

            Map(x => x.Description)
                .Length(5000)
                .Not.Nullable();

            Map(x => x.Meta)
                .Length(1000)
                .Not.Nullable();

            Map(x => x.UrlSlug)
                .Length(200)
                .Not.Nullable();

            Map(x => x.Published)
                .Not.Nullable();

            Map(x => x.PostedOn)
                .Not.Nullable();

            Map(x => x.ModifiedOn);

            
            References(x => x.Category)
                .Column("CategoryId")
                .Not.Nullable();

            HasManyToMany(x => x.Tags)
                .Table("PostTagMap");
        }
    }
}