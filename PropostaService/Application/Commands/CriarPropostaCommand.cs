namespace PropostaService.Application.Commands
{
    public class CriarPropostaCommand
    {
        public Guid ClienteId { get; set; }
        public decimal ValorProposta { get; set; }
    }
}

