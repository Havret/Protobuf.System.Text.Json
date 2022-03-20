namespace Protobuf.System.Text.Json;

public class JsonProtobufSerializerOptions
{
    /// <summary>
    /// Defines how property names should be resolved for protobuf contracts.
    /// When set to true PropertyNamingPolicy will be ignored and property name will be derived from protobuf contract.
    /// This is usually the lower-camel-cased form of the field name, but can be overridden using the <c>json_name</c>
    /// option in the .proto file.
    /// </summary>
    public bool UseProtobufJsonNames { get; set; }
}