using System.Collections.Generic;
using System.Threading.Tasks;

using Domain;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Application.CreditCards;
using System;
using Application.Core;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CreditCardController : ControllerBase
    {
        private IMediator _mediator;
        private IMediator Mediator => _mediator ??= HttpContext.RequestServices.GetService<IMediator>();

        private ActionResult HandleResult<T>(Result<T> result)
        {
            if (result == null) return NotFound();
            if (result.IsSuccess && result.Value != null) return Ok(result.Value);
            if (result.IsSuccess && result.Value == null) return NotFound();

            return BadRequest(result.Error);
        }

        [HttpGet]
        public async Task<ActionResult<List<CreditCard>>> GetCreditCards()
        {
            return HandleResult(await Mediator.Send(new List.Query()));
        }

        [HttpGet("{CardNumber}")]
        public async Task<ActionResult<CreditCard>> GetCreditCard(long cardNumber)
        {

            return HandleResult(await Mediator.Send(new Details.Query { CardNumber = cardNumber }));
        }

        [HttpPost]
        public async Task<IActionResult> CreateCreditCard(CreditCard card)
        {


            return HandleResult(await Mediator.Send(new Create.Command { CreditCard = card }));
        }
    }
}