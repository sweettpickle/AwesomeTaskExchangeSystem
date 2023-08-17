using MediatR;

namespace TaskManager.Integration.UseCases.SendMessage
{
    internal class SendMessageCommand : IRequest
    {
        public string Topic { get; }
        public object Message { get; }

        public SendMessageCommand(string topic, object message)
        {
            Topic = topic;
            Message = message;
        }
    }
}
