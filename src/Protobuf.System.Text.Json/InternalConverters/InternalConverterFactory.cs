using System.Text.Json;
using Google.Protobuf;

namespace Protobuf.System.Text.Json.InternalConverters;

internal class InternalConverterFactory
{
    public static InternalConverter Create(FieldInfo fieldInfo, JsonSerializerOptions jsonSerializerOptions)
    {
        if (fieldInfo.IsMap)
        {
            var converterType = typeof(MapConverter<,>).MakeGenericType(fieldInfo.FieldType.GenericTypeArguments);
            var internalConverter = (InternalConverter) Activator.CreateInstance(converterType)!;
            return internalConverter;
        }
        else if (fieldInfo.IsRepeated)
        {
            var converterType = typeof(RepeatedFieldConverter<>).MakeGenericType(fieldInfo.FieldType);
            var internalConverter = (InternalConverter) Activator.CreateInstance(converterType)!;
            return internalConverter;
        }
        else if (fieldInfo.EnumType != null)
        {
            var internalConverter = (InternalConverter) Activator.CreateInstance(typeof(ProtoEnumConverter), args: new object[] { fieldInfo.EnumType, jsonSerializerOptions.Encoder! })!;
            return internalConverter;
        }
        else if (fieldInfo.FieldType == typeof(ByteString))
        {
            var internalConverter = new ByteStringConverter();
            return internalConverter;
        }
        else
        {
            var converterType = typeof(FieldConverter<>).MakeGenericType(fieldInfo.FieldType);
            var internalConverter = (InternalConverter) Activator.CreateInstance(converterType)!;
            return internalConverter;
        }
    }
}