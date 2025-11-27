using ImageLinks_.Application.Features.StorgeHierarchy.Trees.DTO;
using ImageLinks_.Domain.Models;

namespace ImageLinks_.Application.Features.StorgeHierarchy.Trees.Mappers;

public static class TreeDtoMapper
{
    private static TreeDto ToDto(this Tree tree)
    {
        return new TreeDto(tree.TreeId.ToString(), tree.RnameEng, tree.RnameArb, tree.TreeLvl.ToString());
    }

    public static List<TreeDto> ToDtos(this IEnumerable<Tree> entities)
    {
        return [.. entities.Select(e => e.ToDto())];
    }
}
