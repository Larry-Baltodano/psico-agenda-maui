namespace PsicoAgenda.Dtos
{
    public class CitaDto
    {
        public int Id { get; set; }
        public int PacienteId { get; set; }
        public string PacienteNombre { get; set; }
        public int PsicologoId { get; set; }
        public string PsicologoNombre { get; set; }
        public int DuracionMinutos { get; set; }
        public string Estado { get; set; }
        public Color EstadoColor { get; set; }
        public DateTime FechaCita { get; set; }
    }

    public class CitaCreacionDto
    {
        public int PacienteId { get; set; }
        public int PsicologoId { get; set; }
        public int DuracionMinutos { get; set; }
        public DateTime FechaCita { get; set; }
    }

    public class CitaActualizacionDto
    {
        public int PacienteId { get; set; }
        public int PsicologoId { get; set; }
        public int DuracionMinutos { get; set; }
        public string Estado { get; set; }
        public DateTime FechaCita { get; set; }
    }

    public class CitaNotificacionDto
    {
        public int CitaId { get; set; }
        public string Email { get; set; }
        public string Telefono { get; set; }
    }
}
