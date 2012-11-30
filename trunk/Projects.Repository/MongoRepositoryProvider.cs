using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Projects.Accesses.MongoAccess;
using Projects.Tool.Reflection;
using Projects.Accesses.MongoAccess;


namespace Projects.Repository
{
    public class MongoRepositoryProvider : DefaultMongoProvider
    {
        public static void Init(IEnumerable<string> assemblyStrings)
        {
            var mappings = new List<Type>();
            var baseType = typeof(MongoMap);
            foreach (var assemblyString in assemblyStrings)
            {
                try
                {
                    var assembly = Assembly.Load(assemblyString);
                    if (!assembly.IsDynamic)
                    {
                        mappings.AddRange(
                            assembly.GetExportedTypes()
                                .Where(t => t.IsClass && !t.IsAbstract && t.IsSubclassOf(baseType))
                                .ToList()
                            );
                    }
                }
                catch (System.IO.FileNotFoundException ex)
                {
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            foreach (var type in mappings)
            {
                FastActivator.Create(type);
            }
        }
    }
}
