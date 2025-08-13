using ContratacaoService.Domain.Entities;

namespace ContratacaoService.Domain.Interfaces
{
    public interface IContratacaoRepository
    {
        Task<Contratacao> AdicionarAsync(Contratacao contratacao);
        Task<Contratacao?> ObterPorPropostaIdAsync(Guid propostaId);
        Task<bool> ExisteContratacaoParaPropostaAsync(Guid propostaId);
    }
}

