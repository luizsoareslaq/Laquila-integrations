using System.ComponentModel.DataAnnotations.Schema;
using Laquila.Integrations.Core.Domain.Enums;

namespace Laquila.Integrations.Core.Domain.Models
{
    public class VMWMS_BuscarCadastrosNaoIntegrados
    {
        public required string MaName { get; set; }
        public required string MaCnpj { get; set; }
        public required string MaZip { get; set; }
        public required string MaAddress { get; set; }
        public required string MaCity { get; set; }
        public required string MaState { get; set; }
        public string? MaTel { get; set; }
        public string? MaFax { get; set; }
        public required MandatorsType MaType { get; set; }
        public required int MaCode { get; set; }
        public string? MaDistrict { get; set; }
    }
}