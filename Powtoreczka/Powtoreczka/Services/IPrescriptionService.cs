using Powtoreczka.DTOs;

namespace Powtoreczka.Services;

public interface IPrescriptionService
{
    public Task<PrescriptionDto> GetPrescription(int id);
    public Task<int> AddNewPrescriptions(PrescriptionRequestDto addedPrescription);
}