namespace TodoApi.Services;

public interface IApiService<T>
{
  Task<IEnumerable<T>> GetAll();

  Task<IEnumerable<T>> GetAll(int id);

  Task<T?> Get(string id);

  Task Create(T item);

  Task Update(T item);

  Task Remove(string id);
}