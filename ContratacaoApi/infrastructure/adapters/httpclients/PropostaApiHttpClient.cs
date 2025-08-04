using application.ports;
using domain.entities;
using infrastructure.dto;
using System.Text.Json;
using Microsoft.Extensions.Configuration;

namespace infrastructure.adapters.httpclients
{
    public class PropostaApiHttpClient : IPropostaExternalService
    {
        private readonly HttpClient _httpClient;
        public PropostaApiHttpClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<PropostaCriada> GetPropostaByIdAsync(Guid id)
    {
        try
        {
                var response = await _httpClient.GetAsync($"/api/Proposta/{id}");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var rawProposta = JsonSerializer.Deserialize<PropostaApiRawResponse>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                return PropostaCriada.Reconstitute(rawProposta.Id,
                    rawProposta.Cliente, 
                    rawProposta.Valor,
                    rawProposta.Status,
                    rawProposta.DataCriacao);
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return null; 
            }
            else
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException($"Erro ao buscar proposta na PropostaApi. Status: {response.StatusCode}, Conteúdo: {errorContent}");
            }
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"Erro de requisição HTTP para PropostaApi (Get): {ex.Message}");
            throw;
        }
        catch (JsonException ex)
        {
            Console.WriteLine($"Erro de desserialização JSON da PropostaApi: {ex.Message}");
            throw;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro inesperado ao buscar proposta na PropostaApi (Get): {ex.Message}");
            throw;
        }
    }
}
}
