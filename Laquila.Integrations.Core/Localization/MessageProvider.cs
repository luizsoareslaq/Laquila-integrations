using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Laquila.Integrations.Core.Localization
{
    public static class MessageProvider
    {
        public static string Get(string key, string? language)
        {
            var lang = language?.ToLower() ?? "en";

            return (key, lang) switch
            {
                //Not found messages
                ("OrderIdNotFound", "pt") => "Pedido {0} não foi encontrado para a empresa {1}.",
                ("OrderIdNotFound", "en") => "Order {0} was not found for company {1}.",

                ("OrderItemsNotFound", "pt") => "Itens do pedido {0} não foram encontrados para a empresa {1}.",
                ("OrderItemsNotFound", "en") => "Items for order {0} was not found for company {1}.",

                ("ItemOelIdNotFound", "pt") => "O item com oel_id {0} não foi encontrado na prenota {1}.",
                ("ItemOelIdNotFound", "en") => "Item with oel_id {0} not found in order {1}.",

                ("ItemOelIdNotFoundRequestBody", "pt") => "O item com oel_id {0} não foi encontrado no \"body\" da requisição para a prenota {1}.",
                ("ItemOelIdNotFoundRequestBody", "en") => "Item with oel_id {0} not found in request body for order {1}.",

                ("CompanyIdNotFound", "pt") => "Nenhuma empresa encontrada com este ID.",
                ("CompanyIdNotFound", "en") => "No company found with the given id.",

                ("CompaniesNotFound", "pt") => "Nenhuma empresa encontrada com estes filtros",
                ("CompaniesNotFound", "en") => "No Companies found with these filters.",

                ("UserCompanyNotFound", "pt") => "Empresa não vinculada para este usuário.",
                ("UserCompanyNotFound", "en") => "The specified company is not associated with this user.",

                ("NoCompanyAssigned", "pt") => "Nenhuma empresa vinculada a este usuário. Fale com o suporte.",
                ("NoCompanyAssigned", "en") => "No company is linked to this user. Please contact support.",

                ("MultipleCompaniesAssigned", "pt") => "Múltiplas empresas estão vinculadas neste usuário. Faça o login com uma empresa específica utilizando o parâmetro \"company\":\"<cnpj_da_empresa>\".",
                ("MultipleCompaniesAssigned", "en") => "Multiple companies are linked to this user. Log in with a specific company using the parameter \"company\":\"<company_cnpj>\".",

                ("UserIdNotFound", "pt") => "Nenhum usuário encontrado com este ID.",
                ("UserIdNotFound", "en") => "No user found with the given id.",

                ("UsersNotFound", "pt") => "Nenhum usuário encontrado com estes filtros.",
                ("UsersNotFound", "en") => "No Users found with these filters.",

                ("UsernameNotFound", "pt") => "Usuário {0} não encontrado.",
                ("UsernameNotFound", "en") => "Username {0} was not found.",

                ("IntegrationIDNotFound", "pt") => "Nenhuma integração encontrada com este ID.",
                ("IntegrationIDNotFound", "en") => "No integration found with the given id.",

                ("IntegrationsNotFound", "pt") => "Nenhuma integração encontrada com estes filtros.",
                ("IntegrationsNotFound", "en") => "No integrations found with these filters.",

                ("IntegrationsURLIDNotFound", "pt") => "Nenhuma URL de integração encontrada com este ID.",
                ("IntegrationsURLIDNotFound", "en") => "No integrations url found with the given id.",

                ("IntegrationsURLIDOrEndpointNotFound", "pt") => "Nenhuma URL de integração encontrada com este ID ou chave de endpoint.",
                ("IntegrationsURLIDOrEndpointNotFound", "en") => "No integrations url found with the given id or endpoint key",

                ("QueuesNotFound", "pt") => "Nenhum item pendente na fila foi encontrado com esses filtros.",
                ("QueuesNotFound", "en") => "No pending queue items were found with these filters.",

                ("QueueIDNotFound", "pt") => "Nenhum item pendente na fila foi encontrado com este ID.",
                ("QueueIDNotFound", "en") => "No pending queue item was found with this ID.",

                ("RolesDatabaseNotFound", "pt") => "Nenhuma permissão de usuário foi encontrada no banco de dados.",
                ("RolesDatabaseNotFound", "en") => "No roles were found in the database.",

                //Order messages
                ("OrderInvalidStatus", "pt") => "O pedido {0} não está em um status válido para atualização de datas.",
                ("OrderInvalidStatus", "en") => "The order {0} is not in a valid status for updating dates.",

                ("OrderQueued", "pt") => "A atualização de status do pedido {0} foi enfileirada com sucesso com id {1}.",
                ("OrderQueued", "en") => "The status update for order {0} was queued successfully with id {1}.",

                ("OrderSent", "pt") => "Pedido {0} enviado com sucesso para o sistema externo.",
                ("OrderSent", "en") => "Order {0} sent successfully to external system.",

                ("OrderSentError", "pt") => "Não foi possível enviar o pedido {0}: {1}.",
                ("OrderSentError", "en") => "Failed to send order {0}: {1}.",

                ("InvoiceSent", "pt") => "Nota {0} do pedido {1} enviada com sucesso ao sistema externo.",
                ("InvoiceSent", "en") => "Invoice {0} from order {1} successfully sent to the external system.",

                ("InvoiceSentError", "pt") => "Não foi possível enviar a nota fiscal {0} do pedido {1} : {2}.",
                ("InvoiceSentError", "en") => "Failed to sent Invoice {0} from order {1} : {2}.",

                ("MandatorsSent", "pt") => "Clientes integrados com sucesso ao sistema externo.",
                ("MandatorsSent", "en") => "Mandators successfully sent to the external system.",

                ("MandatorsSentError", "pt") => "Não foi possível enviar a atualização de clientes: {0}",
                ("MandatorsSentError", "en") => "Failed to sent mandators update: {0}",

                ("ItemsSent", "pt") => "Itens integrados com sucesso ao sistema externo.",
                ("ItemsSent", "en") => "Items successfully sent to the external system.",

                ("ItemsSentError", "pt") => "Não foi possível enviar a atualização de itens: {0}",
                ("ItemsSentError", "en") => "Failed to sent items update: {0}",

                ("ItemsProcessed", "pt") => "Itens processados com sucesso no banco de dados.",
                ("ItemsProcessed", "en") => "Items successfully processed in the database.",
                
                //Other messages
                ("ItemsUpdateSuccess", "pt") => "A atualização dos status dos itens da prenota {0} foi enfileirada com sucesso.",
                ("ItemsUpdateSuccess", "en") => "Items update from order {0} were queued successfully.",

                ("UsernameAlreadyExists", "pt") => "Este nome de usuário ja existe.",
                ("UsernameAlreadyExists", "en") => "Username already exists.",

                ("PasswordConfirmPasswordNotMatch", "pt") => "A senha e a confirmação de senha não combinam.",
                ("PasswordConfirmPasswordNotMatch", "en") => "Password and Confirm Password do not match.",

                ("PasswordRequired", "pt") => "A senha é obrigatória.",
                ("PasswordRequired", "en") => "Password is required when updating the password.",

                ("ConfirmPasswordRequired", "pt") => "A confirmação de senha é obrigatória.",
                ("ConfirmPasswordRequired", "en") => "Confirm Password is required when updating the password.",

                ("NoRolesAssigned", "pt") => "Pelo menos uma função deve ser atribuída para este usuário.",
                ("NoRolesAssigned", "en") => "At least one role must be assigned to the user.",

                ("AtLeastOneDateRequired", "pt") => "Pelo menos uma data deve ser fornecida.",
                ("AtLeastOneDateRequired", "en") => "At least one date must be provided.",

                ("AtLeastOneItemRequired", "pt") => "Pelo menos um item deve ser fornecido.",
                ("AtLeastOneItemRequired", "en") => "At least one item must be provided.",

                ("InvalidToken", "pt") => "Token inválido.",
                ("InvalidToken", "en") => "Invalid token.",

                ("GeneratingTokenError", "pt") => "Erro ao gerar o token de acesso.",
                ("GeneratingTokenError", "en") => "Error while generating access token.",

                ("InvalidLogin", "pt") => "Tentativa de login inválida.",
                ("InvalidLogin", "en") => "Invalid login attemp.",

                ("InvalidRefreshToken", "pt") => "Refresh token inválido ou expirado.",
                ("InvalidRefreshToken", "en") => "Refresh Token is invalid or expired.",

                ("CompanyDocumentAlreadyExists", "pt") => "Já existe uma empresa com este CNPJ cadastrado.",
                ("CompanyDocumentAlreadyExists", "en") => "A company with the same document already exists.",


                _ => key
            };
        }

    }
}