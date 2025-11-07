using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Laquila.Integrations.Core.Domain.DTO.MasterData.Items
{
    public class MasterDataItemsPackageDTO
    {
        [JsonPropertyName("items_attributes")]
        public required List<ItemsDetailsDTO> ItemAttributes { get; set; }
    }

    public class ItemsDetailsDTO
    {
        [JsonPropertyName("at_id")]
        public required string AtId { get; set; }

        [JsonPropertyName("at_desc")]
        public required string AtDesc { get; set; }

        [JsonPropertyName("at_type")]
        public required string AtType { get; set; }
        
        [JsonPropertyName("atb_box_id")]
        public required int AtbBoxId { get; set; } 

        [JsonPropertyName("atb_gross_weight")]
        public required decimal AtbGrossWeight { get; set; } 

        [JsonPropertyName("atb_net_weight")]
        public required decimal AtbNetWeight { get; set; } 

        [JsonPropertyName("atb_fl_standard_package")]
        public required bool AtbFlStandardPackage { get; set; } 

        [JsonPropertyName("atb_height")]
        public required decimal AtbHeight { get; set; } 

        [JsonPropertyName("atb_width")]
        public required decimal AtbWidth { get; set; } 

        [JsonPropertyName("atb_length")]
        public required decimal AtbLength { get; set; } 

        [JsonPropertyName("atb_m3")]
        public required decimal AtbM3 { get; set; }

        [JsonPropertyName("atb_units_per_pallet")]
        public int? AtbUnitsPerPallet { get; set; } 

        [JsonPropertyName("atb_units_per_layer")]
        public int? AtbUnitsPerLayer { get; set; } 

        [JsonPropertyName("atb_max_stacking")]
        public int? AtbMaxStacking { get; set; }

        [JsonPropertyName("atb_volume_factor")]
        public required decimal AtbVolumeFactor { get; set; } 

        [JsonPropertyName("atb_qty_per_package")]
        public required int AtbQtyPerPackage { get; set; } 

        [JsonPropertyName("atb_ean_type")]
        public required int AtbEanType { get; set; } 

        [JsonPropertyName("atb_ean_barcode")]
        public required string AtbEanBarcode { get; set; } 

    }
}