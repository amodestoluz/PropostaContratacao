using domain.entities.events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace application.usecases.interfaces
{
    public interface IContratacaoPropostaUseCase
    {
        Task Handle(ContratacaoPropostaEvent contratacaoPropostaEvent);
    }

}
