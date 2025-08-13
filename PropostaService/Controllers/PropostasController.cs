using Microsoft.AspNetCore.Mvc;
using PropostaService.Application.Commands;
using PropostaService.Application.Services;

namespace PropostaService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PropostasController : ControllerBase
    {
        private readonly IPropostaService _propostaService;

        public PropostasController(IPropostaService propostaService)
        {
            _propostaService = propostaService ?? throw new ArgumentNullException(nameof(propostaService));
        }

        [HttpPost]
        public async Task<IActionResult> CriarProposta([FromBody] CriarPropostaCommand command)
        {
            try
            {
                var proposta = await _propostaService.CriarPropostaAsync(command);
                return CreatedAtAction(nameof(ObterProposta), new { id = proposta.Id }, proposta);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ObterProposta(Guid id)
        {
            var proposta = await _propostaService.ObterPropostaPorIdAsync(id);
            
            if (proposta == null)
                return NotFound();

            return Ok(proposta);
        }

        [HttpGet]
        public async Task<IActionResult> ListarPropostas()
        {
            var propostas = await _propostaService.ListarPropostasAsync();
            return Ok(propostas);
        }

        [HttpPut("{id}/status")]
        public async Task<IActionResult> AtualizarStatus(Guid id, [FromBody] AtualizarStatusPropostaCommand command)
        {
            try
            {
                command.PropostaId = id;
                var proposta = await _propostaService.AtualizarStatusPropostaAsync(command);
                return Ok(proposta);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}

