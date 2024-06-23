using Powtoreczka.DTOs;
using Powtoreczka.Repositories;

namespace Powtoreczka.Services;

public class PrescriptionService : IPrescriptionService
{
    private IPrescriptionRepository _prescriptionRepository;

    public PrescriptionService(IPrescriptionRepository prescriptionRepository)
    {
        _prescriptionRepository = prescriptionRepository;
    }

    public async Task<PrescriptionDto> GetPrescription(int id)
    {
        var result = await _prescriptionRepository.GetPrescription(id);
        return result;
    }

    public async Task<int> AddNewPrescriptions(PrescriptionRequestDto addedPrescription)
    {
        var result = await _prescriptionRepository.AddNewPrescriptions(addedPrescription);
        return result;
    }
}