using Microsoft.EntityFrameworkCore;
using ContratacaoService.Domain.Entities;
using ContratacaoService.Domain.Interfaces;
using ContratacaoService.Infrastructure.Data;

namespace ContratacaoService.Infrastructure.Repositories
{
    public class ContratacaoRepository : IContratacaoRepository
    {
        private readonly ContratacaoDbContext _context;

        public ContratacaoRepository(ContratacaoDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<Contratacao> AdicionarAsync(Contratacao contratacao)
        {
            _context.Contratacoes.Add(contratacao);
            await _context.SaveChangesAsync();
            return contratacao;
        }

        public async Task<Contratacao?> ObterPorPropostaIdAsync(Guid propostaId)
        {
            return await _context.Contratacoes
                .FirstOrDefaultAsync(c => c.PropostaId == propostaId);
        }

        public async Task<bool> ExisteContratacaoParaPropostaAsync(Guid propostaId)
        {
            return await _context.Contratacoes
                .AnyAsync(c => c.PropostaId == propostaId);
        }
    }
}

