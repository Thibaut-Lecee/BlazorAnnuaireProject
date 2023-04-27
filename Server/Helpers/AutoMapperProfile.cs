using BlazorAnnuaireProject.Shared.Entities;
using BlazorAnnuaireProject.Models;

namespace BlazorAnnuaireProject.Helpers;
using AutoMapper;
using BlazorAnnuaireProject.Shared;

// L'automapper permet de mapper les données d'un objet à un autre objet
// Ici, on utilise l'automapper pour mapper les données de l'objet Admin à l'objet AuthenticateResponse

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