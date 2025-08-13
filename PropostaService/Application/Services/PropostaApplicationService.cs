using PropostaService.Application.Commands;
using PropostaService.Application.DTOs;
using PropostaService.Domain.Entities;
using PropostaService.Domain.Interfaces;

namespace PropostaService.Application.Services
{
    public class PropostaApplicationService : IPropostaService
    {
        private readonly IPropostaRepository _propostaRepository;

        public PropostaApplicationService(IPropostaRepository propostaRepository)
        {
            _propostaRepository = propostaRepository ?? throw new ArgumentNullException(nameof(propostaRepository));
        }

        public async Task<PropostaDto> CriarPropostaAsync(CriarPropostaCommand command)
        {
            var proposta = new Proposta(command.ClienteId, command.ValorProposta);
            var propostaCriada = await _propostaRepository.AdicionarAsync(proposta);

            return MapearParaDto(propostaCriada);
        }

        public async Task<PropostaDto?> ObterPropostaPorIdAsync(Guid id)
        {
            var proposta = await _propostaRepository.ObterPorIdAsync(id);
            return proposta != null ? MapearParaDto(proposta) : null;
        }

        public async Task<IEnumerable<PropostaDto>> ListarPropostasAsync()
        {
            var propostas = await _propostaRepository.ListarTodasAsync();
            return propostas.Select(MapearParaDto);
        }

        public async Task<PropostaDto> AtualizarStatusPropostaAsync(AtualizarStatusPropostaCommand command)
        {
            var proposta = await _propostaRepository.ObterPorIdAsync(command.PropostaId);
            
            if (proposta == null)
                throw new InvalidOperationException($"Proposta com ID {command.PropostaId} não encontrada");

            proposta.AtualizarStatus(command.NovoStatus);
            var propostaAtualizada = await _propostaRepository.AtualizarAsync(proposta);

            return MapearParaDto(propostaAtualizada);
        }

        private static PropostaDto MapearParaDto(Proposta proposta)
        {
            return new PropostaDto
            {
                Id = proposta.Id,
                ClienteId = proposta.ClienteId,
                DataCriacao = proposta.DataCriacao,
                ValorProposta = proposta.ValorProposta,
                Status = proposta.Status
            };
        }
    }
}

