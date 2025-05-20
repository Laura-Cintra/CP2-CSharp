namespace Cp2Mottu.Domain.Interfaces
{
    public interface IRepositorio<T> where T : class
    {
        Task<List<T>> ObterTodos();

        Task<T> ObterPorId(int id);

        Task<T>Adicionar(T entity);

        Task<T> Atualizar(T entity);

        Task<bool> Remover(T entity);
    }
}
