namespace OAI_PMH.Models.SGI.OrganicStructure
{
    public class PersonaSecundaria
    {
        public string Id { get; set; }
        public string Nombre { get; set; }
        public string PrimerApellido { get; set; }
        public string SegundoApellido { get; set; }

        public string NombreCompleto
        {
            get
            {
                string completo = "";
                if (!string.IsNullOrEmpty(Nombre))
                {
                    completo += " " + Nombre;
                }
                if (!string.IsNullOrEmpty(PrimerApellido))
                {
                    completo += " " + PrimerApellido;
                }
                if (!string.IsNullOrEmpty(SegundoApellido))
                {
                    completo += " " + SegundoApellido;
                }
                return completo.Trim();
            }
        }
    }
}
