using FluentAssertions;
using Moq;
using ContratacaoService.Application.Commands;
using ContratacaoService.Application.DTOs;
using ContratacaoService.Application.Services;
using ContratacaoService.Domain.Entities;
using ContratacaoService.Domain.Interfaces;

namespace ContratacaoService.Tests.Application.Services
{
    public class ContratacaoApplicationServiceTests
    {
        private readonly Mock<IContratacaoRepository> _mockContratacaoRepository;
        private readonly Mock<IPropostaServiceGateway> _mockPropostaGateway;
        private readonly ContratacaoApplicationService _service;

        public ContratacaoApplicationServiceTests()
        {
            _mockContratacaoRepository = new Mock<IContratacaoRepository>();
            _mockPropostaGateway = new Mock<IPropostaServiceGateway>();
            _service = new ContratacaoApplicationService(_mockContratacaoRepository.Object, _mockPropostaGateway.Object);
        }

        [Fact]
        public async Task ContratarPropostaAsync_DeveCriarContratacao_QuandoPropostaAprovada()
        {
            // Arrange
            var propostaId = Guid.NewGuid();
            var command = new ContratarPropostaCommand { PropostaId = propostaId };
            var propostaStatus = new PropostaStatusDto { Id = propostaId, Status = 2 }; // Aprovada
            var contratacao = new Contratacao(propostaId);

            _mockContratacaoRepository.Setup(r => r.ExisteContratacaoParaPropostaAsync(propostaId))
                                     .ReturnsAsync(false);
            _mockPropostaGateway.Setup(g => g.VerificarStatusPropostaAsync(propostaId))
                               .ReturnsAsync(propostaStatus);
            _mockContratacaoRepository.Setup(r => r.AdicionarAsync(It.IsAny<Contratacao>()))
                                     .ReturnsAsync(contratacao);

            // Act
            var resultado = await _service.ContratarPropostaAsync(command);

            // Assert
            resultado.Should().NotBeNull();
            resultado.PropostaId.Should().Be(propostaId);
            _mockContratacaoRepository.Verify(r => r.AdicionarAsync(It.IsAny<Contratacao>()), Times.Once);
        }

        [Fact]
        public async Task ContratarPropostaAsync_DeveLancarExcecao_QuandoJaExisteContratacao()
        {
            // Arrange
            var propostaId = Guid.NewGuid();
            var command = new ContratarPropostaCommand { PropostaId = propostaId };

            _mockContratacaoRepository.Setup(r => r.ExisteContratacaoParaPropostaAsync(propostaId))
                                     .ReturnsAsync(true);

            // Act & Assert
            var act = async () => await _service.ContratarPropostaAsync(command);
            await act.Should().ThrowAsync<InvalidOperationException>()
                     .WithMessage($"Já existe uma contratação para a proposta {propostaId}");
        }

        [Fact]
        public async Task ContratarPropostaAsync_DeveLancarExcecao_QuandoPropostaNaoEncontrada()
        {
            // Arrange
            var propostaId = Guid.NewGuid();
            var command = new ContratarPropostaCommand { PropostaId = propostaId };

            _mockContratacaoRepository.Setup(r => r.ExisteContratacaoParaPropostaAsync(propostaId))
                                     .ReturnsAsync(false);
            _mockPropostaGateway.Setup(g => g.VerificarStatusPropostaAsync(propostaId))
                               .ReturnsAsync((PropostaStatusDto?)null);

            // Act & Assert
            var act = async () => await _service.ContratarPropostaAsync(command);
            await act.Should().ThrowAsync<InvalidOperationException>()
                     .WithMessage($"Proposta {propostaId} não encontrada");
        }

        [Fact]
        public async Task ContratarPropostaAsync_DeveLancarExcecao_QuandoPropostaNaoPodeSerContratada()
        {
            // Arrange
            var propostaId = Guid.NewGuid();
            var command = new ContratarPropostaCommand { PropostaId = propostaId };
            var propostaStatus = new PropostaStatusDto { Id = propostaId, Status = 1 }; // Em Análise

            _mockContratacaoRepository.Setup(r => r.ExisteContratacaoParaPropostaAsync(propostaId))
                                     .ReturnsAsync(false);
            _mockPropostaGateway.Setup(g => g.VerificarStatusPropostaAsync(propostaId))
                               .ReturnsAsync(propostaStatus);

            // Act & Assert
            var act = async () => await _service.ContratarPropostaAsync(command);
            await act.Should().ThrowAsync<InvalidOperationException>()
                     .WithMessage($"Proposta {propostaId} não pode ser contratada. Status atual: 1");
        }
    }
}

