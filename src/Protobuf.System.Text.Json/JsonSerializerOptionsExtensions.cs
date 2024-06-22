using Protobuf.System.Text.Json;
using Protobuf.System.Text.Json.WellKnownTypesConverters;

// ReSharper disable once CheckNamespace
namespace System.Text.Json;

/// <summary>
/// Extension methods for <see cref="JsonSerializerOptions"/>.
/// </summary>
public static class JsonSerializerOptionsExtensions
{
    /// <summary>
    /// Adds support for Protobuf contracts to JsonSerializer.
    /// </summary>
    /// <param name="options">The <see cref="T:System.Text.Json.JsonSerializerOptions" />.</param>
    /// <param name="configure">An <see cref="T:System.Action" /> to configure the <see cref="T:System.Text.Json.Protobuf.JsonProtobufSerializerOptions" />.</param>
    public static void AddProtobufSupport(this JsonSerializerOptions options, Action<JsonProtobufSerializerOptions> configure)
    {
        var jsonProtobufSerializerOptions = new JsonProtobufSerializerOptions();
        configure.Invoke(jsonProtobufSerializerOptions);
        if (jsonProtobufSerializerOptions.TreatDurationAsTimeSpan)
        {
            options.Converters.Add(new DurationConverter());
        }
        if (jsonProtobufSerializerOptions.TreatTimestampAsDateTime)
        {
            options.Converters.Add(new TimestampConverter());
        }
        options.Converters.Add(new ByteStringConverter());
        options.Converters.Add(new ProtobufJsonConverterFactory(jsonProtobufSerializerOptions));
    }
    
    /// <summary>
    /// Adds support for Protobuf contracts to JsonSerializer.
    /// </summary>
    /// <param name="options">The <see cref="T:System.Text.Json.JsonSerializerOptions" />.</param>
    public static void AddProtobufSupport(this JsonSerializerOptions options)
    {
        options.AddProtobufSupport(_ => {});
    }
}