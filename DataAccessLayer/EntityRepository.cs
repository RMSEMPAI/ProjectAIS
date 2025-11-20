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
    public class EntityRepository<T> : IRepository<T> where T : class, IDomainObject, new()
    {
        private readonly ITEmployeeContext _context;

        public EntityRepository(ITEmployeeContext context)
        {
            _context = context;
        }


        public void Add(T entity)
        {
            entity.Id = ReadAll().Max(x => x.Id+1);
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
            var query = _context.Set<T>().AsQueryable();

            // Автоматически включаем Language для ITEmployee
            if (typeof(T) == typeof(ITEmployee))
            {
                query = query.Include(e => (e as ITEmployee).Language);
            }

            return query.AsNoTracking().ToList();
        }



        public T ReadById(int id)
        {
            var query = _context.Set<T>().AsQueryable();

            if (typeof(T) == typeof(ITEmployee))
            {
                query = query.Include(e => (e as ITEmployee).Language);
            }

            return query.AsNoTracking().FirstOrDefault(e => e.Id == id);
        }

        public void Update(T entity)
        {
            _context.Set<T>().Update(entity);
            _context.SaveChanges();
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}