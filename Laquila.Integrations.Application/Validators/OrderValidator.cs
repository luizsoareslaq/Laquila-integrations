using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Laquila.Integrations.Core.Context;
using Laquila.Integrations.Core.Domain.DTO.Romaneio.Request;
using Laquila.Integrations.Core.Localization;
using Laquila.Integrations.Domain.Enums.Everest30;
using Laquila.Integrations.Domain.Models.Everest30;

namespace Laquila.Integrations.Application.Validators
{
    public class OrderValidator
    {
        public static bool CanUpdateStatus(int loStatus)
        {
            return loStatus == (int)LoStatusEnum.Separacao
                || loStatus == (int)LoStatusEnum.Conferencia;
        }

        public static bool CanUpdateConfStatus(int loStatus)
        {
            return loStatus == (int)LoStatusEnum.Faturamento;
        }

        public static (bool canUpdate, string message) CanUpdateDates(LoadOut entity, PrenotaDatesDTO dto)
        {
            switch (entity.LoStatus)
            {
                case (int)LoStatusEnum.Separacao:
                    {
                        if (entity.LoDtIniPicking is null
                            && dto.LoDtIniPicking is null
                            && (dto.LoDtEndPicking.HasValue
                             || dto.LoDtIniConf.HasValue
                             || dto.LoDtEndConf.HasValue))
                        {
                            return (false, string.Format(
                                MessageProvider.Get("OrderInvalidStatusIniPicking", UserContext.Language),
                                entity.LoOe));
                        }

                        if (entity.LoDtEndPicking is null
                            && dto.LoDtEndPicking is null
                            && (dto.LoDtIniConf.HasValue || dto.LoDtEndConf.HasValue))
                        {
                            return (false, string.Format(
                                MessageProvider.Get("OrderInvalidStatusEndPicking", UserContext.Language),
                                entity.LoOe));
                        }

                        break;
                    }

                case (int)LoStatusEnum.Conferencia:
                    {
                        if (entity.LoDtIniConf is null
                            && dto.LoDtIniConf is null
                            && dto.LoDtEndConf.HasValue)
                        {
                            return (false, string.Format(
                                MessageProvider.Get("OrderInvalidStatusIniConf", UserContext.Language),
                                entity.LoOe));
                        }

                        break;
                    }
            }

            return (true, string.Empty);
        }

    }
}