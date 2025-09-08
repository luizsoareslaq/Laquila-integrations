using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Laquila.Integrations.Core.Domain.Filters
{
    public class LAQFilters
    {
        [JsonPropertyName("cd_empresa")]
        public int CdEmpresa { get; set; }
        [JsonPropertyName("cnpj_empresa")]
        public string? CnpjEmpresa { get; set; }
        [JsonPropertyName("cd_cliente")]
        public long? CdCliente { get; set; }
        [JsonPropertyName("cpf_cnpj_cliente")]
        public string? CpfCnpjCliente { get; set; }

        [JsonPropertyName("data_emissao_inicial")]
        public DateOnly DataEmissaoInicial { get; set; }

        [JsonPropertyName("data_emissao_final")]
        public DateOnly DataEmissaoFinal { get; set; }

        [JsonPropertyName("id_romaneio")]
        public int? IdRomaneio { get; set; }

        [JsonPropertyName("id_notaemitida")]
        public long? IdNotaEmitida { get; set; }

        [JsonPropertyName("nr_nota")]
        public long? NrNota { get; set; }

        [JsonPropertyName("cd_transportador")]
        public long? CdTransportador { get; set; }

        [JsonPropertyName("cd_redespacho")]
        public long? CdRedespacho { get; set; }
    }
}