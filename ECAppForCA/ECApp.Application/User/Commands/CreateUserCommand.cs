using ECApp.Application.Common.Interfaces;
using ECApp.Domain.Entities;
using MediatR;

namespace ECApp.Application.User.Commands;

public class CreateUserCommand : IRequest<bool>
{
    public Guid Id { get; set; }
    public string Name { get; set; }
}

public class CreateUserCommandHandler(IECDbContext context) : IRequestHandler<CreateUserCommand, bool>
{
    public async Task<bool> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        context.Users.Add(new Users()
        {
            Id = Guid.NewGuid(),
            CreatedTime = DateTimeOffset.UtcNow,
            Creator = "System",
        });

        return await context.SaveChangesAsync(CancellationToken.None) > 0;
    }
}