using Serilog.Core;
using Serilog.Events;
using System.Collections.Generic;

/// <summary>
/// Implements a destructuring policy for read-only dictionaries.
/// </summary>
public class IReadOnlyDictionaryDestructuringPolicy : IDestructuringPolicy
{
    /// <summary>
    /// Attempts to destructure the given value if it is a read-only dictionary.
    /// </summary>
    /// <param name="value">The value to destructure.</param>
    /// <param name="propertyValueFactory">The factory to create property values.</param>
    /// <param name="result">The destructured result if the operation is successful.</param>
    /// <returns>True if the value was successfully destructured; otherwise, false.</returns>
    public bool TryDestructure(object value, ILogEventPropertyValueFactory propertyValueFactory, out LogEventPropertyValue result)
    {
        if (value is IReadOnlyDictionary<object, object> readOnlyDictionary)
        {
            var dictionary = new Dictionary<object, LogEventPropertyValue>();

            foreach (var kvp in readOnlyDictionary)
            {
                dictionary[kvp.Key] = propertyValueFactory.CreatePropertyValue(kvp.Value, true);
            }

            var properties = new List<LogEventProperty>();
            foreach (var kvp in dictionary)
            {
                var key = kvp.Key?.ToString() ?? string.Empty; // Ensure key is not null
                properties.Add(new LogEventProperty(key, kvp.Value));
            }
            result = new StructureValue(properties);
            return true;
        }

        result = new ScalarValue(null);
        return false;
    }
}