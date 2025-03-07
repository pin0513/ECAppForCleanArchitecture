using System.Text.Json;
using System.Text.Json.Serialization;

namespace ECApp.Backend.Helpers;

public class JsonDateTimeOffsetConverter : JsonConverter<DateTimeOffset>
{
    /// <summary>
    /// System.Text.Json原本的使用行為，將Input字串轉為DateTimeOffset型別
    /// </summary>
    /// <param name="reader"></param>
    /// <param name="typeToConvert"></param>
    /// <param name="options"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public override DateTimeOffset Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return DateTimeOffset.Parse(reader.GetString() ?? throw new InvalidOperationException());
    }

    /// <summary>
    /// 使API輸出的DateTimeOffSet型別資料，統一轉為ISO 8601格式
    /// </summary>
    /// <param name="writer"></param>
    /// <param name="value"></param>
    /// <param name="options"></param>
    public override void Write(Utf8JsonWriter writer, DateTimeOffset value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString("yyyy-MM-ddTHH:mm:ss.fffZ"));
    }
}