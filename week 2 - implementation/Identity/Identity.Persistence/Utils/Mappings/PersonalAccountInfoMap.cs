using FluentNHibernate.Mapping;
using Identity.Domain;

namespace Identity.Persistence.Utils.Mappings
{
    internal class PersonalAccountInfoMap : ClassMap<PersonalAccountInfo>
    {
        public PersonalAccountInfoMap()
        {
            Id(e => e.Id).GeneratedBy.Foreign("Parent");
            Map(e => e.CreatedAt).Generated.Insert();
            Map(e => e.PublicId).Not.Nullable().Length(255);

            Map(e => e.AccountNumber).Not.Nullable().Length(255);
            Map(e => e.AccountNickname).Not.Nullable().Length(255);

            HasOne(e => e.Parent).ForeignKey("FK_PersonalAccountInfo_Parrot");

            Table("PersonalAccountInfo");
        }
    }
}
