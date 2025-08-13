using Microsoft.AspNetCore.Mvc;
using ContratacaoService.Application.Commands;
using ContratacaoService.Application.Services;

namespace ContratacaoService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ContratacoesController : ControllerBase
    {
        private readonly IContratacaoService _contratacaoService;

        public ContratacoesController(IContratacaoService contratacaoService)
        {
            _contratacaoService = contratacaoService ?? throw new ArgumentNullException(nameof(contratacaoService));
        }

        [HttpPost]
        public async Task<IActionResult> ContratarProposta([FromBody] ContratarPropostaCommand command)
        {
            try
            {
                var contratacao = await _contratacaoService.ContratarPropostaAsync(command);
                return CreatedAtAction(nameof(ContratarProposta), new { id = contratacao.Id }, contratacao);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}

