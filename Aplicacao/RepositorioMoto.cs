using Cp2Mottu.Application.DTOs.Moto;
using Cp2Mottu.Domain.Interfaces;
using Cp2Mottu.Domain.Persistence;
using Cp2Mottu.Infrastructure.Context;

namespace Cp2Mottu.Application
{
    public class RepositorioMoto : IRepositorio<Moto>
    {

        private readonly AppDbContext _context;

        public RepositorioMoto (AppDbContext context)
        {
            _context = context;
        }

        public Task<Moto> adicionar(MotoCreateDto motoDto)
        {
            
            var moto = new Moto(motoDto.Placa, motoDto.Modelo.ToString(), motoDto.IdFilial, filial); // Cria uma nova moto com os dados do DTO e a filial encontrada
            filial.Motos.Add(moto); // Adiciona a moto à lista de motos da filial
            await _context.Motos.AddAsync(moto); // Adiciona a moto ao contexto
            await _context.SaveChangesAsync(); // Salva as alterações no banco de dados
        }

        public Task<Moto> atualizar(Moto entity)
        {
            throw new NotImplementedException();
        }

        public Task<Moto> obterPorId(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Moto> obterTodos()
        {
            throw new NotImplementedException();
        }

        public Task<Moto> remover(int id)
        {
            throw new NotImplementedException();
        }
    }
}
