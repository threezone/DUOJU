using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate.Mapping.Attributes;
using Spring.Data.NHibernate;
using System.IO;
using System.Reflection;

namespace Duoju.DAO.SpringNhibernate
{
    public class ExtendedSessionFactoryObject : LocalSessionFactoryObject
    {
        public ExtendedSessionFactoryObject()
        { }

        public string[] MappingAssemblyNames
        {
            get;
            set;
        }

        protected override void PostProcessConfiguration(NHibernate.Cfg.Configuration config)
        {
            var stream = new MemoryStream();
            HbmSerializer.Default.Validate = true;

            foreach (var name in MappingAssemblyNames)
            {
                var asm = Assembly.Load(name);
                HbmSerializer.Default.Serialize(stream, asm);
                stream.Position = 0;
                config.AddInputStream(stream);
            }
            
            stream.Close();
        }
    }
}
