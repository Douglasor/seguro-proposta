using PropostaService.Domain.Enums;

namespace PropostaService.Application.Commands
{
    public class AtualizarStatusPropostaCommand
    {
        public Guid PropostaId { get; set; }
        public StatusProposta NovoStatus { get; set; }
    }
}

