using application.dto;
using application.ports;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    public class ContratacaoController :  ControllerBase
    {
        private readonly IContratacaoPropostaService _contratacaoPropostaService;

        public ContratacaoController(IContratacaoPropostaService propostaService)
        {
            _contratacaoPropostaService = propostaService;
        }

        [HttpPost("{idProposta}")] 
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)] 
        public ActionResult<string> ProcessarProposta(Guid idProposta)
        {

            var command = new ContratarPropostaCommand(idProposta, DateTime.UtcNow);

            _contratacaoPropostaService.Contratar(command);
            return Ok($"Proposta com ID '{idProposta}' recebida com sucesso via POST!");
        }

    }
}
