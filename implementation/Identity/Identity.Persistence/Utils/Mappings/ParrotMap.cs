using FluentNHibernate.Mapping;
using Identity.Domain;
using NHibernate.Type;

namespace Identity.Persistence.Utils.Mappings
{
    internal class ParrotMap : ClassMap<Parrot>
    {
        public ParrotMap()
        {
            Id(e => e.Id).GeneratedBy.Identity();
            Map(e => e.CreatedAt).Generated.Insert();
            Map(e => e.PublicId).Not.Nullable().Length(255);

            Map(e => e.Nickname).Not.Nullable().Length(255);
            Map(e => e.Email).Not.Nullable().Length(255);
            Map(e => e.Address).Nullable().Length(255);
            Map(e => e.Role).CustomType<EnumStringType<RoleEnum>>();

            HasOne(e => e.AuthInfo).Cascade.All();
            HasOne(e => e.PersonalAccountInfo).Cascade.All();

            Table("Parrot");
        }
    }
}
