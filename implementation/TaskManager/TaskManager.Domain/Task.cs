using TaskManager.Domain.Utils;

namespace TaskManager.Domain
{
    public class Task : Entity
    {
        public virtual string Name { get; protected set; }
        public virtual string Description { get; protected set;}
        public virtual decimal WriteOffAmount { get; protected set;}
        public virtual decimal AccrualAmount { get; protected set;}
        public virtual TaskStatusEnum Status { get; protected set;}
        public virtual long ParrotId { get; protected set; }
        public virtual Parrot Parrot { get; protected set; }

        protected Task() { }

        public Task(
            string name, 
            string description, 
            decimal writeOffAmount, 
            decimal accrualAmount, 
            Parrot parrot)
        {
            PublicId = $"{name}_{parrot.PublicId}_{CreatedAt}";
            Name = name;
            Description = description;
            WriteOffAmount = writeOffAmount;
            AccrualAmount = accrualAmount;
            Status = TaskStatusEnum.Opened;
            Parrot = parrot;
        }

        public virtual void Complete()
        {
            if (Status == TaskStatusEnum.Completed) return;

            Status = TaskStatusEnum.Completed;
        }
    }
}
