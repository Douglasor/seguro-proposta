using Microsoft.EntityFrameworkCore;
using PropostaService.Domain.Entities;
using PropostaService.Domain.Interfaces;
using PropostaService.Infrastructure.Data;

namespace PropostaService.Infrastructure.Repositories
{
    public class PropostaRepository : IPropostaRepository
    {
        private readonly PropostaDbContext _context;

        public PropostaRepository(PropostaDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<Proposta> AdicionarAsync(Proposta proposta)
        {
            _context.Propostas.Add(proposta);
            await _context.SaveChangesAsync();
            return proposta;
        }

        public async Task<Proposta?> ObterPorIdAsync(Guid id)
        {
            return await _context.Propostas.FindAsync(id);
        }

        public async Task<IEnumerable<Proposta>> ListarTodasAsync()
        {
            return await _context.Propostas.ToListAsync();
        }

        public async Task<Proposta> AtualizarAsync(Proposta proposta)
        {
            _context.Propostas.Update(proposta);
            await _context.SaveChangesAsync();
            return proposta;
        }

        public async Task<bool> ExisteAsync(Guid id)
        {
            return await _context.Propostas.AnyAsync(p => p.Id == id);
        }
    }
}

