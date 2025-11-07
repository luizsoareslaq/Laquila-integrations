using System.ComponentModel.DataAnnotations.Schema;

namespace Laquila.Integrations.Core.Domain.Models
{
    public class VMWMS_BuscarItensNaoIntegrados
    {
        public required string AtId { get; set; }
        public required string AtDesc { get; set; }
        public required string AtType { get; set; }
        public required int AtbBoxId { get; set; } 
        public required decimal AtbGrossWeight { get; set; } 
        public required decimal AtbNetWeight { get; set; } 
        public required string AtbFlStandardPackage { get; set; } 
        public required decimal AtbHeight { get; set; } 
        public required decimal AtbWidth { get; set; } 
        public required decimal AtbLength { get; set; } 
        public required decimal AtbM3 { get; set; }
        public int? AtbUnitsPerPallet { get; set; } 
        public int? AtbUnitsPerLayer { get; set; } 
        public int? AtbMaxStacking { get; set; }
        public decimal? AtbVolumeFactor { get; set; } 
        public required int AtbQtyPerPackage { get; set; } 
        public required int AtbEanType { get; set; } 
        public required string AtbEanBarcode { get; set; } 
    }
}