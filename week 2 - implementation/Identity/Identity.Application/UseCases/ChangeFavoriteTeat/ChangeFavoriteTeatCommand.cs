using MediatR;

namespace Identity.Application.UseCases.ChangePassword
{
    public class ChangeFavoriteTeatCommand : IRequest
    {
        public string PublicId { get; }
        public string NewFavoriteTeat { get; }

        public ChangeFavoriteTeatCommand(string publicId, string newFavoriteTeat)
        {
            PublicId = publicId;
            NewFavoriteTeat = newFavoriteTeat;
        }
    }
}
