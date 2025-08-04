using domain.entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace application.ports
{

    public interface IPropostaExternalService
    {
        Task<PropostaCriada> GetPropostaByIdAsync(Guid id);

    }
}
