
using System.ComponentModel.DataAnnotations.Schema;

namespace Laquila.Integrations.Core.Domain.Models
{
    public class VLAQConsultarNotas
    {
        [Column("cpf_cnpj_empresa")]
        public required string CpfCpnjEmpresa { get; set; }

        [Column("cd_empresa")]
        public int CdEmpresa { get; set; }

        [Column("id_notaemitida")]
        public long IdNotaEmitida { get; set; }

        [Column("nr_nota")]
        public long NrNota { get; set; }

        [Column("cd_chave_acesso_nota")]
        public required string CdChaveAcessoNota { get; set; }

        [Column("dia_emissao")]
        public int DiaEmissao { get; set; }

        [Column("mes_emissao")]
        public int MesEmissao { get; set; }

        [Column("ano_emissao")]
        public int AnoEmissao { get; set; }

        [Column("dh_emissao_nota")]
        public DateTime DhEmissaoNota { get; set; }

        [Column("cd_cliente")]
        public long CdCliente { get; set; }

        [Column("cpf_cnpj_cliente")]
        public required string CpfCnpjCliente { get; set; }

        [Column("razao_cliente")]
        public required string RazaoCliente { get; set; }

        [Column("fantasia_cliente")]
        public required string FantasiaCliente { get; set; }

        [Column("vl_mercadoria_nota")]
        public decimal VlMercadoriaNota { get; set; }

        [Column("vl_total_nota")]
        public decimal VlTotalNota { get; set; }

        [Column("cd_transportador_nota")]
        public long? CdTransportador { get; set; }

        [Column("razao_transportadora")]
        public string? RazaoTransportadora { get; set; }

        [Column("cd_redespacho_nota")]
        public long? CdRedespacho { get; set; }

        [Column("razao_redespacho")]
        public string? RazaoRedespacho { get; set; }

        [Column("at_situacao_nota")]
        public int AtSituacaoNota { get; set; }
        
        [Column("situacao_nota")]
        public required string SituacaoNota { get; set; }

        [Column("id_romaneio")]
        public int IdRomaneio { get; set; }

        [Column("dh_emissao_romaneio")]
        public DateOnly? DhEmissaoRomaneio { get; set; }

        [Column("at_situacao_romaneio")]
        public int? AtSituacaoRomaneio { get; set; }

        [Column("situacao_romaneio")]
        public string? SituacaoRomaneio { get; set; }
        
    }
}