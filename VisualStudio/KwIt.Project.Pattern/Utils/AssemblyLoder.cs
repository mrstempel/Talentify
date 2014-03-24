using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace KwIt.Project.Pattern.Utils
{
    public static class AssemblyLoader
    {
        private static ICollection<Assembly> _AllAssemblies;
        private static ICollection<Assembly> AllAssemblies
        {
            get
            {
                if (_AllAssemblies == null)
                {
                    _AllAssemblies = new List<Assembly>();
                    var applicationPath =
                        //string.IsNullOrEmpty(ConfigurationManager.AppSettings["Application.Path"]) ? 
                        Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase).TrimStart(("file:\\").ToCharArray());
                        //ConfigurationManager.AppSettings["Application.Path"] + "\\bin";
                    
                    foreach (string dll in Directory.GetFiles(applicationPath, "Ruckzuck*.dll"))
                        _AllAssemblies.Add(Assembly.LoadFile(dll));
                }
                return _AllAssemblies;
            }

        }

        public static IEnumerable<Type> GetTypes(Type type, string assemblyName = null)
        {
            var query = from x in AllAssemblies
                        select x;

            if (assemblyName != null)
                query = from q in query
                        where q.FullName.Equals(assemblyName)
                        select q;

            var result = from q in query
                         from t in q.GetTypes()
                         where t.IsSubclassOf(type)
                         select t;

            return result.ToList();
        }
    }
}
