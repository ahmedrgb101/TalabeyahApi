using Ardalis.Specification;
using TalabeyahTaskApi.Application.Common;
using TalabeyahTaskApi.Application.Common.Exporters;
using TalabeyahTaskApi.Application.Common.Models;
using TalabeyahTaskApi.Application.Identity.Users;
using TalabeyahTaskApi.Domain.Catalog;
using TalabeyahTaskApi.Domain.Common.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TalabeyahTaskApi.Application.TicketManager;

#region Governorate
#region Create
public class CreateGovernorateRequest : IRequest<int>
{
    public string Name { get; set; }
}

public class CreateGovernorateRequestHandler : IRequestHandler<CreateGovernorateRequest, int>
{
    private readonly IRepository<Governorate> _repository;
    private readonly IFileStorageService _file;

    public CreateGovernorateRequestHandler(IRepository<Governorate> repository, IFileStorageService file) =>
        (_repository, _file) = (repository, file);

    public async Task<int> Handle(CreateGovernorateRequest request, CancellationToken cancellationToken)
    {
        var Governorate = new Governorate(request.Name);

        // Add Domain Events to be raised after the commit
        Governorate.DomainEvents.Add(EntityCreatedEvent.WithEntity(Governorate));

        await _repository.AddAsync(Governorate, cancellationToken);

        return Governorate.Id;
    }
}
#endregion
#region Update
public class UpdateGovernorateRequest : IRequest<int>
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int RegionId { get; set; }
}
public class UpdateGovernorateRequestHandler : IRequestHandler<UpdateGovernorateRequest, int>
{
    private readonly IRepository<Governorate> _repository;
    private readonly IStringLocalizer<UpdateGovernorateRequestHandler> _localizer;
    private readonly IFileStorageService _file;

    public UpdateGovernorateRequestHandler(IRepository<Governorate> repository, IStringLocalizer<UpdateGovernorateRequestHandler> localizer, IFileStorageService file) =>
        (_repository, _localizer, _file) = (repository, localizer, file);

    public async Task<int> Handle(UpdateGovernorateRequest request, CancellationToken cancellationToken)
    {
        var Governorate = await _repository.GetByIdAsync(request.Id, cancellationToken);

        _ = Governorate ?? throw new NotFoundException(string.Format(_localizer["Governorate.notfound"], request.Id));

        var updatedGovernorate = Governorate.Update(request.Name);

        // Add Domain Events to be raised after the commit
        Governorate.DomainEvents.Add(EntityUpdatedEvent.WithEntity(Governorate));

        await _repository.UpdateAsync(updatedGovernorate, cancellationToken);

        return request.Id;
    }
}
#endregion
#region Get
public class GetGovernorateRequest : IRequest<GovernorateDto>
{
    public int Id { get; set; }

    public GetGovernorateRequest(int id) => Id = id;
}

public class GetGovernorateRequestHandler : IRequestHandler<GetGovernorateRequest, GovernorateDto>
{
    private readonly IRepository<Governorate> _repository;
    private readonly IStringLocalizer<GetGovernorateRequestHandler> _localizer;

    public GetGovernorateRequestHandler(IRepository<Governorate> repository, IStringLocalizer<GetGovernorateRequestHandler> localizer) =>
        (_repository, _localizer) = (repository, localizer);

    public async Task<GovernorateDto> Handle(GetGovernorateRequest request, CancellationToken cancellationToken) =>
        await _repository.GetBySpecAsync(
            (ISpecification<Governorate, GovernorateDto>)new GovernorateByIdSpec(request.Id), cancellationToken)
        ?? throw new NotFoundException(string.Format(_localizer["Governorate.notfound"], request.Id));
}

public class GovernorateByIdSpec : Specification<Governorate, GovernorateDto>, ISingleResultSpecification
{
    public GovernorateByIdSpec(int Id) =>
        Query.Where(p => p.Id == Id);
}

