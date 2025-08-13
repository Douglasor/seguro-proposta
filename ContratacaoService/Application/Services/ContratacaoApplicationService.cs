using ContratacaoService.Application.Commands;
using ContratacaoService.Application.DTOs;
using ContratacaoService.Domain.Entities;
using ContratacaoService.Domain.Interfaces;

namespace ContratacaoService.Application.Services
{
    public class ContratacaoApplicationService : IContratacaoService
    {
        private readonly IContratacaoRepository _contratacaoRepository;
        private readonly IPropostaServiceGateway _propostaServiceGateway;

        public ContratacaoApplicationService(
            IContratacaoRepository contratacaoRepository,
            IPropostaServiceGateway propostaServiceGateway)
        {
            _contratacaoRepository = contratacaoRepository ?? throw new ArgumentNullException(nameof(contratacaoRepository));
            _propostaServiceGateway = propostaServiceGateway ?? throw new ArgumentNullException(nameof(propostaServiceGateway));
        }

        public async Task<ContratacaoDto> ContratarPropostaAsync(ContratarPropostaCommand command)
        {
            // Verificar se já existe contratação para esta proposta
            var contratacaoExistente = await _contratacaoRepository.ExisteContratacaoParaPropostaAsync(command.PropostaId);
            if (contratacaoExistente)
                throw new InvalidOperationException($"Já existe uma contratação para a proposta {command.PropostaId}");

            // Verificar status da proposta no PropostaService
            var propostaStatus = await _propostaServiceGateway.VerificarStatusPropostaAsync(command.PropostaId);
            if (propostaStatus == null)
                throw new InvalidOperationException($"Proposta {command.PropostaId} não encontrada");

            if (!propostaStatus.PodeSerContratada)
                throw new InvalidOperationException($"Proposta {command.PropostaId} não pode ser contratada. Status atual: {propostaStatus.Status}");

            // Criar contratação
            var contratacao = new Contratacao(command.PropostaId);
            var contratacaoCriada = await _contratacaoRepository.AdicionarAsync(contratacao);

            return MapearParaDto(contratacaoCriada);
        }

        private static ContratacaoDto MapearParaDto(Contratacao contratacao)
        {
            return new ContratacaoDto
            {
                Id = contratacao.Id,
                PropostaId = contratacao.PropostaId,
                DataContratacao = contratacao.DataContratacao
            };
        }
    }
}

