using System.Text.Json.Protobuf;

// ReSharper disable once CheckNamespace
namespace System.Text.Json;

public static class JsonSerializerOptionsExtensions
{
    public static void AddProtobufSupport(this JsonSerializerOptions options)
    {
        options.Converters.Add(new TimestampConverter());
        options.Converters.Add(new ProtobufJsonConverterFactory());
    }
}