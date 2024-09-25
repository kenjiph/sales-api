using Application.Abstractions.Ports;
using Application.Contracts;
using Domain.Modules.Sales.Aggregates;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SaleController : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAllSales(
            [FromServices] IQueryHandler<Query.GetAllSalesQuery, IEnumerable<Sale>> handler,
            CancellationToken cancellationToken)
        {
            try
            {
                var result = await handler.Handle(new Query.GetAllSalesQuery(), cancellationToken);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetSaleById(
            [FromServices] IQueryHandler<Query.GetSaleByIdQuery, Sale> handler,
            [FromRoute] Guid id,
            CancellationToken cancellationToken)
        {
            try
            {
                var result = await handler.Handle(new Query.GetSaleByIdQuery(id), cancellationToken);
                if (result == null)
                    return NotFound();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateSale(
            [FromServices] ICommandHandler<Command.CreateSaleCommand> handler,
            [FromBody] Command.CreateSaleCommand command,
            CancellationToken cancellationToken)
        {
            try
            {
                await handler.Handle(command, cancellationToken);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        
        [HttpPost("CreateFromServiceBus")]
        public async Task<IActionResult> CreateSaleFromServiceBus(
            [FromServices] ICommandHandler<Command.CreateSaleFromServiceBusCommand> handler,
            CancellationToken cancellationToken)
        {
            try
            {
                var command = new Command.CreateSaleFromServiceBusCommand();
                await handler.Handle(command, cancellationToken);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPut("UpdateSale")]
        public async Task<IActionResult> UpdateSale(
            [FromServices] ICommandHandler<Command.UpdateSaleCommand> handler,
            [FromBody] Command.UpdateSaleCommand command,
            CancellationToken cancellationToken)
        {
            try
            {
                await handler.Handle(command, cancellationToken);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSale(
            [FromServices] ICommandHandler<Command.DeleteSaleCommand> handler,
            [FromRoute] Guid id,
            CancellationToken cancellationToken)
        {
            try
            {
                var command = new Command.DeleteSaleCommand(id);
                await handler.Handle(command, cancellationToken);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}
