using Protobuf.System.Text.Json;

// ReSharper disable once CheckNamespace
namespace System.Text.Json;

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
        options.Converters.Add(new TimestampConverter());
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