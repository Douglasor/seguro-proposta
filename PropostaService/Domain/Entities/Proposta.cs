using PropostaService.Domain.Enums;

namespace PropostaService.Domain.Entities
{
    public class Proposta
    {
        public Guid Id { get; private set; }
        public Guid ClienteId { get; private set; }
        public DateTime DataCriacao { get; private set; }
        public decimal ValorProposta { get; private set; }
        public StatusProposta Status { get; private set; }

        // Construtor privado para EF Core
        private Proposta() { }

        public Proposta(Guid clienteId, decimal valorProposta)
        {
            if (clienteId == Guid.Empty)
                throw new ArgumentException("ClienteId não pode ser vazio", nameof(clienteId));
            
            if (valorProposta <= 0)
                throw new ArgumentException("Valor da proposta deve ser maior que zero", nameof(valorProposta));

            Id = Guid.NewGuid();
            ClienteId = clienteId;
            ValorProposta = valorProposta;
            DataCriacao = DateTime.UtcNow;
            Status = StatusProposta.EmAnalise;
        }

        public void AtualizarStatus(StatusProposta novoStatus)
        {
            if (Status == StatusProposta.Aprovada || Status == StatusProposta.Rejeitada)
                throw new InvalidOperationException("Não é possível alterar o status de uma proposta já finalizada");

            Status = novoStatus;
        }

        public bool PodeSerContratada()
        {
            return Status == StatusProposta.Aprovada;
        }
    }
}

