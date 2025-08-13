using FluentAssertions;
using PropostaService.Domain.Entities;
using PropostaService.Domain.Enums;

namespace PropostaService.Tests.Domain.Entities
{
    public class PropostaTests
    {
        [Fact]
        public void Proposta_DeveCriarComDadosValidos()
        {
            // Arrange
            var clienteId = Guid.NewGuid();
            var valorProposta = 1000m;

            // Act
            var proposta = new Proposta(clienteId, valorProposta);

            // Assert
            proposta.Id.Should().NotBeEmpty();
            proposta.ClienteId.Should().Be(clienteId);
            proposta.ValorProposta.Should().Be(valorProposta);
            proposta.Status.Should().Be(StatusProposta.EmAnalise);
            proposta.DataCriacao.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
        }

        [Fact]
        public void Proposta_DeveLancarExcecao_QuandoClienteIdVazio()
        {
            // Arrange
            var clienteId = Guid.Empty;
            var valorProposta = 1000m;

            // Act & Assert
            var act = () => new Proposta(clienteId, valorProposta);
            act.Should().Throw<ArgumentException>()
                .WithMessage("ClienteId não pode ser vazio*");
        }

        [Fact]
        public void Proposta_DeveLancarExcecao_QuandoValorPropostaMenorOuIgualZero()
        {
            // Arrange
            var clienteId = Guid.NewGuid();
            var valorProposta = 0m;

            // Act & Assert
            var act = () => new Proposta(clienteId, valorProposta);
            act.Should().Throw<ArgumentException>()
                .WithMessage("Valor da proposta deve ser maior que zero*");
        }

        [Fact]
        public void AtualizarStatus_DeveAtualizarStatus_QuandoStatusEmAnalise()
        {
            // Arrange
            var proposta = new Proposta(Guid.NewGuid(), 1000m);
            var novoStatus = StatusProposta.Aprovada;

            // Act
            proposta.AtualizarStatus(novoStatus);

            // Assert
            proposta.Status.Should().Be(novoStatus);
        }

        [Fact]
        public void AtualizarStatus_DeveLancarExcecao_QuandoStatusJaFinalizado()
        {
            // Arrange
            var proposta = new Proposta(Guid.NewGuid(), 1000m);
            proposta.AtualizarStatus(StatusProposta.Aprovada);

            // Act & Assert
            var act = () => proposta.AtualizarStatus(StatusProposta.Rejeitada);
            act.Should().Throw<InvalidOperationException>()
                .WithMessage("Não é possível alterar o status de uma proposta já finalizada");
        }

        [Theory]
        [InlineData(StatusProposta.Aprovada, true)]
        [InlineData(StatusProposta.EmAnalise, false)]
        [InlineData(StatusProposta.Rejeitada, false)]
        public void PodeSerContratada_DeveRetornarValorCorreto(StatusProposta status, bool esperado)
        {
            // Arrange
            var proposta = new Proposta(Guid.NewGuid(), 1000m);
            if (status != StatusProposta.EmAnalise)
                proposta.AtualizarStatus(status);

            // Act
            var resultado = proposta.PodeSerContratada();

            // Assert
            resultado.Should().Be(esperado);
        }
    }
}

