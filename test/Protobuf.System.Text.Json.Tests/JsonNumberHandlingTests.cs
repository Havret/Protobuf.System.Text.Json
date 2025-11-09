using System;
using System.Text.Json;
using System.Text.Json.Protobuf.Tests;
using System.Text.Json.Serialization;
using Protobuf.System.Text.Json.Tests.Utils;
using Xunit;

namespace Protobuf.System.Text.Json.Tests;

public class JsonNumberHandlingTests
{
    [Fact]
    public void Should_deserialize_numbers_as_strings_when_NumberHandling_set_to_AllowReadingFromString()
    {
        // Arrange
        var json =
        """
        {
          "doubleProperty" : "0.12",
          "floatProperty" : "7.89",
          "int32Property" : "123",
          "int64Property" : "456",
          "uint32Property" : "789",
          "uint64Property" : "101112",
          "sint32Property" : "1314",
          "sint64Property" : "151617",
          "fixed32Property" : "1819",
          "fixed64Property" : "202122",
          "sfixed32Property" : "2324",
          "sfixed64Property" : "252627"
        }
        """;
        
        var options = TestHelper.CreateJsonSerializerOptions();
        options.NumberHandling = JsonNumberHandling.AllowReadingFromString;

        // Act
        var deserialized = JsonSerializer.Deserialize<SimpleMessage>(json, options);

        // Assert
        Assert.NotNull(deserialized);
        Assert.Equal(0.12d, deserialized.DoubleProperty);
        Assert.Equal(7.89f, deserialized.FloatProperty);
        Assert.Equal(123, deserialized.Int32Property);
        Assert.Equal(456L, deserialized.Int64Property);
        Assert.Equal(789u, deserialized.Uint32Property);
        Assert.Equal(101112ul, deserialized.Uint64Property);
        Assert.Equal(1314, deserialized.Sint32Property);
        Assert.Equal(151617L, deserialized.Sint64Property);
        Assert.Equal(1819u, deserialized.Fixed32Property);
        Assert.Equal(202122ul, deserialized.Fixed64Property);
        Assert.Equal(2324, deserialized.Sfixed32Property);
        Assert.Equal(252627L, deserialized.Sfixed64Property);
    }
    
    [Fact]
    public void Should_fail_deserializing_numbers_as_strings_when_NumberHandling_not_set_to_AllowReadingFromString()
    {
        // Arrange
        var json =
        """
        {
          "doubleProperty" : "0.12",
          "floatProperty" : "7.89",
          "int32Property" : "123",
          "int64Property" : "456",
          "uint32Property" : "789",
          "uint64Property" : "101112",
          "sint32Property" : "1314",
          "sint64Property" : "151617",
          "fixed32Property" : "1819",
          "fixed64Property" : "202122",
          "sfixed32Property" : "2324",
          "sfixed64Property" : "252627"
        }
        """;

        var options = TestHelper.CreateJsonSerializerOptions();
        // Explicitly disable reading numbers from strings
        options.NumberHandling = JsonNumberHandling.Strict;

        // Act & Assert
        Assert.Throws<JsonException>(() => JsonSerializer.Deserialize<SimpleMessage>(json, options));
    }
    
    [Fact]
    public void Should_serialize_numbers_as_strings_when_NumberHandling_set_to_WriteAsString()
    {
        // Arrange
        var message = new SimpleMessage
        {
            DoubleProperty = 0.12d,
            FloatProperty = 7.89f,
            Int32Property = 123,
            Int64Property = 456L,
            Uint32Property = 789u,
            Uint64Property = 101112u, // Note: this is actually uint32 in proto
            Sint32Property = 1314,
            Sint64Property = 151617L,
            Fixed32Property = 1819u,
            Fixed64Property = 202122ul,
            Sfixed32Property = 2324,
            Sfixed64Property = 252627L
        };
        
        var options = TestHelper.CreateJsonSerializerOptions();
        options.NumberHandling = JsonNumberHandling.WriteAsString;

        // Act
        var json = JsonSerializer.Serialize(message, options);

        // Assert
        Assert.Contains("\"doubleProperty\":\"0.12\"", json);
        Assert.Contains("\"floatProperty\":\"7.89\"", json);
        Assert.Contains("\"int32Property\":\"123\"", json);
        Assert.Contains("\"int64Property\":\"456\"", json);
        Assert.Contains("\"uint32Property\":\"789\"", json);
        Assert.Contains("\"uint64Property\":\"101112\"", json);
        Assert.Contains("\"sint32Property\":\"1314\"", json);
        Assert.Contains("\"sint64Property\":\"151617\"", json);
        Assert.Contains("\"fixed32Property\":\"1819\"", json);
        Assert.Contains("\"fixed64Property\":\"202122\"", json);
        Assert.Contains("\"sfixed32Property\":\"2324\"", json);
        Assert.Contains("\"sfixed64Property\":\"252627\"", json);
    }
    
