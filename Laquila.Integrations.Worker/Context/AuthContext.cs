
namespace Laquila.Integrations.Worker.Context
{
    public class AuthContext
    {
        private readonly object _lock = new();
        private string? _token;
        private DateTime _expiresAt;
        private string? _company_cnpj;

        public void SetToken(string token, DateTime expiresAt, string? company_cnpj)
        {
            lock (_lock)
            {
                _token = token;
                _expiresAt = expiresAt;
                _company_cnpj = company_cnpj;
            }
        }

        public (string? Token, DateTime ExpiresAt, string? company_cnpj) GetToken()
        {
            lock (_lock)
            {
                return (_token, _expiresAt,_company_cnpj);
            }
        }

        public bool IsValid()
        {
            lock (_lock)
            {
                return !string.IsNullOrEmpty(_token) && DateTime.Now < _expiresAt;
            }
        }

        public void Clear()
        {
            lock (_lock)
            {
                _token = null;
                _expiresAt = DateTime.MinValue;
            }
        }
    }
}