# Protobuf.System.Text.Json

[![Build](https://github.com/Havret/Protobuf.System.Text.Json/actions/workflows/build.yml/badge.svg)](https://github.com/Havret/Protobuf.System.Text.Json/actions/workflows/build.yml)
[![NuGet](https://img.shields.io/nuget/vpre/Protobuf.System.Text.Json.svg)](https://www.nuget.org/packages/Protobuf.System.Text.Json/)

This library provides support for [Google.Protobuf](https://www.nuget.org/packages/Google.Protobuf) types to [System.Text.Json](https://www.nuget.org/packages/System.Text.Json/).

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