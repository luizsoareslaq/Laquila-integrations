using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace Laquila.Integrations.Core.Domain.Filters
{
    public class LAQFilters
    {
        [Required]
        [FromQuery(Name = "page")]
        public int Page { get; set; } = 1;
        
        [Required]
        [FromQuery(Name = "page-size")]
        public int PageSize { get; set; } = 20;

        [Required]
        [FromQuery(Name = "cd_empresa")]
        public int CdEmpresa { get; set; }
        [Required]
        [FromQuery(Name = "data_emissao_inicial")]
        public DateOnly DataEmissaoInicial { get; set; }

        [Required]
        [FromQuery(Name = "data_emissao_final")]
        public DateOnly DataEmissaoFinal { get; set; }

        [FromQuery(Name = "cnpj_empresa")]
        public string? CnpjEmpresa { get; set; }

        [FromQuery(Name = "cd_cliente")]
        public long? CdCliente { get; set; }

        [FromQuery(Name = "cpf_cnpj_cliente")]
        public string? CpfCnpjCliente { get; set; }

        [FromQuery(Name = "id_romaneio")]
        public int? IdRomaneio { get; set; }

        [FromQuery(Name = "id_notaemitida")]
        public long? IdNotaEmitida { get; set; }

        [FromQuery(Name = "nr_nota")]
        public long? NrNota { get; set; }

        [FromQuery(Name = "cd_transportador")]
        public long? CdTransportador { get; set; }

        [FromQuery(Name = "cd_redespacho")]
        public long? CdRedespacho { get; set; }
    }
}