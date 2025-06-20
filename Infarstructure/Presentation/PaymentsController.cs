using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServicesAbstractions;

[ApiController]
[Route("api/payments")]
public class PaymentsController : ControllerBase
{
    private readonly IServiceManager _serviceManager;

    public PaymentsController(IServiceManager serviceManager)
    {
        _serviceManager = serviceManager;
    }

    [HttpPost("{basketId}")]
    [Authorize]
    public async Task<IActionResult> CreateOrUpdatePaymentIntent(string basketId)
    {
        var result = await _serviceManager.PaymentService.CreateOrUpdatePaymentIntentAsync(basketId);
        return Ok(result);
    }
}
