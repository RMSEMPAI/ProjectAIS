using DataAccessLayer;
using LogicLab;
using LogicLib;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Dapper.SqlMapper;

namespace LogicLibrary
{
    public class EntityRepository<T> : IRepository<T> where T : class, IDomainObject, IComparable<T>, new()
    {
        private ITEmployee _context;

        public EntityRepository(ITEmployee context)
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
            var entity = ReadById(id);
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
            return _context.Set<T>().AsNoTracking().ToList();
        }

        public T ReadById(int id)
        {
            var entity = _context.Set<T>().Find(id);
            if (entity != null)
            {
                _context.Entry(entity).State = EntityState.Detached;
            }
            return entity;
        }

        public void Update(T entity)
        {
            _context.Set<T>().Update(entity);
            SaveChanges();
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}