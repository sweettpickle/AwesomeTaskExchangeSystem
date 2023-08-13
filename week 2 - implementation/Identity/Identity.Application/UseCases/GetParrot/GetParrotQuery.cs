using Identity.Application.Utils.Common.Models;
using MediatR;

namespace Identity.Application.UseCases.GetParrot
{
    public class GetParrotQuery : IRequest<ParrotResult>
    {
        public string PublicId { get; }

        public GetParrotQuery(string publicId)
        {
            PublicId = publicId;
        }
    }
}
