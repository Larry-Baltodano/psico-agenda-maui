namespace PsicoAgenda.Dtos
{
    public class PacienteDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Email { get; set; }
        public string Telefono { get; set; }
        public string Direccion { get; set; }
        public string Nota { get; set; }
        public int PsicologoId { get; set; }
    }

    public class PacienteCreacionDto
    {
        public string Nombre { get; set; }
        public string Email { get; set; }
        public string Telefono { get; set; }
        public string Direccion { get; set; }
        public string Nota { get; set; }
        public int PsicologoId { get; set; }
    }
}
