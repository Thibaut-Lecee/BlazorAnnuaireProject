namespace BlazorAnnuaireProject.AnnuaireServices.SiteService
{
    public class SiteWithServicesDto
    {
        public int Id { get; set; }
        public string Ville { get; set; }
        public string Description { get; set; }
        public List<ServiceDto> Services { get; set; }
    }

    public class SiteWithSalariesDto
    {
        public int Id { get; set; }
        public string Ville { get; set; }
        public string Description { get; set; }
        public List<SalariesDto> Salaries { get; set; }
        public List<string> Services { get; internal set; }
    }
}