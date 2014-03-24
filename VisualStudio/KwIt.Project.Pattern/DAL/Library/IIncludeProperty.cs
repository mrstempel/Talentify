using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KwIt.Project.Pattern.DAL.Library
{
    public interface IIncludeProperty
    {
        string Name { get; set; }
        ICollection<IIncludeProperty> Children { get; set; }
    }
}
