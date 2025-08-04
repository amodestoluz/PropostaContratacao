using application.dto;
using application.ports;
using Microsoft.AspNetCore.Mvc;
using proposta.mappers;
using proposta.request;


namespace proposta.controller
{
    [ApiController]
    [Route("api/Proposta")]
    public class PropostasController : ControllerBase
    {
        private readonly IPropostaService _propostaService;

        public PropostasController(IPropostaService propostaService)
        {
            _propostaService = propostaService;
        }


        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<PropostaApiResponse>> CriarProposta([FromBody] CriarPropostaApiRequest request)
        {
            try
            {
                var command = new CriarPropostaCommand(request.cliente, request.valor);
                var response = await _propostaService.CriarPropostaAsync(command);
                return CreatedAtAction(nameof(GetPropostaById), new { response.id }, response);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<PropostaApiResponse>> GetPropostaById(Guid id)
        {
            var response = await _propostaService.GetPropostaByIdAsync(id);
            if (response == null)
            {
                return NotFound();
            }
            return Ok(response.ToApiResponse());
        }

        [HttpGet] 
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<PropostaApiResponse>))]
        public async Task<ActionResult<IEnumerable<PropostaApiResponse>>> ListarPropostas()
        {
            var propostasResponse = await _propostaService.ListarPropostasAsync();
            return Ok(propostasResponse.Select(p => p.ToApiResponse()));
        }

        [HttpPatch("{idProposta}/status")] 
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> AlterarStatusProposta(Guid idProposta, [FromBody] AlterarStatusPropostaApiRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _propostaService.AlterarStatusPropostaAsync(new AlterarStatusPropostaCommand(idProposta, request.NovoStatus));
                return NoContent(); 
            }
            catch (ArgumentException ex)
            {
                return NotFound(new { message = ex.Message }); 
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message }); 
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao alterar status da proposta {idProposta}: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Ocorreu um erro interno ao processar a requisição." });
            }
        }

    }
}
