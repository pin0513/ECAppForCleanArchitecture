using ECApp.Application.Common.Interfaces;
using ECApp.Application.User.ViewModels;
using MediatR;

namespace ECApp.Application.User.Queries;

public class GetUserQuery : IRequest<UserVm>
{
    public Guid Id { get; set; }
}

public class GetUserQueryHandler(IECDbContext context) : IRequestHandler<GetUserQuery, UserVm>
{
    public async Task<UserVm> Handle(GetUserQuery request, CancellationToken cancellationToken)
    {
        var user = context.Users.FirstOrDefault(a=>a.Id == request.Id);
        if (user == null) throw new Exception("User not found");
        
        return new UserVm
        {
            Id = user.Id,
            Name = user.Name,
        };
    }
}