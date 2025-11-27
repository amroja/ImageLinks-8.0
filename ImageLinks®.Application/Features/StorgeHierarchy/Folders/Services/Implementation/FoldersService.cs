using ImageLinks_.Application.Common.Enums;
using ImageLinks_.Application.Common.Helpers;
using ImageLinks_.Application.Features.Groups.DTO;
using ImageLinks_.Application.Features.Groups.Services.Interface;
using ImageLinks_.Application.Features.StorgeHierarchy.Folders.DTO;
using ImageLinks_.Application.Features.StorgeHierarchy.Folders.IRepository;
using ImageLinks_.Application.Features.StorgeHierarchy.Folders.Mappers;
using ImageLinks_.Application.Features.StorgeHierarchy.Folders.Requests;
using ImageLinks_.Application.Features.StorgeHierarchy.Folders.Services.Interface;
using ImageLinks_.Application.Features.StorgeHierarchy.Trees.IRepository;
using ImageLinks_.Domain.Models;
using ImageLinks_.Domain.Results;

namespace ImageLinks_.Application.Features.StorgeHierarchy.Folders.Services.Implementation
{
    public class FoldersService : IFoldersService
    {
        private readonly ITreeRepository _treeRepository;
        private readonly IFoldersRepository _foldersRepository;
        private readonly IGroupMbrService _groupMbrService;

        public FoldersService(IFoldersRepository foldersRepository, ITreeRepository treeRepository, IGroupMbrService groupMbrService)
        {
            _treeRepository = treeRepository;
            _foldersRepository = foldersRepository;
            _groupMbrService = groupMbrService;
        }

        public async Task<Result<List<FoldersDto>>> SelectAsync(FoldersRequest filter, CancellationToken ct = default)
        {
            List<FoldersModel>? list = await _foldersRepository.SelectAsync(filter, ct);
            return list.ToDtos();
        }

        public async Task<Result<List<FoldersDto>>> GetFoldersByParent(FoldersRequest filter, CancellationToken ct = default)
        {
            List<FoldersModel> folders = new List<FoldersModel>();

            List<Tree> treeResult = await _treeRepository.SelectAsync(new Tree { TreeId = Convert.ToByte(filter.TreeId) }, ct);

            if (treeResult == null || !treeResult.Any())
                return folders.ToDtos();

            Tree tree = treeResult.First();
            bool isSysAdmin = filter.UserId == GeneralEnums.USERS.SYSADMIN.ToId();

            if (isSysAdmin)
            {
                folders = await _foldersRepository.SelectAsync(new FoldersRequest { FoldStat = "1" }, ct);
                return folders.ToDtos();
            }

            filter.GroupIds = await _groupMbrService.GetGroupsByUser(new GroupMbrDto { UserId = filter.UserId }, ct);
            filter.GroupIds = filter.GroupIds?.Where(id => id != "0").ToList();

            bool hasGroups = filter.GroupIds != null && filter.GroupIds.Any();
            bool isLevelOne = tree.TreeLvl == (int)GeneralEnums.TREE_LEVEL.LEVEL_ONE;
            bool isLevelTwo = tree.TreeLvl == (int)GeneralEnums.TREE_LEVEL.LEVEL_TWO;

            if (isLevelOne)
            {
                folders = hasGroups
                    ? await _foldersRepository.SelectActiveFoldersForGroupsLevelOne(filter, ct)
                    : await _foldersRepository.SelectActiveFoldersForUserLevelOne(filter, ct);
            }
            else if (isLevelTwo)
            {
                folders = hasGroups
                    ? await _foldersRepository.SelectActiveFoldersForGroupsLevelTwo(filter, ct)
                    : await _foldersRepository.SelectActiveFoldersForUserLevelTwo(filter, ct);
            }

            return folders.ToDtos();
        }
    }
}