public class GovernorateByNameSpec : Specification<Governorate>, ISingleResultSpecification
{
    public GovernorateByNameSpec(string name) =>
        Query.Where(p => p.Name == name);
}
#endregion
#region Search
public class GovernorateBySearchRequestWithNameSpec : EntitiesByPaginationFilterSpec<Governorate, GovernorateDto>
{
    public GovernorateBySearchRequestWithNameSpec(SearchGovernorateRequest request)
        : base(request) =>
        Query
            .Where(p => p.Name.Contains(request.Name), !string.IsNullOrEmpty(request.Name));
}
public class SearchGovernorateRequest : PaginationFilter, IRequest<PaginationResponse<GovernorateDto>>
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public bool IsAdmin { get; set; }
}

public class SearchGovernorateRequestHandler : IRequestHandler<SearchGovernorateRequest, PaginationResponse<GovernorateDto>>
{
    private readonly IReadRepository<Governorate> _repository;

    public SearchGovernorateRequestHandler(IReadRepository<Governorate> repository) => _repository = repository;

    public async Task<PaginationResponse<GovernorateDto>> Handle(SearchGovernorateRequest request, CancellationToken cancellationToken)
    {
        var spec = new GovernorateBySearchRequestWithNameSpec(request);
        return await _repository.PaginatedListAsync(spec,
                                                    request.PageNumber,
                                                    request.PageSize,
                                                    cancellationToken: cancellationToken);
    }
}
#endregion
#region Delete
public class DeleteGovernorateRequest : IRequest<int>
{
    public int Id { get; set; }

    public DeleteGovernorateRequest(int id) => Id = id;
}

public class DeleteGovernorateRequestHandler : IRequestHandler<DeleteGovernorateRequest, int>
{
    private readonly IRepository<Governorate> _repository;
    private readonly IStringLocalizer<DeleteGovernorateRequestHandler> _localizer;

    public DeleteGovernorateRequestHandler(IRepository<Governorate> repository, IStringLocalizer<DeleteGovernorateRequestHandler> localizer) =>
        (_repository, _localizer) = (repository, localizer);

    public async Task<int> Handle(DeleteGovernorateRequest request, CancellationToken cancellationToken)
    {
        var Governorate = await _repository.GetByIdAsync(request.Id, cancellationToken);

        _ = Governorate ?? throw new NotFoundException(_localizer["Governorate.notfound"]);

        // Add Domain Events to be raised after the commit
        Governorate.DomainEvents.Add(EntityDeletedEvent.WithEntity(Governorate));

        await _repository.DeleteAsync(Governorate, cancellationToken);

        return request.Id;
    }
}
#endregion
#region Validator
public class UpdateGovernorateRequestValidator : CustomValidator<UpdateGovernorateRequest>
{
    public UpdateGovernorateRequestValidator(IReadRepository<Governorate> GovernorateRepo, IStringLocalizer<UpdateGovernorateRequestValidator> localizer)
    {
        RuleFor(p => p.Name)
            .NotEmpty()
            .MaximumLength(75)
            .MustAsync(async (Governorate, name, ct) =>
                    await GovernorateRepo.GetBySpecAsync(new GovernorateByNameSpec(name), ct)
                        is not Governorate existingGovernorate || existingGovernorate.Id == Governorate.Id)
                .WithMessage((_, name) => string.Format(localizer["Governorate.alreadyexists"], name));
    }
}
#endregion
#endregion

#region City
#region Create
public class CreateCityRequest : IRequest<int>
{
    public string Name { get; set; }
    public int GovernorateId { get; set; }
}

public class CreateCityRequestHandler : IRequestHandler<CreateCityRequest, int>
{
    private readonly IRepository<City> _repository;
    private readonly IFileStorageService _file;

    public CreateCityRequestHandler(IRepository<City> repository, IFileStorageService file) =>
        (_repository, _file) = (repository, file);

