using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace QuanLiKhoHang.Utilities;

public static class EntitySchemaExporter
{
    public static string ExportToJson(string specFilePath)
    {
        var entities = ParseSpecification(specFilePath);
        var options = new JsonSerializerOptions
        {
            WriteIndented = true,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        };

        return JsonSerializer.Serialize(entities, options);
    }

    private static IReadOnlyCollection<EntitySchema> ParseSpecification(string specFilePath)
    {
        if (!File.Exists(specFilePath))
        {
            throw new FileNotFoundException("Specification file not found.", specFilePath);
        }

        var schemas = new List<EntitySchema>();
        EntitySchema? current = null;

        foreach (var rawLine in File.ReadLines(specFilePath))
        {
            var line = rawLine.Trim();

            if (string.IsNullOrWhiteSpace(line))
            {
                continue;
            }

            if (line.StartsWith("________________________________________", StringComparison.Ordinal))
            {
                if (current is not null)
                {
                    schemas.Add(current);
                    current = null;
                }

                continue;
            }

            if (line.StartsWith("●", StringComparison.Ordinal))
            {
                if (current is null)
                {
                    continue;
                }

                var content = line.TrimStart('●').Trim();
                var propertyName = ExtractPropertyName(content);

                if (string.IsNullOrEmpty(propertyName))
                {
                    continue;
                }

                if (content.Contains("— PK", StringComparison.OrdinalIgnoreCase))
                {
                    current.PrimaryKeys.Add(propertyName);
                }

                var fkIndex = content.IndexOf("FK →", StringComparison.OrdinalIgnoreCase);
                if (fkIndex >= 0)
                {
                    var reference = ExtractForeignKeyReference(content[(fkIndex + 4)..]);
                    if (!string.IsNullOrEmpty(reference))
                    {
                        current.ForeignKeys.Add(new ForeignKey(propertyName, reference));
                    }
                }

                continue;
            }

            if (IsEntityHeading(line))
            {
                if (current is not null)
                {
                    schemas.Add(current);
                }

                current = new EntitySchema(line);
            }
        }

        if (current is not null)
        {
            schemas.Add(current);
        }

        return schemas;
    }

    private static bool IsEntityHeading(string line)
    {
        if (line.StartsWith("Business notes", StringComparison.OrdinalIgnoreCase) ||
            line.StartsWith("Relationships", StringComparison.OrdinalIgnoreCase) ||
            line.StartsWith("Indexes", StringComparison.OrdinalIgnoreCase) ||
            line.StartsWith("Relationship", StringComparison.OrdinalIgnoreCase))
        {
            return false;
        }

        return !line.Contains(":", StringComparison.Ordinal) && !line.StartsWith("-", StringComparison.Ordinal);
    }

    private static string ExtractPropertyName(string content)
    {
        if (string.IsNullOrWhiteSpace(content))
        {
            return string.Empty;
        }

        var firstSpaceIndex = content.IndexOf(' ');
        if (firstSpaceIndex < 0)
        {
            return content.Trim();
        }

        return content[..firstSpaceIndex].Trim();
    }

    private static string ExtractForeignKeyReference(string text)
    {
        var cleaned = text.Trim();
        if (string.IsNullOrEmpty(cleaned))
        {
            return string.Empty;
        }

        var terminators = new[] { '.', ',', '—' };
        var terminatorIndex = cleaned.IndexOfAny(terminators);
        if (terminatorIndex >= 0)
        {
            cleaned = cleaned[..terminatorIndex];
        }

        return cleaned.Replace("NULL", string.Empty, StringComparison.OrdinalIgnoreCase).Trim();
    }

    public record EntitySchema(string Entity)
    {
        public List<string> PrimaryKeys { get; } = new();
        public List<ForeignKey> ForeignKeys { get; } = new();
    }

    public record ForeignKey(string Property, string Reference);
}
