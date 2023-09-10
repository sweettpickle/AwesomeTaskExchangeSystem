using Accounting.Domain;
using FluentNHibernate.Mapping;
using NHibernate.Type;

namespace Accounting.Persistence.Utils.Mappings
{
    internal class ParrotMap : ClassMap<Parrot>
    {
        public ParrotMap()
        {
            Id(e => e.Id).GeneratedBy.Identity();
            Map(e => e.CreatedAt).Generated.Insert();
            Map(e => e.PublicId).Not.Nullable().Length(255);

            Map(e => e.Email).Not.Nullable().Length(255);
            Map(e => e.Role).CustomType<EnumStringType<RoleEnum>>();

            Table("Parrot");
        }
    }
}
