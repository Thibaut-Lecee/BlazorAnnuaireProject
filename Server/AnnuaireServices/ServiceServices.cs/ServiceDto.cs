namespace BlazorAnnuaireProject.AnnuaireServices.SiteService
{
    public class ServiceDto
    {
        public int Id { get; set; }
        public string Nom { get; set; }

        public List<SalariesDto> Salaries { get; set; }

    }
}