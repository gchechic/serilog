using Serilog;
using Serilog.Configuration;
using Serilog.Core;
using Serilog.Events;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Xunit;

namespace Serilog.Tests.Configuration
{
    public class LoggerDestructuringConfigurationTests
    {
        [Fact]
        public void AsReadOnlyDictionary_ShouldAddReadOnlyDictionaryType()
        {
            // Arrange
            var loggerConfiguration = new LoggerConfiguration();
            var addedTypes = new List<Type>();
            var destructuringConfiguration = new LoggerDestructuringConfiguration(
                loggerConfiguration,
                t => addedTypes.Add(t)
            );

            // Act
            destructuringConfiguration.AsReadOnlyDictionary<IReadOnlyDictionary<object, object>>();

            // Assert
            Assert.Contains(typeof(IReadOnlyDictionary<object, object>), addedTypes);
        }
    }

    public static class LoggerDestructuringConfigurationExtensions
    {
        public static LoggerDestructuringConfiguration AsReadOnlyDictionary<T>(this LoggerDestructuringConfiguration configuration)
        {
            configuration.AddDestructuringType(typeof(T));
            return configuration;
        }
    }

    public class LoggerDestructuringConfiguration
    {
        private readonly LoggerConfiguration _loggerConfiguration;
        private readonly Action<Type> _addDestructuringType;

        public LoggerDestructuringConfiguration(
            LoggerConfiguration loggerConfiguration,
            Action<Type> addDestructuringType)
        {
            _loggerConfiguration = loggerConfiguration;
            _addDestructuringType = addDestructuringType;
        }

        public void AddDestructuringType(Type type)
        {
            _addDestructuringType(type);
        }
    }
}
