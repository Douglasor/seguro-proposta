using System.Text.Json;
using ContratacaoService.Application.DTOs;
using ContratacaoService.Domain.Interfaces;

namespace ContratacaoService.Infrastructure.Gateways
{
    public class PropostaServiceHttpGateway : IPropostaServiceGateway
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public PropostaServiceHttpGateway(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public async Task<PropostaStatusDto?> VerificarStatusPropostaAsync(Guid propostaId)
        {
            try
            {
                var propostaServiceUrl = _configuration["PropostaService:BaseUrl"];
                var response = await _httpClient.GetAsync($"{propostaServiceUrl}/api/propostas/{propostaId}");

                if (!response.IsSuccessStatusCode)
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                        return null;
                    
                    throw new HttpRequestException($"Erro ao consultar PropostaService: {response.StatusCode}");
                }

                var content = await response.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                var proposta = JsonSerializer.Deserialize<PropostaStatusDto>(content, options);
                return proposta;
            }
            catch (HttpRequestException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Erro ao comunicar com PropostaService", ex);
            }
        }
    }
}

