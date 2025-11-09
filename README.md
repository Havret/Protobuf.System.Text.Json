# Protobuf.System.Text.Json

[![Build](https://github.com/Havret/Protobuf.System.Text.Json/actions/workflows/build.yml/badge.svg)](https://github.com/Havret/Protobuf.System.Text.Json/actions/workflows/build.yml)
[![NuGet](https://img.shields.io/nuget/vpre/Protobuf.System.Text.Json.svg)](https://www.nuget.org/packages/Protobuf.System.Text.Json/)

This library provides support for [Google.Protobuf](https://www.nuget.org/packages/Google.Protobuf) types to [System.Text.Json](https://www.nuget.org/packages/System.Text.Json/).

By default classes generated from *.proto files don't work well with System.Text.Json. You can properly serialize and deserialize only very simple contracts (only scalar value types work properly). When your schema contains some more advanced structures like repeated fields, maps, oneofs, or optional fields the results are far from acceptable. 

You may try to write a custom JsonConverter that will utilize JsonFormatter and JsonParser from Google.Protobuf package to do all the heavy lifting. Unfortunately, this solution falls short in terms of performance. This library provides high-performance and low-allocating types that serialize Google.Protobuf objects to JavaScript Object Notation (JSON) text and deserialize JSON text to Google.Protobuf objects.

## Quickstart

Add `Protobuf.System.Text.Json` NuGet package to your project using dotnet CLI:

```
dotnet add package Protobuf.System.Text.Json
```

To enable Protobuf support you need to register it on `JsonSerializerOptions`:

```csharp
var jsonSerializerOptions = new JsonSerializerOptions();
jsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
jsonSerializerOptions.AddProtobufSupport();
```

You need to pass the options each time you want to serialize:

```csharp
var msg = new SimpleMessage
{
    DoubleProperty = 2.5d
};
var payload = JsonSerializer.Serialize(msg, options: jsonSerializerOptions);
```

or deserialize `Google.Protobuf` objects:

```csharp
var deserialized = JsonSerializer.Deserialize<SimpleMessage>(payload, jsonSerializerOptions);
```

## Configuration
The library offers several configuration options to fine-tune protobuf serialization. You can modify the default settings using a delegate passed to the AddProtobufSupport method. The available options are described below:

### PropertyNamingSource
This option specifies the source for property names in JSON serialization. The default value is `PropertyNamingSource.Default`.

Available values:
- `PropertyNamingSource.Default`: Use the default `PropertyNamingPolicy` from `JsonSerializerOptions`.
- `PropertyNamingSource.ProtobufJsonName`: Use the JsonName from the protobuf contract. This is usually the lower-camel-cased form of the field name, but can be overridden using the `json_name` option in the .proto file.
- `PropertyNamingSource.ProtobufFieldName`: Use the original field name as defined in the .proto file (e.g., "double_property" instead of "doubleProperty").

Example:
```csharp
var jsonSerializerOptions = new JsonSerializerOptions();
jsonSerializerOptions.AddProtobufSupport(options => 
{
    options.PropertyNamingSource = PropertyNamingSource.ProtobufFieldName;
});
```

### UseProtobufJsonNames (Obsolete)
**Note:** This property is obsolete and will be removed in a future version. Use `PropertyNamingSource` instead.

This option defines how property names should be resolved for protobuf contracts. When set to `true`, the `PropertyNamingPolicy` will be ignored, and property names will be derived from the protobuf contract. The default value is `false`.

### TreatDurationAsTimeSpan
This option controls how `Google.Protobuf.WellKnownTypes.Duration` fields are handled. When set to true, `Google.Protobuf.WellKnownTypes.Duration` properties will be converted to `TimeSpan` before serialization and will be expected in the same format as `TimeSpan` during deserialization. The default value is `true`.

### TreatTimestampAsDateTime
This option controls how `Google.Protobuf.WellKnownTypes.Timestamp` fields are handled. When set to true, `Google.Protobuf.WellKnownTypes.Timestamp` properties will be converted to DateTime before serialization and will be expected in the same format as `DateTime` during deserialization. The default value is `true`.`

### UseStringProtoEnumValueNames
By setting`UseStringProtoEnumValueNames` to `true`, the library will serialize enum values using the names specified in the .proto file and expect the same format during deserialization. The default value is `false`.

By customizing these options, you can control how the library handles various aspects of protobuf serialization and deserialization, ensuring the JSON output is consistent with your requirements.

## Performance

``` ini
BenchmarkDotNet v0.13.12, macOS Sonoma 14.5 (23F79) [Darwin 23.5.0]
Apple M1 Pro, 1 CPU, 10 logical and 10 physical cores
.NET SDK 8.0.100
  [Host]     : .NET 8.0.0 (8.0.23.53103), Arm64 RyuJIT AdvSIMD
  DefaultJob : .NET 8.0.0 (8.0.23.53103), Arm64 RyuJIT AdvSIMD
```

### Simple message

| Method                                       | Mean     | Error     | StdDev    |
|--------------------------------------------- |---------:|----------:|----------:|
| SystemTextJsonProtobufSupport                | 1.562 us | 0.0080 us | 0.0074 us |
| NewtonsoftJsonWithProtobufParserAndFormatter | 8.705 us | 0.0432 us | 0.0383 us |

### Message with oneofs

| Method                                       | Mean     | Error     | StdDev    |
|--------------------------------------------- |---------:|----------:|----------:|
| SystemTextJsonProtobufSupport                | 1.832 us | 0.0132 us | 0.0117 us |
| NewtonsoftJsonWithProtobufParserAndFormatter | 9.962 us | 0.1668 us | 0.1561 us |

### Message with repeated fields

| Method                                       | Mean      | Error    | StdDev   |
|--------------------------------------------- |----------:|---------:|---------:|
| SystemTextJsonProtobufSupport                |  53.79 us | 0.262 us | 0.232 us |
| NewtonsoftJsonWithProtobufParserAndFormatter | 340.94 us | 1.360 us | 1.205 us |

## License

This project uses [MIT licence](https://github.com/Havret/Protobuf.System.Text.Json/blob/main/LICENSE). Long story short - you are more than welcome to use it anywhere you like, completely free of charge and without oppressive obligations.
