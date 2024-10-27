using System;
using Newtonsoft.Json;

namespace playFusionX.Data.Models;

public class SpotifyTokenReponse
{
    [JsonProperty("access_token")]
    public string? AccessToken { get; set; }

    [JsonProperty("refresh_token")]
    public string? RefreshToken { get; set; }

    [JsonProperty("expires_in")]
    public int ExpiresIn { get; set; }
}
