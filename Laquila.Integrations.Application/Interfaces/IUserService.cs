using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Laquila.Integrations.Application.DTO.Users;
using Laquila.Integrations.Application.DTO.Users.Request;
using Laquila.Integrations.Domain.Filters;

namespace Laquila.Integrations.Application.Interfaces
{
    public interface IUserService
    {
        Task<UserResponseDTO> CreateUserAsync(UserDTO dto);
        Task<(List<UserResponseDTO> Data, int DataCount)> GetUsers(int page, int pageSize, string orderBy, bool ascending, UserFilters? filters);
        Task<UserResponseDTO> GetUserById(Guid id);
        Task<UserResponseDTO> UpdateUser(Guid id, UserDTO dto);
    }
}