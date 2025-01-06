namespace TodoApi.Services;

public interface IApiService<T>
{
  Task<IEnumerable<T>> GetAll();

  Task<T?> Get(int id);

  Task Create(T item);

  Task Update(T item);

  Task Remove(int id);
}