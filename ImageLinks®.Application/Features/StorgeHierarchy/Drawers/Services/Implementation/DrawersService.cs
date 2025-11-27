using ImageLinks_.Application.Common.Enums;
using ImageLinks_.Application.Common.Helpers;
using ImageLinks_.Application.Features.Groups.DTO;
using ImageLinks_.Application.Features.Groups.Services.Interface;
using ImageLinks_.Application.Features.StorgeHierarchy.Drawers.DTO;
using ImageLinks_.Application.Features.StorgeHierarchy.Drawers.IRepository;
using ImageLinks_.Application.Features.StorgeHierarchy.Drawers.Mappers;
using ImageLinks_.Application.Features.StorgeHierarchy.Drawers.Requests;
using ImageLinks_.Application.Features.StorgeHierarchy.Drawers.Services.Interface;
using ImageLinks_.Application.Features.StorgeHierarchy.Trees.IRepository;
using ImageLinks_.Domain.Models;
using ImageLinks_.Domain.Results;

namespace ImageLinks_.Application.Features.StorgeHierarchy.Drawers.Services.Implementation;
public class DrawersService : IDrawersService
{
   
    private readonly IDrawersRepository _drawersRepository;
    private readonly IGroupMbrService _groupMbrService;
    private readonly ITreeRepository _treeRepository;

    public DrawersService(IDrawersRepository drawersRepository, IGroupMbrService groupMbrService, ITreeRepository treeRepository)
    {
        _drawersRepository = drawersRepository;
        _groupMbrService = groupMbrService;
        _treeRepository = treeRepository;
    }

    public async Task<Result<List<DrawersDto>>> SelectAsync(DrawersRequest filter, CancellationToken ct = default)
    {
        List<DrawersModel>? list = await _drawersRepository.SelectAsync(filter, ct);
        return list.ToDtos();
    }

    public async Task<Result<List<DrawersDto>>> GetDrawersByParent(DrawersRequest filter, CancellationToken ct = default)
    {
        List<DrawersModel> drawers = new List<DrawersModel>();

        List<Tree> treeResult = await _treeRepository.SelectAsync(new Tree { TreeId = Convert.ToByte(filter.TreeId) }, ct);

        if (treeResult == null || !treeResult.Any())
            return drawers.ToDtos();

        Tree tree = treeResult.First();
        bool isSysAdmin = filter.UserId == GeneralEnums.USERS.SYSADMIN.ToId();

        if (isSysAdmin)
        {
            drawers = await _drawersRepository.SelectAsync(new DrawersRequest { DrwStat = "1" }, ct);
            return drawers.ToDtos();
        }

        filter.GroupIds = await _groupMbrService.GetGroupsByUser(new GroupMbrDto { UserId = filter.UserId }, ct);
        filter.GroupIds = filter.GroupIds?.Where(id => id != "0").ToList();

        bool hasGroups = filter.GroupIds != null && filter.GroupIds.Any();
        bool isLevelOne = tree.TreeLvl == (int)GeneralEnums.TREE_LEVEL.LEVEL_ONE;
        bool isLevelTwo = tree.TreeLvl == (int)GeneralEnums.TREE_LEVEL.LEVEL_TWO;

        if (isLevelOne)
        {
            drawers = hasGroups
                ? await _drawersRepository.SelectActiveDrawersForGroupsLevelOne(filter, ct)
                : await _drawersRepository.SelectActiveDrawersForUserLevelOne(filter, ct);
        }
        else if (isLevelTwo)
        {
            drawers = hasGroups
                ? await _drawersRepository.SelectActiveDrawersForGroupsLevelTwo(filter, ct)
                : await _drawersRepository.SelectActiveDrawersForUserLevelTwo(filter, ct);
        }

        return drawers.ToDtos();
    }
}
