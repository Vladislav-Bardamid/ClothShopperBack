﻿using System.Text.Json.Serialization;

namespace ClothShopperBack.DAL.Common.VkApiModels;

public class VkAlbum
{
    public int Id { get; set; }
    [JsonPropertyName("privacy_comment")]
    public int PrivacyComment { get; set; }
}