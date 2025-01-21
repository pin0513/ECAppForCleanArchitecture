namespace ECApp.Domain.Entities;

public partial class SchemaVersion
{
    /// <summary>
    /// 識別碼
    /// </summary>
    public Guid Id { get; set; }

    public string? PartitionKey { get; set; }

    /// <summary>
    /// 物件類別鍵值
    /// </summary>
    public string RawKey { get; set; } = null!;

    /// <summary>
    /// 物件名稱
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// 資料容器
    /// </summary>
    public string? Data { get; set; }

    public DateTimeOffset? DeletedTime { get; set; }

    /// <summary>
    /// 建立者
    /// </summary>
    public long Creator { get; set; }

    /// <summary>
    /// 建立時間
    /// </summary>
    public DateTimeOffset CreatedTime { get; set; }

    /// <summary>
    /// 最後更新者
    /// </summary>
    public long? LatestUpdater { get; set; }

    /// <summary>
    /// 最新更新時間
    /// </summary>
    public DateTimeOffset? LatestUpdatedTime { get; set; }
}
