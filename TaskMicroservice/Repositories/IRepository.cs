namespace TaskMicroservice.Repositories;

public interface IRepository<T>  where T : class
{
    IEnumerable<T> GetAll();
    
    void Add(T entity);
    
    int Update(T entity);
    
    int Delete(T entity);
}