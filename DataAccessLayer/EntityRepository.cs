using DataAccessLayer;
using LogicLab;
using Microsoft.EntityFrameworkCore;
using LogicLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Dapper.SqlMapper;

namespace LogicLibrary
{
    // EntityRepository - ИСПРАВЛЕННАЯ ВЕРСИЯ
    public class EntityRepository<T> : IRepository<T> where T : class, IDomainObject, new()
    {
        private readonly ITEmployeeContext _context;

        public EntityRepository(ITEmployeeContext context)
        {
            _context = context;
        }

        public void Add(T entity)
        {
            _context.Set<T>().Add(entity);
            SaveChanges();
        }

        public bool Delete(int id)
        {
            var entity = _context.Set<T>().Find(id);
            if (entity != null)
            {
                _context.Set<T>().Remove(entity);
                SaveChanges();
                return true;
            }
            return false;
        }

        public IEnumerable<T> ReadAll()
        {
            return _context.Set<T>().AsNoTracking().ToList(); // AsNoTracking для производительности
        }

        public T ReadById(int id)
        {
            return _context.Set<T>().AsNoTracking().FirstOrDefault(e => e.Id == id);
        }

        public void Update(T entity)
        {
            // Отслеживаем сущность перед обновлением
            var existing = _context.Set<T>().Find(entity.Id);
            if (existing != null)
            {
                _context.Entry(existing).CurrentValues.SetValues(entity);
                SaveChanges();
            }
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}