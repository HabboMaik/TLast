using System;
using System.Collections.Generic;

using Newtonsoft.Json;

namespace TLast.Models
{
    public class SelectedBadge
    {
        [JsonProperty("badgeIndex")]
        public int BadgeIndex { get; set; }

        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }
    }

    public class UserModel
    {
        [JsonProperty("uniqueId")]
        public string UniqueId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("figureString")]
        public string FigureString { get; set; }

        [JsonProperty("motto")]
        public string Motto { get; set; }

        [JsonProperty("memberSince")]
        public DateTime MemberSince { get; set; }

        [JsonProperty("profileVisible")]
        public bool ProfileVisible { get; set; }

        [JsonProperty("selectedBadges")]
        public List<SelectedBadge> SelectedBadges { get; set; }
    }
}