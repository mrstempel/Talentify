using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KwIt.Project.Pattern.DAL.Context
{
    public class BaseContext : DbContext
    {
        public BaseContext() : base("DbContextConnection")
        {
        }

        public BaseContext(string connection)
            : base(connection)
        {

        }
    }
}
