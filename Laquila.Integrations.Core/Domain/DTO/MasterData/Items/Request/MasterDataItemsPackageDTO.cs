using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Laquila.Integrations.Core.Domain.DTO.MasterData.Items
{
    public class MasterDataItemsPackageDTO
    {
        public MasterDataItemsPackageDTO(List<ItemsDetailsDTO> itemAttributes)
        {
            ItemAttributes = itemAttributes;
        }
        
        [JsonPropertyName("items_attributes")]
        public List<ItemsDetailsDTO> ItemAttributes { get; set; }
    }

    public class ItemsDetailsDTO
    {
        public ItemsDetailsDTO(){}

        [JsonPropertyName("at_id")]
        public required string AtId { get; set; }

        [JsonPropertyName("at_desc")]
        public required string AtDesc { get; set; }

        [JsonPropertyName("at_type")]
        public required string AtType { get; set; }
        
        [JsonPropertyName("atb_box_id")]
        public int AtbBoxId { get; set; } 

        [JsonPropertyName("atb_gross_weight")]
        public decimal AtbGrossWeight { get; set; } 

        [JsonPropertyName("atb_net_weight")]
        public decimal AtbNetWeight { get; set; } 

        [JsonPropertyName("atb_fl_standard_package")]
        public string AtbFlStandardPackage { get; set; } 

        [JsonPropertyName("atb_height")]
        public decimal AtbHeight { get; set; } 

        [JsonPropertyName("atb_width")]
        public decimal AtbWidth { get; set; } 

        [JsonPropertyName("atb_length")]
        public decimal AtbLength { get; set; } 

        [JsonPropertyName("atb_m3")]
        public decimal AtbM3 { get; set; }

        [JsonPropertyName("atb_units_per_pallet")]
        public int? AtbUnitsPerPallet { get; set; } 

        [JsonPropertyName("atb_units_per_layer")]
        public int? AtbUnitsPerLayer { get; set; } 

        [JsonPropertyName("atb_max_stacking")]
        public int? AtbMaxStacking { get; set; }

        [JsonPropertyName("atb_volume_factor")]
        public decimal? AtbVolumeFactor { get; set; } 

        [JsonPropertyName("atb_qty_per_package")]
        public int AtbQtyPerPackage { get; set; } 

        [JsonPropertyName("atb_ean_type")]
        public int AtbEanType { get; set; } 

        [JsonPropertyName("atb_ean_barcode")]
        public required string AtbEanBarcode { get; set; } 

    }
}