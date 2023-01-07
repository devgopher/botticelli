namespace Botticelli.DataLayer.Interfaces
{
    public interface IStorage<T, in TId>
    where T : class
    {
        public T Get(TId id);
        public ICollection<T> Get(Func<T, bool> filter);
        public void Add(params T[] entites);
        public void Remove(TId id);
        public void Remove(T entity);
        public void Update(params T[] entities);
    }
}