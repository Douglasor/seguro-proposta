using PropostaService.Domain.Entities;

namespace PropostaService.Domain.Interfaces
{
    public interface IPropostaRepository
    {
        Task<Proposta> AdicionarAsync(Proposta proposta);
        Task<Proposta?> ObterPorIdAsync(Guid id);
        Task<IEnumerable<Proposta>> ListarTodasAsync();
        Task<Proposta> AtualizarAsync(Proposta proposta);
        Task<bool> ExisteAsync(Guid id);
    }
}

