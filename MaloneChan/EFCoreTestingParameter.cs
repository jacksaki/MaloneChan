using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Xml.Linq;
using Npgsql.EntityFrameworkCore.PostgreSQL;
using Microsoft.EntityFrameworkCore;
namespace MaloneChan {
    public class EFCoreTestingParameter {
        public XElement ToXElement() {
            return new XElement("EFCoreTesting",
                new XElement("Imports",this.Imports.Select(x => new XElement("Import", x)).ToArray()),
                new XElement("References", this.References.Select(x => new XElement("Reference", x.GetName())).ToArray()),
                new XElement("ExternalAssemblies", this.ExternalAssemblies.Select(x => new XElement("ExternalAssembly", x.Location)).ToArray()),
                new XElement("SourceFiles",this.SourceFiles.Select(x=>new XElement("SourceFile",x)).ToArray())
                );
        }
        public static EFCoreTestingParameter CreateFromXElement(XElement element) {
            var p = new EFCoreTestingParameter();
            foreach(var e in element.Element("Imports")?.Elements("Import")) {
                p.Imports.Add(e.Value);
            }
            foreach (var e in element.Element("References")?.Elements("Reference")) {
                var asm = GetAssemblyFromName(e.Value);
                if (asm != null) {
                    p.References.Add(asm);
                }
            }
            foreach (var e in element.Element("ExternalAssemblies")?.Elements("ExternalAssembly").Where(x=>System.IO.File.Exists(x.Value))) {
                var asm = GetAssemblyFromFile(e.Value);
                if (asm != null) {
                    p.ExternalAssemblies.Add(asm);
                }
            }
            foreach (var e in element.Element("SourceFiles")?.Elements("SourceFile").Where(x => System.IO.File.Exists(x.Value))) {
                p.SourceFiles.Add(e.Value);
            }
            return p;
        }

        private static Assembly GetAssemblyFromName(string name) {
            try {
                return Assembly.Load(name);
            } catch {
                return null;
            }
        }
        private static Assembly GetAssemblyFromFile(string name) {
            try {
                return Assembly.Load(name);
            } catch {
                return null;
            }
        }
        public ObservableCollection<string> SourceFiles {
            get;
        } = new ObservableCollection<string>();
        public ObservableCollection<string> Imports {
            get;
        } = new ObservableCollection<string>();
        public ObservableCollection<Assembly> References {
            get;
        } = new ObservableCollection<Assembly>();
        public ObservableCollection<Assembly> ExternalAssemblies {
            get;
        } = new ObservableCollection<Assembly>();
    }
}
