namespace Protobuf.System.Text.Json;

/// <summary>
/// Provides a set of options to configure the behavior of the JSON-Protobuf serialization and deserialization process.
/// </summary>
public class JsonProtobufSerializerOptions
{
    /// <summary>
    /// Defines how property names should be resolved for protobuf contracts.
    /// When set to true PropertyNamingPolicy will be ignored and property name will be derived from protobuf contract.
    /// This is usually the lower-camel-cased form of the field name, but can be overridden using the <c>json_name</c>
    /// option in the .proto file.
    /// The default value is false.
    /// </summary>
    public bool UseProtobufJsonNames { get; set; }

    /// <summary>
    /// Controls how <see cref="Google.Protobuf.WellKnownTypes.Duration"/> fields are handled.
    /// When set to true, <see cref="Google.Protobuf.WellKnownTypes.Duration"/> properties will
    /// be converted to <see cref="TimeSpan"/> before serialization and will be expected in the
    /// same format as <see cref="TimeSpan"/> (-dddddddd.hh:mm:ss.fffffff) during deserialization.
    /// The default value is true.
    /// </summary>
    public bool TreatDurationAsTimeSpan { get; set; } = true;


    /// <summary>
    /// Controls how <see cref="Google.Protobuf.WellKnownTypes.Timestamp"/> fields are handled.
    /// When set to true, <see cref="Google.Protobuf.WellKnownTypes.Timestamp"/> properties will
    /// be converted to <see cref="DateTime"/> before serialization and will be expected in the
    /// same format as <see cref="DateTime"/> during deserialization.
    /// The default value is true.
    /// </summary>
    public bool TreatTimestampAsDateTime { get; set; } = true;

    /// <summary>
    /// Controls how enums defined as part of the protobuf contract are handled.
    /// When set to true enum values will be serialized as strings based on the naming  
    /// specified in the .proto file. The same format will be expected during deserialization.
    /// The default value is false.
    /// </summary>
    /// 
    public bool UseStringProtoEnumValueNames { get; set; }
}