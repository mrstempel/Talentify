using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KwIt.Project.Pattern.DAL.Library
{
    /// <summary>
    /// Interface for all unit of works
    /// </summary>
    /// <typeparam name="TContext">DbContext</typeparam>
    public interface IUnitOfWork<TContext> : IDisposable
        where TContext : DbContext
    {
        /// <summary>
        /// Commit changes to DbContext
        /// </summary>
        /// <returns></returns>
        int Save();
        /// <summary>
        /// DbContext
        /// </summary>
        TContext Context { get; }
    }
}
