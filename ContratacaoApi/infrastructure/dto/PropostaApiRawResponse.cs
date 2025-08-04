using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace infrastructure.dto
{
    public class PropostaApiRawResponse
    {
        public Guid Id { get; set; }
        public string Cliente { get; set; }
        public string Valor { get; set; }
        public string Status { get; set; } 
        public string DataCriacao { get; set; }
    }
}
