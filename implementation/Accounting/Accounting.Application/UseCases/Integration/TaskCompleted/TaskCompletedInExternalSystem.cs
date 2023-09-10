using MediatR;

namespace Accounting.Application.UseCases.Integration.TaskCompleted
{
    public class TaskCompletedInExternalSystem : INotification
    {
        public string TaskPid { get; }
        public string ParrotPid { get; }
        public decimal Amount { get; }

        public TaskCompletedInExternalSystem(string taskPid, string parrotPid, decimal amount)
        {
            TaskPid = taskPid;
            ParrotPid = parrotPid;
            Amount = amount;
        }
    }
}
