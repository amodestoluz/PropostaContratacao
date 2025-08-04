using application.ports;
using domain.entities;
using Microsoft.EntityFrameworkCore;
using infrastructure.data;
using infrastructure.data.mappers;

namespace infrastructure.adapters.data
{
    public class PropostaRepository : IPropostaRepository
    {
        private readonly ApplicationDbContext _context;

        public PropostaRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Proposta proposta)
        {
           await _context.Propostas.AddAsync(proposta.ToPropostaModel());
           await _context.SaveChangesAsync(); 
        }

        public async Task<IEnumerable<Proposta>> GetAllAsync()
        {
            var propostaModels = await _context.Propostas.ToListAsync();
            return propostaModels.ToDomainEntity();
        }

        public async Task<Proposta> GetByIdAsync(Guid id)
        {
            var propostaModel = await _context.Propostas.FindAsync(id);
            return propostaModel.ToDomainEntity();
        }

        public async Task UpdateAsync(Proposta proposta)
        {
          
            var existingPropostaModel = await _context.Propostas.FindAsync(proposta.Id);

            if (existingPropostaModel == null)
            {
                throw new InvalidOperationException($"Proposta com ID '{proposta.Id}' não encontrada para atualização.");
            }

            existingPropostaModel.Cliente = proposta.Cliente;
            existingPropostaModel.Valor = proposta.Valor;
            existingPropostaModel.Status = proposta.Status;
            existingPropostaModel.DataCriacao = proposta.DataCriacao;
            existingPropostaModel.DataContratacao = proposta.DataContratacao;

            _context.Entry(existingPropostaModel).State = EntityState.Modified;

            await _context.SaveChangesAsync();
        }
    }
}
