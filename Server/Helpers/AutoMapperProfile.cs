using BlazorAnnuaireProject.Entities;
using BlazorAnnuaireProject.Models;

namespace BlazorAnnuaireProject.Helpers;
using AutoMapper;
public class AutoMapperProfile : Profile
{

    public AutoMapperProfile()
    {
        CreateMap<Admin, AuthenticateResponse>();

        CreateMap<RegisterRequest, Salaries>();

        CreateMap<Site, CreateSiteResponse>();

        CreateMap<CreateSiteRequest, Site>();


    }

}