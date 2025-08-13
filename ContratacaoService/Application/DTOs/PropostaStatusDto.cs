namespace ContratacaoService.Application.DTOs
{
    public class PropostaStatusDto
    {
        public Guid Id { get; set; }
        public int Status { get; set; } // 1 = EmAnalise, 2 = Aprovada, 3 = Rejeitada
        public bool PodeSerContratada => Status == 2; // Aprovada
    }
}

