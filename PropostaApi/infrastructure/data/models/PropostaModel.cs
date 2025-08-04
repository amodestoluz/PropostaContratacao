using domain.@enum;
using System.ComponentModel.DataAnnotations;


namespace infrastructure.data.models
{
    public class PropostaModel
    {
        [Key]
        public Guid Id { get; set; }
        [StringLength(100)]
        [Required]
        public string Cliente { get; set; }
        [Required]
        public decimal Valor { get; set; }
        [Required]
        public StatusPropostaEnum Status { get; set; }
        [Required]
        public DateTime DataCriacao { get; set; }
        public DateTime? DataContratacao { get; set; }

        public PropostaModel() { }

        public PropostaModel(Guid id, string cliente, decimal valor, StatusPropostaEnum status, DateTime dataCriacao, DateTime? dataContratacao)
        {
            this.Id = id;
            this.Cliente = cliente;
            this.Valor = valor;
            this.Status = status;
            this.DataCriacao = dataCriacao;
            this.DataContratacao = dataContratacao;
        }
    }
}
