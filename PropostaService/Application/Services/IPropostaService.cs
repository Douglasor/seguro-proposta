using PropostaService.Application.Commands;
using PropostaService.Application.DTOs;

namespace PropostaService.Application.Services
{
    public interface IPropostaService
    {
        Task<PropostaDto> CriarPropostaAsync(CriarPropostaCommand command);
        Task<PropostaDto?> ObterPropostaPorIdAsync(Guid id);
        Task<IEnumerable<PropostaDto>> ListarPropostasAsync();
        Task<PropostaDto> AtualizarStatusPropostaAsync(AtualizarStatusPropostaCommand command);
    }
}

