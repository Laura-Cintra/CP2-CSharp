using Cp2Mottu.Application.DTOs.Moto;
using Cp2Mottu.Domain.Interfaces;
using Cp2Mottu.Domain.Persistence;
using Cp2Mottu.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Cp2Mottu.Application
{
    public class FilialRepositorio : IRepositorio<Filial>
    {

        private readonly AppDbContext _context;

        public FilialRepositorio(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Filial> Adicionar(Filial filial)
        {
            await _context.Filiais.AddAsync(filial); // Adiciona a moto ao contexto
            await _context.SaveChangesAsync(); // Salva as alterações no banco de dados

            return filial;
        }

        public async Task<Filial> Atualizar(Filial filial)
        {
            _context.Filiais.Update(filial);
            _context.Entry(filial).State = EntityState.Modified; // Marca a entidade como modificada
            await _context.SaveChangesAsync(); // Salva as alterações no banco de dados

            return filial; // Retorna a moto atualizada
        }

        public async Task<Filial> ObterPorId(int id) => 
            await _context.Filiais.Include(f => f.Motos).FirstOrDefaultAsync(f => f.Id == id); // Obtém a moto pelo ID, incluindo a filial associada



        public async Task<List<Filial>> ObterTodos() => 
            await _context.Filiais.OrderBy(f => f.Id).ToListAsync(); // Obtém todas as motos, incluindo as filiais associadas


        public async Task<bool> Remover(Filial filial)
        {
            _context.Filiais.Remove(filial);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
