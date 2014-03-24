using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KwIt.Project.Pattern.DAL.Context;
using KwIt.Project.Pattern.DAL.Library;

namespace KwIt.Project.Pattern.DAL.UnitOfWork
{
    public class GenericUnitOfWork<TContext> : IUnitOfWork<TContext>
        where TContext : BaseContext, new()
    {
        public GenericUnitOfWork()
        {
            _context = new TContext();
        }

        public GenericUnitOfWork(TContext context)
        {
            _context = context;
        }

        private bool _disposed;

        protected TContext _context;
        public TContext Context
        {
            get { return _context; }
        }

        public int Save()
        {
            return _context.SaveChanges();
        }

        public void Dispose(bool disposing)
        {
            if (!_disposed)
                if (disposing)
                    _context.Dispose();

            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
