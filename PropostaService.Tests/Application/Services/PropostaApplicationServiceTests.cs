using FluentAssertions;
using Moq;
using PropostaService.Application.Commands;
using PropostaService.Application.Services;
using PropostaService.Domain.Entities;
using PropostaService.Domain.Enums;
using PropostaService.Domain.Interfaces;

namespace PropostaService.Tests.Application.Services
{
    public class PropostaApplicationServiceTests
    {
        private readonly Mock<IPropostaRepository> _mockRepository;
        private readonly PropostaApplicationService _service;

        public PropostaApplicationServiceTests()
        {
            _mockRepository = new Mock<IPropostaRepository>();
            _service = new PropostaApplicationService(_mockRepository.Object);
        }

        [Fact]
        public async Task CriarPropostaAsync_DeveCriarProposta_ComDadosValidos()
        {
            // Arrange
            var command = new CriarPropostaCommand
            {
                ClienteId = Guid.NewGuid(),
                ValorProposta = 1000m
            };

            var propostaEsperada = new Proposta(command.ClienteId, command.ValorProposta);
            _mockRepository.Setup(r => r.AdicionarAsync(It.IsAny<Proposta>()))
                          .ReturnsAsync(propostaEsperada);

            // Act
            var resultado = await _service.CriarPropostaAsync(command);

            // Assert
            resultado.Should().NotBeNull();
            resultado.ClienteId.Should().Be(command.ClienteId);
            resultado.ValorProposta.Should().Be(command.ValorProposta);
            resultado.Status.Should().Be(StatusProposta.EmAnalise);
            _mockRepository.Verify(r => r.AdicionarAsync(It.IsAny<Proposta>()), Times.Once);
        }

        [Fact]
        public async Task ObterPropostaPorIdAsync_DeveRetornarProposta_QuandoExiste()
        {
            // Arrange
            var propostaId = Guid.NewGuid();
            var proposta = new Proposta(Guid.NewGuid(), 1000m);
            _mockRepository.Setup(r => r.ObterPorIdAsync(propostaId))
                          .ReturnsAsync(proposta);

            // Act
            var resultado = await _service.ObterPropostaPorIdAsync(propostaId);

            // Assert
            resultado.Should().NotBeNull();
            resultado!.Id.Should().Be(proposta.Id);
        }

        [Fact]
        public async Task ObterPropostaPorIdAsync_DeveRetornarNull_QuandoNaoExiste()
        {
            // Arrange
            var propostaId = Guid.NewGuid();
            _mockRepository.Setup(r => r.ObterPorIdAsync(propostaId))
                          .ReturnsAsync((Proposta?)null);

            // Act
            var resultado = await _service.ObterPropostaPorIdAsync(propostaId);

            // Assert
            resultado.Should().BeNull();
        }

        [Fact]
        public async Task AtualizarStatusPropostaAsync_DeveAtualizarStatus_QuandoPropostaExiste()
        {
            // Arrange
            var propostaId = Guid.NewGuid();
            var proposta = new Proposta(Guid.NewGuid(), 1000m);
            var command = new AtualizarStatusPropostaCommand
            {
                PropostaId = propostaId,
                NovoStatus = StatusProposta.Aprovada
            };

            _mockRepository.Setup(r => r.ObterPorIdAsync(propostaId))
                          .ReturnsAsync(proposta);
            _mockRepository.Setup(r => r.AtualizarAsync(It.IsAny<Proposta>()))
                          .ReturnsAsync(proposta);

            // Act
            var resultado = await _service.AtualizarStatusPropostaAsync(command);

            // Assert
            resultado.Should().NotBeNull();
            resultado.Status.Should().Be(StatusProposta.Aprovada);
            _mockRepository.Verify(r => r.AtualizarAsync(It.IsAny<Proposta>()), Times.Once);
        }

        [Fact]
        public async Task AtualizarStatusPropostaAsync_DeveLancarExcecao_QuandoPropostaNaoExiste()
        {
            // Arrange
            var propostaId = Guid.NewGuid();
            var command = new AtualizarStatusPropostaCommand
            {
                PropostaId = propostaId,
                NovoStatus = StatusProposta.Aprovada
            };

            _mockRepository.Setup(r => r.ObterPorIdAsync(propostaId))
                          .ReturnsAsync((Proposta?)null);

            // Act & Assert
            var act = async () => await _service.AtualizarStatusPropostaAsync(command);
            await act.Should().ThrowAsync<InvalidOperationException>()
                     .WithMessage($"Proposta com ID {propostaId} não encontrada");
        }
    }
}

