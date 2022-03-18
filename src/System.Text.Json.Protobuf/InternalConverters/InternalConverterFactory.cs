namespace System.Text.Json.Protobuf.InternalConverters;

internal class InternalConverterFactory
{
    public static InternalConverter Create(Type fieldType, bool isRepeated)
    {
        if (isRepeated)
        {
            var converterType = typeof(RepeatedFieldConverter<>).MakeGenericType(fieldType);
            var internalConverter = (InternalConverter) Activator.CreateInstance(converterType)!;
            return internalConverter;
        }
        else
        {
            var converterType = typeof(FieldConverter<>).MakeGenericType(fieldType);
            var internalConverter = (InternalConverter) Activator.CreateInstance(converterType)!;
            return internalConverter;
        }
    }
}