using LogicLab;

namespace DataAccessLayer
{
    public interface IRepository<T> where T : IDomainObject
    {
        /// <summary>
        /// Добавить новую сущность
        /// </summary>
        /// <param name="entity">Добавляемая сущность</param>
        void Add(T entity);

        /// <summary>
        /// Удалить сущность по идентификатору
        /// </summary>
        /// <param name="id">Идентификатор сущности</param>
        /// <returns>True если удаление успешно, иначе False</returns>
        bool Delete(int id);

        /// <summary>
        /// Получить все сущности
        /// </summary>
        /// <returns>Коллекция всех сущностей</returns>
        IEnumerable<T> ReadAll();

        /// <summary>
        /// Получить сущность по идентификатору
        /// </summary>
        /// <param name="id">Идентификатор сущности</param>
        /// <returns>Найденная сущность или null</returns>
        T ReadById(int id);

        /// <summary>
        /// Обновить существующую сущность
        /// </summary>
        /// <param name="entity">Обновляемая сущность с новыми данными</param>
        void Update(T entity);

        /// <summary>
        /// Сохранить изменения (для EF контекста)
        /// </summary>
        void SaveChanges();
    }
}
