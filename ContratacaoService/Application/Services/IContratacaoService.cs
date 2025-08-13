using ContratacaoService.Application.Commands;
using ContratacaoService.Application.DTOs;

namespace ContratacaoService.Application.Services
{
    public interface IContratacaoService
    {
        Task<ContratacaoDto> ContratarPropostaAsync(ContratarPropostaCommand command);
    }
}

