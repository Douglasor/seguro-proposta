using ContratacaoService.Application.DTOs;

namespace ContratacaoService.Domain.Interfaces
{
    public interface IPropostaServiceGateway
    {
        Task<PropostaStatusDto?> VerificarStatusPropostaAsync(Guid propostaId);
    }
}

