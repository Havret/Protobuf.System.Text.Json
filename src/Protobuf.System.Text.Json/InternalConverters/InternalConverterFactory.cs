namespace Protobuf.System.Text.Json.InternalConverters;

internal class InternalConverterFactory
{
    public static InternalConverter Create(FieldInfo fieldInfo)
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
        else
        {
            var converterType = typeof(FieldConverter<>).MakeGenericType(fieldInfo.FieldType);
            var internalConverter = (InternalConverter) Activator.CreateInstance(converterType)!;
            return internalConverter;
        }
    }
}