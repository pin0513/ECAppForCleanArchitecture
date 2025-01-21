namespace ECApp.Application.Interfaces;

public interface ICurrentUserService
{
    public Guid UserId { get; set; }
    public string UserAccount { get; set; }
}