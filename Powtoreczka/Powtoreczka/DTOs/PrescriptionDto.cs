namespace Powtoreczka.DTOs;

public class PrescriptionDto
{
    public int PrescriptionId { get; set; }
    public DateTime Date { get; set; }
    public DateTime DueDate { get; set; }
    public int IdPatient { get; set; }
    public int IdDoctor { get; set; }
    public ICollection<MedicamentDto> Medicaments { get; set; }
}