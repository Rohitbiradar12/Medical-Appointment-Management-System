namespace UserManagementService.Repository
{
    public interface IRepository<K, T> where T : class
    {
        Task<T> Get(K key);
        Task<ICollection<T>> GetAll();
        Task<T> Add(T entity);
        Task<T> Update(T entity);
        Task<T> Delete(K key);
        Task<T> Get(string email);
    }
}
