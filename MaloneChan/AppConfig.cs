using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MaloneChan {
    public class AppConfig {

        private static AppConfig _singleton = null;
        private AppConfig() {

        }
        public static AppConfig GetInstance() {
            if (_singleton == null) {
                _singleton = new AppConfig();
                _singleton.Load();
            }
            return _singleton;
        }
        public string ConfigPath {
            get {
                return System.IO.Path.ChangeExtension(System.Reflection.Assembly.GetExecutingAssembly().Location, ".conf");
            }
        }
        public void Load() {
            var doc = XDocument.Load(this.ConfigPath);
            var root = doc.Element("Configurations");
            this.ConnectionParameters.Clear();
            this.ConnectionParameters.AddRange(root.Element("Databases").Elements("Database").Select(x => DbConnectionParameter.CreateFromXElement(x)));
            this.EFCoreTestingParameter = EFCoreTestingParameter.CreateFromXElement(root.Element("EFCoreTesting"));
        }
        public IDbConnectionParameter CurrentConnectionParameter {
            get;
            set;
        }
        public List<IDbConnectionParameter> ConnectionParameters {
            get;
        } = new List<IDbConnectionParameter>();
        public EFCoreTestingParameter EFCoreTestingParameter {
            get;
            private set;
        }

        internal void Save() {
            var doc = new XDocument(
                new XElement("Configurations",
                    new XElement("Databases",
                        this.ConnectionParameters.Select(x => x.ToXElement()).ToArray()),
                    this.EFCoreTestingParameter.ToXElement()));
            doc.Save(this.ConfigPath);
        }
    }
}
