using Google.Protobuf.Reflection;
using Protobuf.System.Text.Json.InternalConverters;

namespace Protobuf.System.Text.Json;

internal class FieldInfo
{
    public IFieldAccessor Accessor { get; init; }
    public InternalConverter? Converter { get; set; }
    public bool IsRepeated { get; init; }
    public Type FieldType { get; init; }
    public string JsonName { get; set; }
    public bool IsOneOf { get; set; }
    public bool IsMap { get; set; }
}