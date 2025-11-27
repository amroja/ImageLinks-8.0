using ImageLinks_.Application.Common.Enums;
using ImageLinks_.Application.Common.Helpers;
using ImageLinks_.Application.Features.Groups.DTO;
using ImageLinks_.Application.Features.Groups.Services.Interface;
using ImageLinks_.Application.Features.StorgeHierarchy.DocumentClass.DTO;
using ImageLinks_.Application.Features.StorgeHierarchy.DocumentClass.IRepository;
using ImageLinks_.Application.Features.StorgeHierarchy.DocumentClass.Mappers;
using ImageLinks_.Application.Features.StorgeHierarchy.DocumentClass.Requests;
using ImageLinks_.Application.Features.StorgeHierarchy.DocumentClass.Services.Interface;
using ImageLinks_.Application.Features.StorgeHierarchy.Trees.IRepository;
using ImageLinks_.Domain.Models;
using ImageLinks_.Domain.Results;

namespace ImageLinks_.Application.Features.StorgeHierarchy.DocumentClass.Services.Implementation;

public class DocService : IDocService
{
    private readonly ITreeRepository _treeRepository;
    private readonly IDocRepository _docRepository;
    private readonly IGroupMbrService _groupMbrService;
    public DocService(ITreeRepository treeRepository,IDocRepository repository,IGroupMbrService groupMbrService)
    {
        _treeRepository = treeRepository;
        _docRepository = repository;
        _groupMbrService = groupMbrService;
    }

    public async Task<Result<List<DocDto>>> GetDocs(DocumentsRequest filter, CancellationToken ct = default)
    {
        var list = await _docRepository.SelectAsync(filter, ct);
        return list.ToDtos();
    }

    public async Task<Result<List<DocDto>>> GetDocumentsByParent(DocumentsRequest filter, CancellationToken ct = default)
    {
        List<Doc> documents = new List<Doc>();

        List<Tree> treeResult = await _treeRepository.SelectAsync(new Tree { TreeId = Convert.ToByte(filter.TreeId) }, ct);

        if (treeResult == null || !treeResult.Any())
            return documents.ToDtos();

        Tree tree = treeResult.First();
        bool isSysAdmin = filter.UserId == GeneralEnums.USERS.SYSADMIN.ToId();

        if (isSysAdmin)
        {
            documents = await _docRepository.SelectAsync(new DocumentsRequest { DocStat = "1" }, ct);
            return documents.ToDtos();
        }

        filter.GroupIds = await _groupMbrService.GetGroupsByUser(new GroupMbrDto { UserId = filter.UserId }, ct);
        filter.GroupIds = filter.GroupIds?.Where(id => id != "0").ToList();

        bool hasGroups = filter.GroupIds != null && filter.GroupIds.Any();
        bool isLevelOne = tree.TreeLvl == (int)GeneralEnums.TREE_LEVEL.LEVEL_ONE;
        bool isLevelFour = tree.TreeLvl == (int)GeneralEnums.TREE_LEVEL.LEVEL_FOUR;

        if (isLevelOne)
        {
            documents = hasGroups
                ? await _docRepository.SelectActiveDocumentsForGroupsLevelOne(filter, ct)
                : await _docRepository.SelectActiveDocumentsForUserLevelOne(filter, ct);
        }
        else if (isLevelFour)
        {
            documents = hasGroups
                ? await _docRepository.SelectActiveDocumentsForGroupsLevelFour(filter, ct)
                : await _docRepository.SelectActiveDocumentsForUserLevelFour(filter, ct);
        }

        return documents.ToDtos();
    }
}
