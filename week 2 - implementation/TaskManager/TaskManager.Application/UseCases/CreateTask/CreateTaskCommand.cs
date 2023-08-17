using MediatR;
using TaskManager.Application.Utils.Common.Models;

namespace TaskManager.Application.UseCases.CreateTask
{
    public class CreateTaskCommand : IRequest<TaskResult>
    {
        public string Name { get; }
        public string Description { get; }
        public string ParrotPid { get; }

        public CreateTaskCommand(string name, string description, string parrotPid)
        {
            Name = name;
            Description = description;
            ParrotPid = parrotPid;
        }
    }
}
