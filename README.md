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
jsonSerializerOptions.PropertyNamingPolicy = new JsonLowerCaseNamingPolicy();
jsonSerializerOptions.AddProtobufSupport();
```

```csharp
private class JsonLowerCaseNamingPolicy : JsonNamingPolicy
{
  public override string ConvertName(string name)
  {
    return name.ToLower();
  }
}
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

## Performance

``` ini
BenchmarkDotNet=v0.13.1, OS=macOS Monterey 12.3 (21E230) [Darwin 21.4.0]
Apple M1 Pro, 1 CPU, 10 logical and 10 physical cores
.NET SDK=6.0.101
  [Host]     : .NET 6.0.1 (6.0.121.56705), Arm64 RyuJIT
  DefaultJob : .NET 6.0.1 (6.0.121.56705), Arm64 RyuJIT
```

### Simple message
|                                       Method |      Mean |     Error |    StdDev |
|--------------------------------------------- |----------:|----------:|----------:|
|                SystemTextJsonProtobufSupport |  2.572 μs | 0.0094 μs | 0.0074 μs |
| NewtonsoftJsonWithProtobufParserAndFormatter | 17.250 μs | 0.1492 μs | 0.1395 μs |


### Message with oneofs
|                                       Method |      Mean |     Error |    StdDev |
|--------------------------------------------- |----------:|----------:|----------:|
|                SystemTextJsonProtobufSupport |  3.115 μs | 0.0065 μs | 0.0057 μs |
| NewtonsoftJsonWithProtobufParserAndFormatter | 18.384 μs | 0.0403 μs | 0.0377 μs |


### Message with repeated fields
|                                       Method |     Mean |   Error |  StdDev |
|--------------------------------------------- |---------:|--------:|--------:|
|                SystemTextJsonProtobufSupport | 100.9 μs | 0.19 μs | 0.17 μs |
| NewtonsoftJsonWithProtobufParserAndFormatter | 735.1 μs | 1.09 μs | 1.02 μs |

## License

This project uses [MIT licence](https://github.com/Havret/Protobuf.System.Text.Json/blob/main/LICENSE). Long story short - you are more than welcome to use it anywhere you like, completely free of charge and without oppressive obligations.
