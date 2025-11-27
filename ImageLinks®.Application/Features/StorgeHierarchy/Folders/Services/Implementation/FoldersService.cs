using ImageLinks_.Application.Features.StorgeHierarchy.Folders.DTO;
using ImageLinks_.Application.Features.StorgeHierarchy.Folders.IRepository;
using ImageLinks_.Application.Features.StorgeHierarchy.Folders.Requests;
using ImageLinks_.Application.Features.StorgeHierarchy.Folders.Services.Interface;
using ImageLinks_.Application.Features.StorgeHierarchy.Folders.Mappers;
using ImageLinks_.Domain.Results;
using ImageLinks_.Domain.Models;

namespace ImageLinks_.Application.Features.StorgeHierarchy.Folders.Services.Implementation
{
    public class FoldersService : IFoldersService
    {
        private readonly IFoldersRepository _foldersRepository;

        public FoldersService(IFoldersRepository foldersRepository)
        {
            _foldersRepository = foldersRepository;
        }

        public async Task<Result<List<FoldersDto>>> SelectAsync(FoldersRequest filter, CancellationToken ct = default)
        {
            List<FoldersModel>? list = await _foldersRepository.SelectAsync(filter, ct);
            return list.ToDtos();
        }
    }
}
