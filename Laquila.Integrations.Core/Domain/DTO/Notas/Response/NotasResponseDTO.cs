using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Laquila.Integrations.Core.Domain.DTO.Notas.Shared;

namespace Laquila.Integrations.Core.Domain.DTO.Notas.Response
{
    public class NotasResponseDTO
    {
        [JsonPropertyName("cpf_cnpj_empresa")]
        public required string CpfCnpjEmpresa { get; set; }

        [JsonPropertyName("cd_empresa")]
        public int CdEmpresa { get; set; }

        [JsonPropertyName("id_notaemitida")]
        public long IdNotaEmitida { get; set; }

        [JsonPropertyName("nr_nota")]
        public long NrNota { get; set; }

        [JsonPropertyName("cd_chave_acesso_nota")]
        public required string CdChaveAcessoNota { get; set; }

        [JsonPropertyName("dh_emissao_nota")]
        public DateTime DhEmissaoNota { get; set; }

        [JsonPropertyName("cd_cliente")]
        public long CdCliente { get; set; }

        [JsonPropertyName("cpf_cnpj_cliente")]
        public required string CpfCnpjCliente { get; set; }

        [JsonPropertyName("razao_cliente")]
        public required string RazaoCliente { get; set; }

        [JsonPropertyName("fantasia_cliente")]
        public required string FantasiaCliente { get; set; }

        [JsonPropertyName("vl_mercadoria_nota")]
        public decimal VlMercadoriaNota { get; set; }

        [JsonPropertyName("vl_total_nota")]
        public decimal VlTotalNota { get; set; }

        [JsonPropertyName("cd_transportador_nota")]
        public long? CdTransportador { get; set; }
        
        [JsonPropertyName("cnpj_transportadora")]
        public string? CnpjTransportadora { get; set; }

        [JsonPropertyName("razao_transportadora")]
        public string? RazaoTransportadora { get; set; }

        [JsonPropertyName("cd_redespacho_nota")]
        public long? CdRedespacho { get; set; }

        [JsonPropertyName("cnpj_redespacho")]
        public string? CnpjRedespacho { get; set; }
        
        [JsonPropertyName("razao_redespacho")]
        public string? RazaoRedespacho { get; set; }

        [JsonPropertyName("at_situacao_nota")]
        public int AtSituacaoNota { get; set; }

        [JsonPropertyName("situacao_nota")]
        public required string SituacaoNota { get; set; }
        [JsonPropertyName("itens")]
        public IEnumerable<ItensNotaDTO> Itens { get; set; } = new List<ItensNotaDTO>();
    }
}