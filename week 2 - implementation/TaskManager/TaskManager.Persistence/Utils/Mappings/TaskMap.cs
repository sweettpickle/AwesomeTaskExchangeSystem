using FluentNHibernate.Mapping;
using NHibernate.Type;

namespace TaskManager.Persistence.Utils.Mappings
{
    internal class TaskMap : ClassMap<Domain.Task>
    {
        public TaskMap()
        {
            Id(e => e.Id).GeneratedBy.Identity();
            Map(e => e.CreatedAt).Generated.Insert();
            Map(e => e.PublicId).Not.Nullable().Length(255);

            Map(e => e.Name).Not.Nullable().Length(255);
            Map(e => e.Description).Not.Nullable().Length(1000);
            Map(e => e.WriteOffAmount).Not.Nullable();
            Map(e => e.AccrualAmount).Not.Nullable();
            Map(e => e.Status).CustomType<EnumStringType<TaskStatusEnum>>();

            Map(e => e.ParrotId).Not.Nullable();
            HasOne(e => e.Parrot).ForeignKey("FK_Task_Parrot");


            Table("Task");
        }
    }
}
