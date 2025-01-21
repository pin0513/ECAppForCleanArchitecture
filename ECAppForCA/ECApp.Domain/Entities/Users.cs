
namespace ECApp.Domain.Entities;

public partial class Users
{
    public Guid Id { get; set; }

    public string Name { get; set; }

    public DateTimeOffset? DeletedTime { get; set; }

    /// <summary>
    /// 建立者
    /// </summary>
    public string Creator { get; set; }

    /// <summary>
    /// 建立時間
    /// </summary>
    public DateTimeOffset CreatedTime { get; set; }

    /// <summary>
    /// 最後更新者
    /// </summary>
    public string? LatestUpdater { get; set; }

    /// <summary>
    /// 最新更新時間
    /// </summary>
    public DateTimeOffset? LatestUpdatedTime { get; set; }

}
