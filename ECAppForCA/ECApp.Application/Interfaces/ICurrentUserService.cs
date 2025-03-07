namespace ECApp.Application.Interfaces;

public interface ICurrentUserService
{
    public Guid UserId { get;  }
    public string UserName { get; }
    public string UserAccount { get; }
}