    public async Task<int> Handle(CreateCityRequest request, CancellationToken cancellationToken)
    {
        var City = new City(request.Name, request.GovernorateId);

        // Add Domain Events to be raised after the commit
        City.DomainEvents.Add(EntityCreatedEvent.WithEntity(City));

        await _repository.AddAsync(City, cancellationToken);

        return City.Id;
    }
}
#endregion
#region Update
public class UpdateCityRequest : IRequest<int>
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int GovernorateId { get; set; }
}
public class UpdateCityRequestHandler : IRequestHandler<UpdateCityRequest, int>
{
    private readonly IRepository<City> _repository;
    private readonly IStringLocalizer<UpdateCityRequestHandler> _localizer;
    private readonly IFileStorageService _file;

    public UpdateCityRequestHandler(IRepository<City> repository, IStringLocalizer<UpdateCityRequestHandler> localizer, IFileStorageService file) =>
        (_repository, _localizer, _file) = (repository, localizer, file);

    public async Task<int> Handle(UpdateCityRequest request, CancellationToken cancellationToken)
    {
        var City = await _repository.GetByIdAsync(request.Id, cancellationToken);

        _ = City ?? throw new NotFoundException(string.Format(_localizer["City.notfound"], request.Id));

        var updatedCity = City.Update(request.Name, request.GovernorateId);

        // Add Domain Events to be raised after the commit
        City.DomainEvents.Add(EntityUpdatedEvent.WithEntity(City));

        await _repository.UpdateAsync(updatedCity, cancellationToken);

        return request.Id;
    }
}
#endregion
#region Get
public class GetCityRequest : IRequest<CityDto>
{
    public int Id { get; set; }

    public GetCityRequest(int id) => Id = id;
}

public class GetCityRequestHandler : IRequestHandler<GetCityRequest, CityDto>
{
    private readonly IRepository<City> _repository;
    private readonly IStringLocalizer<GetCityRequestHandler> _localizer;

    public GetCityRequestHandler(IRepository<City> repository, IStringLocalizer<GetCityRequestHandler> localizer) =>
        (_repository, _localizer) = (repository, localizer);

    public async Task<CityDto> Handle(GetCityRequest request, CancellationToken cancellationToken) =>
        await _repository.GetBySpecAsync(
            (ISpecification<City, CityDto>)new CityByIdSpec(request.Id), cancellationToken)
        ?? throw new NotFoundException(string.Format(_localizer["ActionPlan.notfound"], request.Id));
}

public class CityByIdSpec : Specification<City, CityDto>, ISingleResultSpecification
{
    public CityByIdSpec(int Id) =>
        Query.Where(p => p.Id == Id);
}

public class CityByNameSpec : Specification<City>, ISingleResultSpecification
{
    public CityByNameSpec(string name) =>
        Query.Where(p => p.Name == name);
}
#endregion
#region Search
public class CityBySearchRequestWithNameSpec : EntitiesByPaginationFilterSpec<City, CityDto>
{
    public CityBySearchRequestWithNameSpec(SearchCityRequest request)
        : base(request) =>
        Query
            .Where(p => p.Name.Contains(request.Name), !string.IsNullOrEmpty(request.Name))
            .Where(p => p.GovernorateId == request.GovernorateId, request.GovernorateId > 0);
}
public class SearchCityRequest : PaginationFilter, IRequest<PaginationResponse<CityDto>>
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public int GovernorateId { get; set; }
}

public class SearchCityRequestHandler : IRequestHandler<SearchCityRequest, PaginationResponse<CityDto>>
{
    private readonly IReadRepository<City> _repository;

    public SearchCityRequestHandler(IReadRepository<City> repository) => _repository = repository;

    public async Task<PaginationResponse<CityDto>> Handle(SearchCityRequest request, CancellationToken cancellationToken)
    {
        var spec = new CityBySearchRequestWithNameSpec(request);
        return await _repository.PaginatedListAsync(spec,
                                                    request.PageNumber,
                                                    request.PageSize,
                                                    cancellationToken: cancellationToken);
    }
}
#endregion
#region Delete
public class DeleteCityRequest : IRequest<int>
{
    public int Id { get; set; }

    public DeleteCityRequest(int id) => Id = id;
}

