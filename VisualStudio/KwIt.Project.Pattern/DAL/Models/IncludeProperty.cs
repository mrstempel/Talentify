using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KwIt.Project.Pattern.DAL.Library;

namespace KwIt.Project.Pattern.DAL.Models
{
    public class IncludeProperty : IIncludeProperty
    {
        public string Name { get; set; }
        public ICollection<IIncludeProperty> Children { get; set; }
    }
}
