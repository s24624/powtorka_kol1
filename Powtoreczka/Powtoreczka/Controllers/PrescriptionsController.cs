using Microsoft.AspNetCore.Mvc;
using Powtoreczka.DTOs;
using Powtoreczka.Services;

namespace Powtoreczka.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PrescriptionsController : ControllerBase
{
    private IPrescriptionService _prescriptionService;

    public PrescriptionsController(IPrescriptionService prescriptionService)
    {
        _prescriptionService = prescriptionService;
    }

    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetPrescriptions(int id)
    {
        var result = await _prescriptionService.GetPrescription(id);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> AddPrescription(PrescriptionRequestDto prescriptionRequestDto)
    {
        var result = await _prescriptionService.AddNewPrescriptions(prescriptionRequestDto);
        return Ok(new { result });
    }

}