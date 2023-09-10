using MediatR;

namespace Accounting.Application.UseCases.Integration.TaskAssigned
{
    public class TaskAssignedInExternalSystem : INotification
    {
        public string TaskPid { get; }
        public string ParrotPid { get; }
        public decimal Amount { get; }

        public TaskAssignedInExternalSystem(string taskPid, string parrotPid, decimal amount)
        {
            TaskPid = taskPid;
            ParrotPid = parrotPid;
            Amount = amount;
        }
    }
}
