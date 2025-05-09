namespace PsicoAgenda.Dtos
{
    public class PsicologoDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Email { get; set; }
        public string Telefono { get; set; }
        public string Especialidad { get; set; }
        public string Direccion { get; set; }
        public string HorarioAtencion { get; set; }
    }

    public class PsicologoCreacionDto
    {
        public string Nombre { get; set; }
        public string Email { get; set; }
        public string Telefono { get; set; }
        public string Especialidad { get; set; }
        public string Direccion { get; set; }
        public string HorarioAtencion { get; set; }
        public string Password { get; set; }
    }

    public class PsicologoActualizacionDto
    {
        public string Nombre { get; set; }
        public string Email { get; set; }
        public string Telefono { get; set; }
        public string Especialidad { get; set; }
        public string Direccion { get; set; }
        public string HorarioAtencion { get; set; }
    }
}
