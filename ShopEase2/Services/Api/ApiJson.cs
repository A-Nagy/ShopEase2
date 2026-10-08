using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ShopEase2.Services.Api
{
    public static class ApiJson
    {
        // NEW (Day 4.B1): Handles web-style camelCase JSON.
        public static JsonSerializerOptions Options { get; } = new(JsonSerializerDefaults.Web);

        public static async Task<string> ErrorAsync( HttpResponseMessage response, CancellationToken ct = default)
        {
            try
            {
                await using Stream body = await response.Content.ReadAsStreamAsync(ct);

                using JsonDocument json = await JsonDocument.ParseAsync(body,cancellationToken: ct);

                if (json.RootElement.ValueKind ==
                        JsonValueKind.Object &&
                    json.RootElement.TryGetProperty(
                        "message",
                        out JsonElement message) &&
                    message.ValueKind ==
                        JsonValueKind.String)
                {
                    string? value =
                        message.GetString();

                    if (!string.IsNullOrWhiteSpace(value))
                    {
                        return value;
                    }
                }
            }
            catch (JsonException)
            {
                // A non-JSON error still has an HTTP status.
            }
            catch (IOException)
            {
                // Fall back to the status.
            }

            return
                $"Request failed ({(int)response.StatusCode} {response.ReasonPhrase}).";
        }
    }
}
