using Identity.Application.Utils.Common.Models;
using MediatR;

namespace Identity.Application.UseCases.Client.GetParrots
{
    public class GetParrotsQuery : IRequest<List<ParrotResult>>
    {
        public GetParrotsQuery() { }
    }
}
