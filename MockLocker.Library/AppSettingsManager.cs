using Microsoft.Extensions.Configuration;
using MockLocker.Library.Models;
using System;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using System.Text.Json;

namespace MockLocker.Library
{
    public class AppSettingsManager
    {
        private IConfigurationRoot Configuration;
        public List<string> GetJsonListFromSettings(string key)
        {
            Configuration = new ConfigurationBuilder()
                                   .SetBasePath(Directory.GetCurrentDirectory())
                                   .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                                   .Build();
            return Configuration.GetSection(key).Get<List<string>>();
        }

        public void UpdateAppSettings(string key, object newValue)
        {

            var json = File.ReadAllText("appsettings.json");
            using (JsonDocument document = JsonDocument.Parse(json))
            {
                JsonElement root = document.RootElement;
                using (var stream = new MemoryStream())
                {
                    using (var writer = new Utf8JsonWriter(stream, new JsonWriterOptions { Indented = true }))
                    {
                        writer.WriteStartObject();
                        foreach (var property in root.EnumerateObject())
                        {
                            if (property.Name.Equals("LockNoStatus") && key.StartsWith("LockNoStatus."))
                            {
                                writer.WritePropertyName("LockNoStatus");
                                writer.WriteStartObject();
                                string subKey = key.Substring("LockNoStatus.".Length);

                                foreach (var subProperty in property.Value.EnumerateObject())
                                {
                                    if (subProperty.Name.Equals(subKey))
                                    {
                                        writer.WritePropertyName(subProperty.Name);
                                        writer.WriteNumberValue(Convert.ToInt32(newValue));
                                    }
                                    else
                                    {
                                        subProperty.WriteTo(writer);
                                    }
                                }
                                writer.WriteEndObject();
                            }
                            else
                            {
                                property.WriteTo(writer);
                            }
                        }
                        writer.WriteEndObject();
                    }
                    File.WriteAllBytes("appsettings.json", stream.ToArray());
                }
            }
        }
        private void WriteUpdatedJson(Utf8JsonWriter writer, JsonElement element, string[] keyParts, string newValue, int currentKeyIndex = 0)
        {
            if (element.ValueKind == JsonValueKind.Object)
            {
                writer.WriteStartObject();
                foreach (var property in element.EnumerateObject())
                {
                    writer.WritePropertyName(property.Name);
                    if (property.NameEquals(keyParts[currentKeyIndex]))
                    {
                        if (currentKeyIndex == keyParts.Length - 1)
                        {
                            writer.WriteStringValue(newValue);
                        }
                        else
                        {
                            WriteUpdatedJson(writer, property.Value, keyParts, newValue, currentKeyIndex + 1);
                        }
                    }
                    else
                    {
                        WriteUpdatedJson(writer, property.Value, keyParts, newValue, currentKeyIndex);
                    }
                }
                writer.WriteEndObject();
            }
            else if (element.ValueKind == JsonValueKind.Array)
            {
                writer.WriteStartArray();
                foreach (var item in element.EnumerateArray())
                {
                    WriteUpdatedJson(writer, item, keyParts, newValue, currentKeyIndex);
                }
                writer.WriteEndArray();
            }
            else
            {
                element.WriteTo(writer);
            }
        }
    }
}
