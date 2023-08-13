using MediatR;

namespace Identity.Integration.UseCases.SendMessageToExternalSystems
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
