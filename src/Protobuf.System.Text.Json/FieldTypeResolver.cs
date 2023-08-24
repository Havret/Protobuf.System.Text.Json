using Google.Protobuf.Reflection;
using Google.Protobuf.WellKnownTypes;
using Type = System.Type;

namespace Protobuf.System.Text.Json;

internal static class FieldTypeResolver
{
    public static Type ResolverFieldType(FieldDescriptor fieldDescriptor, Dictionary<string, Type> propertyTypeLookup)
    {
        switch (fieldDescriptor.FieldType)
        {
            case FieldType.Double:
                return typeof(double);
            case FieldType.Float:
                return typeof(float);
            case FieldType.Int32:
            case FieldType.SInt32:
            case FieldType.SFixed32:
                return typeof(int);
            case FieldType.Int64:
            case FieldType.SInt64:
            case FieldType.SFixed64:
                return typeof(long);
            case FieldType.UInt32:
            case FieldType.Fixed32:
                return typeof(uint);
            case FieldType.UInt64:
            case FieldType.Fixed64:
                return typeof(ulong);
            case FieldType.Bool:
                return typeof(bool);
            case FieldType.String:
                return typeof(string);
            case FieldType.Bytes:
                return typeof(Google.Protobuf.ByteString);
            case FieldType.Message when fieldDescriptor.MessageType.ClrType is { } clrType:
                if (clrType == typeof(DoubleValue))
                    return typeof(double?);
                if (clrType == typeof(FloatValue))
                    return typeof(float?);
                if (clrType == typeof(Int64Value))
                    return typeof(long?);
                if (clrType == typeof(UInt64Value))
                    return typeof(ulong?);
                if (clrType == typeof(Int32Value))
                    return typeof(int?);
                if (clrType == typeof(UInt32Value))
                    return typeof(uint?);
                if (clrType == typeof(BoolValue))
                    return typeof(bool?);
                if (clrType == typeof(StringValue))
                    return typeof(string);
                return clrType;
            case FieldType.Enum when fieldDescriptor.IsRepeated:
                var fieldType = propertyTypeLookup[fieldDescriptor.PropertyName];
                return fieldType.GenericTypeArguments[0];
            case FieldType.Enum:
            case FieldType.Message:
                return propertyTypeLookup[fieldDescriptor.PropertyName];
            default:
                throw new ArgumentOutOfRangeException(nameof(fieldDescriptor),
                    $"FieldType: '{fieldDescriptor.FieldType}' is not supported.");
        }
    }
}