    [Fact]
    public void Should_not_serialize_numbers_as_strings_when_NumberHandling_set_to_Strict()
    {
        // Arrange
        var message = new SimpleMessage
        {
            DoubleProperty = 0.12d,
            FloatProperty = 7.89f,
            Int32Property = 123
        };
        
        var options = TestHelper.CreateJsonSerializerOptions();
        options.NumberHandling = JsonNumberHandling.Strict;

        // Act
        var json = JsonSerializer.Serialize(message, options);

        // Assert - numbers should be JSON numbers, not strings
        Assert.Contains("\"doubleProperty\":0.12", json);
        Assert.Contains("\"floatProperty\":7.89", json);
        Assert.Contains("\"int32Property\":123", json);
    }
    
    [Fact]
    public void Should_deserialize_named_floating_point_literals_when_AllowNamedFloatingPointLiterals_flag_set()
    {
        // Arrange
        var json =
        """
        {
          "doubleProperty": "NaN",
          "floatProperty": "Infinity"
        }
        """;
        
        var options = TestHelper.CreateJsonSerializerOptions();
        options.NumberHandling = JsonNumberHandling.AllowReadingFromString | JsonNumberHandling.AllowNamedFloatingPointLiterals;

        // Act
        var deserialized = JsonSerializer.Deserialize<SimpleMessage>(json, options);

        // Assert
        Assert.NotNull(deserialized);
        Assert.True(double.IsNaN(deserialized.DoubleProperty));
        Assert.True(float.IsPositiveInfinity(deserialized.FloatProperty));
    }
    
    [Fact]
    public void Should_deserialize_negative_infinity_when_AllowNamedFloatingPointLiterals_flag_set()
    {
        // Arrange
        var json =
        """
        {
          "doubleProperty": "-Infinity",
          "floatProperty": "-Infinity"
        }
        """;
        
        var options = TestHelper.CreateJsonSerializerOptions();
        options.NumberHandling = JsonNumberHandling.AllowReadingFromString | JsonNumberHandling.AllowNamedFloatingPointLiterals;

        // Act
        var deserialized = JsonSerializer.Deserialize<SimpleMessage>(json, options);

        // Assert
        Assert.NotNull(deserialized);
        Assert.True(double.IsNegativeInfinity(deserialized.DoubleProperty));
        Assert.True(float.IsNegativeInfinity(deserialized.FloatProperty));
    }
    
    [Fact]
    public void Should_fail_deserializing_named_floating_point_literals_when_flag_not_set()
    {
        // Arrange
        var json =
        """
        {
          "doubleProperty": "NaN"
        }
        """;
        
        var options = TestHelper.CreateJsonSerializerOptions();
        // Only AllowReadingFromString, but not AllowNamedFloatingPointLiterals
        options.NumberHandling = JsonNumberHandling.AllowReadingFromString;

        // Act & Assert
        Assert.Throws<JsonException>(() => JsonSerializer.Deserialize<SimpleMessage>(json, options));
    }
    
    [Fact]
    public void Should_round_trip_with_WriteAsString_and_AllowReadingFromString()
    {
        // Arrange
        var message = new SimpleMessage
        {
            DoubleProperty = 123.456d,
            FloatProperty = 78.9f,
            Int32Property = 999
        };
        
        var options = TestHelper.CreateJsonSerializerOptions();
        options.NumberHandling = JsonNumberHandling.WriteAsString | JsonNumberHandling.AllowReadingFromString;

        // Act
        var json = JsonSerializer.Serialize(message, options);
        var deserialized = JsonSerializer.Deserialize<SimpleMessage>(json, options);

        // Assert
        Assert.NotNull(deserialized);
        Assert.Equal(message.DoubleProperty, deserialized.DoubleProperty);
        Assert.Equal(message.FloatProperty, deserialized.FloatProperty);
        Assert.Equal(message.Int32Property, deserialized.Int32Property);
    }
}