public class DeleteCityRequestHandler : IRequestHandler<DeleteCityRequest, int>
{
    private readonly IRepository<City> _repository;
    private readonly IStringLocalizer<DeleteCityRequestHandler> _localizer;

    public DeleteCityRequestHandler(IRepository<City> repository, IStringLocalizer<DeleteCityRequestHandler> localizer) =>
        (_repository, _localizer) = (repository, localizer);

    public async Task<int> Handle(DeleteCityRequest request, CancellationToken cancellationToken)
    {
        var City = await _repository.GetByIdAsync(request.Id, cancellationToken);

        _ = City ?? throw new NotFoundException(_localizer["City.notfound"]);

        // Add Domain Events to be raised after the commit
        City.DomainEvents.Add(EntityDeletedEvent.WithEntity(City));

        await _repository.DeleteAsync(City, cancellationToken);

        return request.Id;
    }
}
#endregion
#region Validator
public class UpdateCityRequestValidator : CustomValidator<UpdateCityRequest>
{
    public UpdateCityRequestValidator(IReadRepository<City> CityRepo, IStringLocalizer<UpdateCityRequestValidator> localizer)
    {
        RuleFor(p => p.Name)
            .NotEmpty()
            .MaximumLength(75)
            .MustAsync(async (City, name, ct) =>
                    await CityRepo.GetBySpecAsync(new CityByNameSpec(name), ct)
                        is not City existingCity || existingCity.Id == City.Id)
                .WithMessage((_, name) => string.Format(localizer["City.alreadyexists"], name));
    }
}
#endregion
#endregion

#region District
#region Create
public class CreateDistrictRequest : IRequest<int>
{
    public string Name { get; set; }
    public int CityId { get; set; }
}

public class CreateDistrictRequestHandler : IRequestHandler<CreateDistrictRequest, int>
{
    private readonly IRepository<District> _repository;
    private readonly IFileStorageService _file;

    public CreateDistrictRequestHandler(IRepository<District> repository, IFileStorageService file) =>
        (_repository, _file) = (repository, file);

    public async Task<int> Handle(CreateDistrictRequest request, CancellationToken cancellationToken)
    {
        var District = new District(request.Name, request.CityId);

        // Add Domain Events to be raised after the commit
        District.DomainEvents.Add(EntityCreatedEvent.WithEntity(District));

        await _repository.AddAsync(District, cancellationToken);

        return District.Id;
    }
}
#endregion
#region Update
public class UpdateDistrictRequest : IRequest<int>
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int GovernorateId { get; set; }
}
public class UpdateDistrictRequestHandler : IRequestHandler<UpdateDistrictRequest, int>
{
    private readonly IRepository<District> _repository;
    private readonly IStringLocalizer<UpdateDistrictRequestHandler> _localizer;
    private readonly IFileStorageService _file;

    public UpdateDistrictRequestHandler(IRepository<District> repository, IStringLocalizer<UpdateDistrictRequestHandler> localizer, IFileStorageService file) =>
        (_repository, _localizer, _file) = (repository, localizer, file);

    public async Task<int> Handle(UpdateDistrictRequest request, CancellationToken cancellationToken)
    {
        var District = await _repository.GetByIdAsync(request.Id, cancellationToken);

        _ = District ?? throw new NotFoundException(string.Format(_localizer["District.notfound"], request.Id));

        var updatedDistrict = District.Update(request.Name, request.GovernorateId);

        // Add Domain Events to be raised after the commit
        District.DomainEvents.Add(EntityUpdatedEvent.WithEntity(District));

        await _repository.UpdateAsync(updatedDistrict, cancellationToken);

        return request.Id;
    }
}
#endregion
#region Get
public class GetDistrictRequest : IRequest<DistrictDto>
{
    public int Id { get; set; }

    public GetDistrictRequest(int id) => Id = id;
}

public class GetDistrictRequestHandler : IRequestHandler<GetDistrictRequest, DistrictDto>
{
    private readonly IRepository<District> _repository;
    private readonly IStringLocalizer<GetDistrictRequestHandler> _localizer;

