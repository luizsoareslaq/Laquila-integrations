using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Laquila.Integrations.Core.Domain.Enums;

namespace Laquila.Integrations.Core.Domain.DTO.MasterData.Mandators.Request
{
    public class MasterDataMandatorsDTO
    {
        [JsonPropertyName("mandators")]
        public required List<MandatorsAttributesDTO> mandators { get; set; }
    }

    public class MandatorsAttributesDTO
    {
        [JsonPropertyName("ma_name")]
        public required string MaName { get; set; } 

        [JsonPropertyName("ma_cnpj")]
        public required string MaCnpj { get; set; } 

        [JsonPropertyName("ma_zip")]
        public required string MaZip { get; set; }

        [JsonPropertyName("ma_address")]
        public required string MaAddress { get; set; } 

        [JsonPropertyName("ma_city")]
        public required string MaCity { get; set; } 

        [JsonPropertyName("ma_tel")]
        public string? MaTel { get; set; } 

        [JsonPropertyName("ma_fax")]
        public string? MaFax { get; set; } 

        [JsonPropertyName("ma_type")]
        public required MandatorsType MaType { get; set; } 

        [JsonPropertyName("ma_code")]
        public required long MaCode { get; set; }

        [JsonPropertyName("ma_district")]
        public string? MaDistrict { get; set; } 
    }
}