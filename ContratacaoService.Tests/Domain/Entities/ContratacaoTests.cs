using FluentAssertions;
using ContratacaoService.Domain.Entities;

namespace ContratacaoService.Tests.Domain.Entities
{
    public class ContratacaoTests
    {
        [Fact]
        public void Contratacao_DeveCriarComDadosValidos()
        {
            // Arrange
            var propostaId = Guid.NewGuid();

            // Act
            var contratacao = new Contratacao(propostaId);

            // Assert
            contratacao.Id.Should().NotBeEmpty();
            contratacao.PropostaId.Should().Be(propostaId);
            contratacao.DataContratacao.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
        }

        [Fact]
        public void Contratacao_DeveLancarExcecao_QuandoPropostaIdVazio()
        {
            // Arrange
            var propostaId = Guid.Empty;

            // Act & Assert
            var act = () => new Contratacao(propostaId);
            act.Should().Throw<ArgumentException>()
                .WithMessage("PropostaId não pode ser vazio*");
        }
    }
}

