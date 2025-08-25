using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Linq.Expressions;
using System.Threading;
using workshop.wwwapi.Data;
using workshop.wwwapi.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace workshop.wwwapi.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private DatabaseContext _db;
        private DbSet<T> _table = null!;

        public Repository(DatabaseContext dataContext)
        {
            _db = dataContext;
            _table = _db.Set<T>();
        }

        public async Task<IEnumerable<T>> Get()
        {
            return await _table.ToListAsync();
        }

        public async Task<T> Insert(T entity)
        {
            await _table.AddAsync(entity);
            await _db.SaveChangesAsync();
            return entity;
        }
        public async Task<T> Update(T entity)
        {
            _table.Attach(entity);
            _db.Entry(entity).State = EntityState.Modified;
            await _db.SaveChangesAsync();
            return entity;
        }

        public async Task<T?> Delete(object id)
        {
            T? entity = await _table.FindAsync(id);
            if (entity == null) return null;
            _table.Remove(entity);
            await _db.SaveChangesAsync();
            return entity;
        }

        public async Task<T?> Delete(T entity)
        {
            _table.Remove(entity);
            await _db.SaveChangesAsync();
            return entity;
        }

        public async Task<T?> GetById(int id)
        {
            return await _table.FindAsync(id);
        }

        public async Task<T?> GetById(int id1, int id2, Func<IQueryable<T>, IQueryable<T>> includeQuery)
        {
            IQueryable<T> query = _table.Where(e => EF.Property<int>(e, "PatientId") == id1 && EF.Property<int>(e, "DoctorId") == id2);
            query = includeQuery(query);
            return await query.FirstOrDefaultAsync();
        }

        public async Task<T?> GetById(int id, Func<IQueryable<T>, IQueryable<T>> includeQuery)
        {
            IQueryable<T> query = _table.Where(e => EF.Property<int>(e, "Id") == id);
            query = includeQuery(query);
            return await query.FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<T>> GetWithIncludes(Func<IQueryable<T>, IQueryable<T>> includeQuery)
        {
            IQueryable<T> query = includeQuery(_table);
            return await query.ToListAsync();
        }

        public async Task Save()
        {
            await _db.SaveChangesAsync();
        }
        //public async Task<IEnumerable<Patient>> GetPatients()
        //{
        //    return await _databaseContext.Patients.ToListAsync();
        //}
        //public async Task<Patient?> GetPatientById(int id)
        //{
        //    return await _databaseContext.Patients.Where(patient => patient.Id == id).FirstOrDefaultAsync();
        //}
        //public Task<Patient?> CreatePatient()
        //{
        //    throw new NotImplementedException();
        //}
        //public async Task<IEnumerable<Doctor>> GetDoctors()
        //{
        //    return await _databaseContext.Doctors.ToListAsync();
        //}
        //public async Task<IEnumerable<Appointment>> GetAppointmentsByDoctor(int id)
        //{
        //    return await _databaseContext.Appointments.Where(a => a.DoctorId==id).ToListAsync();
        //}
    }
}