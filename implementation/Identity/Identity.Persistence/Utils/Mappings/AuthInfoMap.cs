using FluentNHibernate.Mapping;
using Identity.Domain;

namespace Identity.Persistence.Utils.Mappings
{
    internal class AuthInfoMap : ClassMap<AuthInfo>
    {
        public AuthInfoMap()
        {
            Id(e => e.Id).GeneratedBy.Foreign("Parent");
            Map(e => e.CreatedAt).Generated.Insert();
            Map(e => e.PublicId).Not.Nullable().Length(255);

            Map(e => e.Login).Not.Nullable().Length(255);
            Map(e => e.FavoriteTreatHash).Not.Nullable();

            HasOne(e => e.Parent).ForeignKey("FK_AuthInfo_Parrot");

            Table("AuthInfo");
        }
    }
}
