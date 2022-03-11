using Google.Protobuf.Reflection;

namespace System.Text.Json.Protobuf;

internal class FieldInfo
{
    public IFieldAccessor Accessor { get; init; }
    public InternalConverter? Converter { get; set; }
    public bool IsRepeated { get; init; }
    public Type FieldType { get; init; }
    public string JsonName { get; set; }
}