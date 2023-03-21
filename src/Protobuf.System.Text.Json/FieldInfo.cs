using Google.Protobuf.Reflection;
using Protobuf.System.Text.Json.InternalConverters;

namespace Protobuf.System.Text.Json;

internal class FieldInfo
{
    public IFieldAccessor Accessor { get; set; } = null!;
    public InternalConverter Converter { get; set; } = null!;
    public bool IsRepeated { get; set; }
    public Type FieldType { get; set; } = null!;
    public string JsonName { get; set; } = null!;
    public bool IsOneOf { get; set; }
    public bool IsMap { get; set; }
    public EnumDescriptor? EnumType { get; set; }
}