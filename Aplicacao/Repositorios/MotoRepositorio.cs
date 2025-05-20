using Cp2Mottu.Application.DTOs.Moto;
using Cp2Mottu.Domain.Interfaces;
using Cp2Mottu.Domain.Persistence;
using Cp2Mottu.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Cp2Mottu.Aplicacao.Repositorios
{
    public class MotoRepositorio : IRepositorio<Moto>
    {

        private readonly AppDbContext _context;

        public MotoRepositorio(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Moto> Adicionar(Moto moto)
        {
            await _context.Motos.AddAsync(moto); // Adiciona a moto ao contexto
            await _context.SaveChangesAsync(); // Salva as alterações no banco de dados

            return moto;
        }

        public async Task<Moto> Atualizar(Moto moto)
        {
            _context.Motos.Update(moto);
            _context.Entry(moto).State = EntityState.Modified;
            await _context.SaveChangesAsync(); // Salva as alterações no banco de dados

            return moto; // Retorna a moto atualizada
        }

        public async Task<Moto> ObterPorId(int id) => 
            await _context.Motos.Include(m => m.Filial).FirstOrDefaultAsync(m => m.Id == id); // Obtém a moto pelo ID, incluindo a filial associada



        public async Task<List<Moto>> ObterTodos() => 
            await _context.Motos.Include(m => m.Filial).OrderBy(m => m.Id).ToListAsync(); // Obtém todas as motos, incluindo as filiais associadas


        public async Task<bool> Remover(Moto moto)
        {
            _context.Motos.Remove(moto);
            await _context.SaveChangesAsync();
            return true;
        }

    }
}
