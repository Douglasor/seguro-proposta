using PropostaService.Domain.Enums;

namespace PropostaService.Application.DTOs
{
    public class PropostaDto
    {
        public Guid Id { get; set; }
        public Guid ClienteId { get; set; }
        public DateTime DataCriacao { get; set; }
        public decimal ValorProposta { get; set; }
        public StatusProposta Status { get; set; }
    }
}

