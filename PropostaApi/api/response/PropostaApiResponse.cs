using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace application.dto
{
    public record PropostaApiResponse(Guid id, string cliente, string valor, string status, string dataCriacao);
}
