using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KwIt.Project.Pattern.DAL.Models
{
    [Serializable]
    public class BaseEntity
    {
        /// <summary>
        /// Unique Identity
        /// </summary>
        public virtual int Id { get; set; }
    }
}
