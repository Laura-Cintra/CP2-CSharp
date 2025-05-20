namespace Cp2Mottu.Domain.Interfaces
{
    public interface IRepositorio<T> where T : class
    {
        Task<T> obterTodos();

        Task<T> obterPorId(int id);

        Task<T> adicionar(T entity);

        Task<T> atualizar(T entity);

        Task<T> remover(int id);
    }
}
