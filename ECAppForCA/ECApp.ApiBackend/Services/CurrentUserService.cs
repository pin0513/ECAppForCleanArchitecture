using System.Security.Claims;
using ECApp.Application.Interfaces;

namespace ECApp.Backend.Services;

public class CurrentUserService(IHttpContextAccessor httpContextAccessor, IServiceProvider serviceProvider)
    : ICurrentUserService
{
    private string? _action = string.Empty;

    public string? Action
    {
        get => httpContextAccessor.HttpContext?.Request?.Path.Value ?? _action;
        set =>
            //有特別要設定可設定
            _action = value;
    }

    public Guid UserId
    {
        get
        {
            var userId = httpContextAccessor.HttpContext?.User?.FindFirstValue("UserId");
            Guid.TryParse(userId, out Guid guidUserId);
            return guidUserId;
        }
    }

    public string UserAccount
    {
        get
        {
            var userAccount = httpContextAccessor.HttpContext?.User?.FindFirstValue("UserAccount");
            return userAccount;
        }
    }

    public string? UserName
    {
        get
        {
            var userName = httpContextAccessor.HttpContext?.User?.FindFirstValue("UserName");
            return userName;
        }
    }
}