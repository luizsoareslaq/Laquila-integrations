using System.Text.Json.Serialization;
using Laquila.Integrations.Core.Domain.DTO.Romaneio.Shared;

namespace Laquila.Integrations.Core.Domain.DTO.Romaneio.Response
{
    public class RomaneioResponseDTO
    {
        [JsonPropertyName("cnpj_empresa")]
        public required string CnpjEmpresa { get; set; }
        [JsonPropertyName("cd_empresa")]
        public int CdEmpresa { get; set; }
        [JsonPropertyName("id_romaneio")]
        public long IdRomaneio { get; set; }
        [JsonPropertyName("dh_emissao_romaneio")]
        public DateTime DhEmissaoRomaneio { get; set; }
        [JsonPropertyName("cd_cliente")]
        public long CdCliente { get; set; }
        [JsonPropertyName("cpf_cnpj_cliente")]
        public required string CpfCnpjCliente { get; set; }
        [JsonPropertyName("razao_cliente")]
        public required string RazaoCliente { get; set; }
        [JsonPropertyName("fantasia_cliente")]
        public required string FantasiaCliente { get; set; }
        [JsonPropertyName("cd_transportador")]
        public long? CdTransportador { get; set; }
        [JsonPropertyName("cnpj_transportadora")]
        public string? CnpjTransportadora { get; set; }
        [JsonPropertyName("razao_transportadora")]
        public string? RazaoTransportadora { get; set; }
        [JsonPropertyName("at_situacao_romaneio")]
        public int AtSituacaoRomaneio { get; set; }
        [JsonPropertyName("situacao_romaneio")]
        public required string SituacaoRomaneio { get; set; }
        [JsonPropertyName("itens")]
        public IEnumerable<ItensRomaneioDTO> Itens { get; set; } = new List<ItensRomaneioDTO>();
    }
}