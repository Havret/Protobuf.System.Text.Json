namespace Protobuf.System.Text.Json;

/// <summary>
/// Specifies the source for property names in JSON serialization.
/// </summary>
public enum PropertyNamingSource
{
    /// <summary>
    /// Use the default PropertyNamingPolicy from JsonSerializerOptions.
    /// </summary>
    Default = 0,
    
    /// <summary>
    /// Use the JsonName from the protobuf contract.
    /// This is usually the lower-camel-cased form of the field name,
    /// but can be overridden using the json_name option in the .proto file.
    /// </summary>
    ProtobufJsonName = 1,
    
    /// <summary>
    /// Use the original field name as defined in the .proto file.
    /// For example, "double_property" instead of "doubleProperty".
    /// </summary>
    ProtobufFieldName = 2
}
