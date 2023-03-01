using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace WebAPI.Models
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum RgpClass
    {
        Knight=1,
        Mage= 2,
        Cleric=3

    }
}