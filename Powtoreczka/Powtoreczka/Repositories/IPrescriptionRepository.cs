using Powtoreczka.DTOs;

namespace Powtoreczka.Repositories;

public interface IPrescriptionRepository
{
    public Task<PrescriptionDto> GetPrescription(int id);
    public Task<int> AddNewPrescriptions(PrescriptionRequestDto addedPrescription);
}