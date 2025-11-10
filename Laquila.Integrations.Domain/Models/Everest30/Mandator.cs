using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Laquila.Integrations.Domain.Models.Everest30
{
    public class Mandator
    {
        [Column("MA_NAME")]
        public string MaName { get; set; } = null!;

        [Column("MA_CNPJ")]
        public string MaCnpj { get; set; } = null!;

        [Column("MA_ZIP")]
        public string MaZip { get; set; } = null!;

        [Column("MA_ADDRESS")]
        public string MaAddress { get; set; } = null!;

        [Column("MA_CITY")]
        public string MaCity { get; set; } = null!;

        [Column("MA_STATE")]
        public string MaState { get; set; } = null!;

        [Column("MA_TEL")]
        public string? MaTel { get; set; }

        [Column("MA_FAX")]
        public string? MaFax { get; set; }

        [Column("MA_STATUS")]
        public int MaStatus { get; set; }

        [Column("MA_STATUSOLD")]
        public int? MaStatusOld { get; set; } = null!;

        [Column("MA_CC")]
        public int MaCc { get; set; } = 1;

        [Column("MA_TYPE")]
        public int MaType { get; set; } 

        [Key]
        [Column("MA_CODE")]
        public int MaCode { get; set; }

        [Column("MA_DISTRICT")]
        public string? MaDistrict { get; set; } = null!;

        [Column("MA_REASON")]
        public string? MaReason { get; set; }

        [Column("MA_LAST_UPDATE")]
        public DateTime? MaLastUpdate { get; set; } = null!;

        [Column("MA_LAST_UPDATE_API")]
        public DateTime? MaLastUpdateApi { get; set; } = null!;


    }
}