    public GetDistrictRequestHandler(IRepository<District> repository, IStringLocalizer<GetDistrictRequestHandler> localizer) =>
        (_repository, _localizer) = (repository, localizer);

    public async Task<DistrictDto> Handle(GetDistrictRequest request, CancellationToken cancellationToken) =>
        await _repository.GetBySpecAsync(
            (ISpecification<District, DistrictDto>)new DistrictByIdSpec(request.Id), cancellationToken)
        ?? throw new NotFoundException(string.Format(_localizer["ActionPlan.notfound"], request.Id));
}

public class DistrictByIdSpec : Specification<District, DistrictDto>, ISingleResultSpecification
{
    public DistrictByIdSpec(int Id) =>
        Query.Where(p => p.Id == Id);
}

public class DistrictByNameSpec : Specification<District>, ISingleResultSpecification
{
    public DistrictByNameSpec(string name) =>
        Query.Where(p => p.Name == name);
}
#endregion
#region Search
public class DistrictBySearchRequestWithNameSpec : EntitiesByPaginationFilterSpec<District, DistrictDto>
{
    public DistrictBySearchRequestWithNameSpec(SearchDistrictRequest request)
        : base(request) =>
        Query
            .Where(p => p.Name.Contains(request.Name), !string.IsNullOrEmpty(request.Name))
            .Where(p => p.CityId == request.CityId, request.CityId > 0);
}

public class SearchDistrictRequest : PaginationFilter, IRequest<PaginationResponse<DistrictDto>>
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public int CityId { get; set; }
}

public class SearchDistrictRequestHandler : IRequestHandler<SearchDistrictRequest, PaginationResponse<DistrictDto>>
{
    private readonly IReadRepository<District> _repository;

    public SearchDistrictRequestHandler(IReadRepository<District> repository) => _repository = repository;

    public async Task<PaginationResponse<DistrictDto>> Handle(SearchDistrictRequest request, CancellationToken cancellationToken)
    {
        var spec = new DistrictBySearchRequestWithNameSpec(request);
        return await _repository.PaginatedListAsync(spec,
                                                    request.PageNumber,
                                                    request.PageSize,
                                                    cancellationToken: cancellationToken);
    }
}
#endregion
#region Delete
public class DeleteDistrictRequest : IRequest<int>
{
    public int Id { get; set; }

    public DeleteDistrictRequest(int id) => Id = id;
}

public class DeleteDistrictRequestHandler : IRequestHandler<DeleteDistrictRequest, int>
{
    private readonly IRepository<District> _repository;
    private readonly IStringLocalizer<DeleteDistrictRequestHandler> _localizer;

    public DeleteDistrictRequestHandler(IRepository<District> repository, IStringLocalizer<DeleteDistrictRequestHandler> localizer) =>
        (_repository, _localizer) = (repository, localizer);

    public async Task<int> Handle(DeleteDistrictRequest request, CancellationToken cancellationToken)
    {
        var District = await _repository.GetByIdAsync(request.Id, cancellationToken);

        _ = District ?? throw new NotFoundException(_localizer["District.notfound"]);

        // Add Domain Events to be raised after the commit
        District.DomainEvents.Add(EntityDeletedEvent.WithEntity(District));

        await _repository.DeleteAsync(District, cancellationToken);

        return request.Id;
    }
}
#endregion
#region Validator
public class UpdateDistrictRequestValidator : CustomValidator<UpdateDistrictRequest>
{
    public UpdateDistrictRequestValidator(IReadRepository<District> DistrictRepo, IStringLocalizer<UpdateDistrictRequestValidator> localizer)
    {
        RuleFor(p => p.Name)
            .NotEmpty()
            .MaximumLength(75)
            .MustAsync(async (District, name, ct) =>
                    await DistrictRepo.GetBySpecAsync(new DistrictByNameSpec(name), ct)
                        is not District existingDistrict || existingDistrict.Id == District.Id)
                .WithMessage((_, name) => string.Format(localizer["District.alreadyexists"], name));
    }
}
#endregion
#endregion
