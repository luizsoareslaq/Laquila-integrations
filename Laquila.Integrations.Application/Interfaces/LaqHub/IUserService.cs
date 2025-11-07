using Laquila.Integrations.Application.DTO.Users;
using Laquila.Integrations.Application.DTO.Users.Request;
using Laquila.Integrations.Domain.Filters;

namespace Laquila.Integrations.Application.Interfaces.LaqHub
{
    public interface IUserService
    {
        Task<UserResponseDTO> CreateUserAsync(UserDTO dto);
        Task<(List<UserResponseDTO> Data, int DataCount)> GetUsers(int page, int pageSize, string orderBy, bool ascending, UserFilters? filters);
        Task<UserResponseDTO> GetUserById(Guid id);
        Task<UserResponseDTO> UpdateUser(Guid id, UserDTO dto);
    }
}