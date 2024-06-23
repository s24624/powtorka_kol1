using System.Data;
using System.Data.SqlClient;
using System.Net.NetworkInformation;
using System.Xml.Linq;
using Powtoreczka.DTOs;

namespace Powtoreczka.Repositories;

public class PrescriptionRepository : IPrescriptionRepository
{
    private IConfiguration _configuration;

    public PrescriptionRepository(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task<PrescriptionDto> GetPrescription(int id)
    {
        await using var con = new SqlConnection(_configuration["ConnectionStrings:DefaultConnection"]);
        await con.OpenAsync();
        await using var cmd = new SqlCommand();
        cmd.Connection = con;
        cmd.CommandText =
            "Select IdPrescription,Date,DueDate,IdPatient,IdDoctor from Prescription where IdPrescription " +
            " = @IdPrescription";
        cmd.Parameters.AddWithValue("@IdPrescription",id);

        var dr = await cmd.ExecuteReaderAsync();
        if (await dr.ReadAsync() == null)
            throw new DataException("Prescription with this id does not exist");
        
        var prescriptionWithMedicaments = new PrescriptionDto()
        {
            PrescriptionId = (int)dr["IdPrescription"],
            IdDoctor = (int)dr["IdDoctor"],
            IdPatient = (int)dr["IdPatient"],
            DueDate = (DateTime)dr["DueDate"],
            Date = (DateTime)dr["Date"],
        };
        await dr.CloseAsync();
        prescriptionWithMedicaments.Medicaments = await GetMedicamentsFromPrescription(id, con);
        
        return prescriptionWithMedicaments;
    }

   

    public async Task<ICollection<MedicamentDto>> GetMedicamentsFromPrescription(int prescriptionId, SqlConnection con)
    {
        await using var cmd = new SqlCommand();
        cmd.Connection = con;
        cmd.CommandText =
            "Select pm.IdPrescription, Name,Type From Prescription_Medicament pm inner join Medicament m on " +
            "pm.IdMedicament = m.IdMedicament where pm.IdPrescription = @IdPrescription";
        cmd.Parameters.AddWithValue("@IdPrescription", prescriptionId);
        var dr = await cmd.ExecuteReaderAsync();
        var medicaments = new List<MedicamentDto>();
        while (await dr.ReadAsync())
        {
            var medicament = new MedicamentDto()
            {
                Name = dr["Name"].ToString(),
                Type = dr["Type"].ToString()
            };
            medicaments.Add(medicament);
        }

        return medicaments;
    }
    
    public async Task<int> AddNewPrescriptions(PrescriptionRequestDto addedPrescription)
    {
        if (!(addedPrescription.DueDate < addedPrescription.Date))
            throw new DataException("DueDate must be older that Date");
            
        await using var con = new SqlConnection(_configuration["ConnectionStrings:DefaultConnection"]);
        await con.OpenAsync();
        await using var cmd = new SqlCommand();
        cmd.Connection = con;
        cmd.CommandText = "Insert into Prescription (Date,DueDate,IdPatient,IdDoctor) " +
                          "Values(@Date,@DueDate,@IdPatient,@IdDoctor)";
        cmd.Parameters.AddWithValue("@Date", addedPrescription.Date);
        cmd.Parameters.AddWithValue("@DueDate", addedPrescription.DueDate);
        cmd.Parameters.AddWithValue("@IdPatient", addedPrescription.IdPatient);
        cmd.Parameters.AddWithValue("@IdDoctor", addedPrescription.IdDoctor);
        var createdId = await cmd.ExecuteNonQueryAsync();
        return createdId;
        
    }
}