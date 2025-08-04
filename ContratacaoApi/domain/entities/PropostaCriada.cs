using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace domain.entities
{
    public class PropostaCriada
    {
        public Guid Id { get; private set; }
        public string Cliente { get; private set; }
        public string Valor { get; private set; }
        public string Status { get; private set; }
        public string DataCriacao { get; private set; }
        private PropostaCriada(Guid id, string cliente, string valor, string status, string dataCriacao)
        {
            Id = id;
            Cliente = cliente;
            Valor = valor;
            Status = status;
            DataCriacao = dataCriacao;
        }


        public static PropostaCriada Reconstitute(Guid id, string cliente, string valor, string status, string dataCriacao)
        {
        
            return new PropostaCriada(id, cliente, valor, status, dataCriacao);
        }

        public bool PodeSerContratada()
        {
            if (Status != "Aprovada")
            {
                throw new ArgumentException($"A proposta não pode ser contratada. Status atual: {Status}. Apenas propostas Aprovadas podem ser contratadas.");
            }
            return true; 
        }

    }
}
