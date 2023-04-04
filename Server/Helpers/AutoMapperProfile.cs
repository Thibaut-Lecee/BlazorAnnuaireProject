using BlazorAnnuaireProject.Shared.Entities;
using BlazorAnnuaireProject.Models;

namespace BlazorAnnuaireProject.Helpers;
using AutoMapper;
using BlazorAnnuaireProject.Shared;

public class AutoMapperProfile : Profile
{

    public AutoMapperProfile()
    {
        CreateMap<Admin, AuthenticateResponse>();

        CreateMap<RegisterRequestSalarie, Salaries>();

        CreateMap<Site, CreateSiteResponse>();

        CreateMap<CreateSiteRequest, Site>();

        CreateMap<CreateServiceRequest, Services>();

        CreateMap<Services, CreateServiceResponse>();

    }

}