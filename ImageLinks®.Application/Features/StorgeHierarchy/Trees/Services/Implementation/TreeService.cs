using ImageLinks_.Application.Common.Enums;
using ImageLinks_.Application.Common.Helpers;
using ImageLinks_.Application.Features.Groups.DTO;
using ImageLinks_.Application.Features.Groups.Services.Interface;
using ImageLinks_.Application.Features.StorgeHierarchy.Trees.DTO;
using ImageLinks_.Application.Features.StorgeHierarchy.Trees.IRepository;
using ImageLinks_.Application.Features.StorgeHierarchy.Trees.Mappers;
using ImageLinks_.Application.Features.StorgeHierarchy.Trees.Requests;
using ImageLinks_.Application.Features.StorgeHierarchy.Trees.Services.Interface;
using ImageLinks_.Domain.Models;
using ImageLinks_.Domain.Results;

namespace ImageLinks_.Application.Features.StorgeHierarchy.Trees.Services.Implementation;
public class TreeService : ITreeService
{
    private readonly ITreeRepository _treeRepository;
    private readonly IGroupMbrService _groupMbrService;
    public TreeService(ITreeRepository treeRepository, IGroupMbrService groupMbrService)
    {
        _treeRepository = treeRepository;
        _groupMbrService = groupMbrService;
    }

    public async Task<Result<List<TreeDto>>> GetActiveTreesForUser(TreeRequest tree, CancellationToken ct = default)
    {
        List<Tree> trees = new List<Tree>();

        if (tree.UserId == GeneralEnums.USERS.SYSADMIN.ToId())
        {
            trees = await _treeRepository.SelectAsync(new Tree { TreeStat = 1 }, ct);
        }
        else
        {
            tree.GroupIds = await _groupMbrService.GetGroupsByUser(new GroupMbrDto { UserId = tree.UserId }, ct);
            tree.GroupIds = tree.GroupIds.Where(id => id != "0").ToList();

            if (tree.GroupIds != null && tree.GroupIds.Any())
            {
                trees = await _treeRepository.GetTreesByGroups(tree, ct);
            }
            else
            {
                trees = await _treeRepository.GetActiveTreesForUser(tree, ct);
            }
        }

        return trees.ToDtos();
    }
}