using ImageLinks_.Application.Common.Models;
using ImageLinks_.Application.Features.Users.DTO;
using ImageLinks_.Application.Features.Users.IRepository;
using ImageLinks_.Application.Features.Users.Mappers;
using ImageLinks_.Application.Features.Users.Services.Interface;
using ImageLinks_.Domain.Results;

namespace ImageLinks_.Application.Features.Users.Services.Implementation
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly ISysSettingDetailRepository _sysSettingDetailRepository;

        public UserService(IUserRepository userRepository, ISysSettingDetailRepository sysSettingDetailRepository)
        {
            _userRepository = userRepository;
            _sysSettingDetailRepository = sysSettingDetailRepository;
        }
        public async Task<Result<List<UserDto>>> GetAllUsersEF(CancellationToken ct)
        {
            var users = await _userRepository.GetAll(ct);
            return users.ToDtos();
        }

        public async Task<Result<List<UserDto>>> GetAllUsersDP(CancellationToken ct)
        {
            var x = await _sysSettingDetailRepository.SelectAsync(new(), ct);
            var mx = await _sysSettingDetailRepository.GetAll(ct, x => x.SysSettValue == "1");
            var users = await _userRepository.GetAllUsers(ct);
            return users.ToDtos();
        }

        public async Task<Result<PaginatedList<UserDto>>> GetAllUsersEfFilter(PageRequest pageRequest, CancellationToken ct)
        {
            var users = await _userRepository.GetUsersFilter(pageRequest, ct);
            var usersDTO = users.ToDtos();

            return new PaginatedList<UserDto>
            {
                Items = usersDTO,
                PageNumber = pageRequest.PageNumber,
                PageSize = pageRequest.PageSize,
                TotalCount = usersDTO.Count,
                TotalPages = (int)Math.Ceiling(usersDTO.Count / (double)pageRequest.PageSize)
            };
        }
    }
}
