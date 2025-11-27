using ImageLinks_.Application.Common.Enums;
using ImageLinks_.Application.Common.Helpers;
using ImageLinks_.Application.Features.Groups.DTO;
using ImageLinks_.Application.Features.Groups.Services.Interface;
using ImageLinks_.Application.Features.StorgeHierarchy.Cabinets.DTO;
using ImageLinks_.Application.Features.StorgeHierarchy.Cabinets.IRepository;
using ImageLinks_.Application.Features.StorgeHierarchy.Cabinets.Mappers;
using ImageLinks_.Application.Features.StorgeHierarchy.Cabinets.Requests;
using ImageLinks_.Application.Features.StorgeHierarchy.Cabinets.Services.Interface;
using ImageLinks_.Domain.Models;
using ImageLinks_.Domain.Results;

namespace ImageLinks_.Application.Features.StorgeHierarchy.Cabinets.Services.Implementation;

public class CabinetsService : ICabinetsService
{
    private readonly ICabinetsRepository _cabinetsRepository;
    private readonly IGroupMbrService _groupMbrService;
    public CabinetsService(ICabinetsRepository cabinetsRepository, IGroupMbrService groupMbrService)
    {
        _cabinetsRepository = cabinetsRepository;
        _groupMbrService = groupMbrService;
    }

    public async Task<Result<List<CabinetsDto>>> GetCabinets(CabinetsRequest filter, CancellationToken ct = default)
    {
        List<CabinetsModel>? list = await _cabinetsRepository.SelectAsync(filter, ct);
        return list.ToDtos();
    }

    public async Task<Result<List<CabinetsDto>>> GetCabinetsByParent(CabinetsRequest filter, CancellationToken ct = default)
    {
        List<CabinetsModel> cabinets = new List<CabinetsModel>();

        if (filter.UserId == GeneralEnums.USERS.SYSADMIN.ToId())
        {
            cabinets = await _cabinetsRepository.SelectAsync(new CabinetsRequest { CabStat = "1" }, ct);
        }
        else
        {
            filter.GroupIds = await _groupMbrService.GetGroupsByUser(new GroupMbrDto { UserId = filter.UserId }, ct);
            filter.GroupIds = filter.GroupIds.Where(id => id != "0").ToList();

            if (filter.GroupIds != null && filter.GroupIds.Any())
            {
                cabinets = await _cabinetsRepository.SelecActiveCabinetsForGroups(filter, ct);
            }
            else
            {
                cabinets = await _cabinetsRepository.SelecActiveCabinetsForUser(filter, ct);
            }
        }
        return cabinets.ToDtos();
    }
}
