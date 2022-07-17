using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Livet;
using Npgsql;
namespace MaloneChan {
    public class DbConnectionParameter : NotificationObject, IDbConnectionParameter {
        public static DbConnectionParameter CreateFromXElement(XElement element) {
            var p = new DbConnectionParameter();
            p.UserName = element.Attribute("UserName")?.Value;
            p.ServerName = element.Attribute("ServerName")?.Value;
            p.DatabaseName = element.Attribute("DatabaseName")?.Value;
            p.Name = element.Attribute("Name")?.Value;
            p.OrgName = p.Name;
            p.Password = element.Attribute("Password")?.Value.Decrypt();
            if (element.Element("SshParameter") != null) {
                p.UseSsh = "Y".Equals(element.Attribute("UseSsh")?.Value, StringComparison.OrdinalIgnoreCase);
                p.SshParameter = SecureShellParameter.CreateFromXElement(element.Element("SshParameter"));
            } else {
                p.Port = element.Attribute("Port")?.Value?.ToIntN() ?? 5432;
            }
            return p;
        }
        public string OrgName {
            get;
            private set;
        } = "";
        public XElement ToXElement() {
            var element = new XElement("Database",
                new XAttribute("Name", this.Name),
                new XAttribute("UserName", this.UserName),
                new XAttribute("DatabaseName", this.DatabaseName),
                new XAttribute("Password", this.Password.Encrypt())
            );
            if (this.UseSsh) {
                element.Add("SshParameter", this.SshParameter.ToXElement());
            } else {
                element.Add(new XAttribute("Port", this.Port));
            }
            return element;
        }

        public void Validate() {
            if(string.IsNullOrEmpty(this.Name)) {
                throw new DbConnectionParameterException("Name を入力してください"); ;
            }
            if (string.IsNullOrEmpty(this.ServerName)) {
                throw new DbConnectionParameterException("ServerName を入力してください"); ;
            }
            if (string.IsNullOrEmpty(this.DatabaseName)) {
                throw new DbConnectionParameterException("Database を入力してください"); ;
            }
            if (string.IsNullOrEmpty(this.UserName)) {
                throw new DbConnectionParameterException("Username を入力してください"); ;
            }
            if (string.IsNullOrEmpty(this.Password)) {
                throw new DbConnectionParameterException("Password を入力してください"); ;
            }
            if (!this.UseSsh && !this.Port.HasValue) {
                throw new DbConnectionParameterException("Port を入力してください"); ;
            } else {
                this.SshParameter.Validate();
            }
        }

        public void Connect() {
            using(var q = new PgQuery(this)) {

            }
        }

        public string ConnectionString {
            get {
                var sb = new System.Text.StringBuilder();
                sb.Append("Host=").Append(this.ServerName).Append(";")
                  .Append("Database=").Append(this.DatabaseName).Append(";")
                  .Append("Username=").Append(this.UserName).Append(";")
                  .Append("Password").Append(this.Password).Append(";");
                if (!this.UseSsh) {
                    sb.Append("Port=").Append(this.Port).Append(";");
                }
                return sb.ToString();
            }
        }

        private bool _UseSsh;

        public bool UseSsh {
            get {
                return _UseSsh;
            }
            set { 
                if (_UseSsh == value) {
                    return;
                }
                _UseSsh = value;
                if (value) {
                    this.Port = null;
                }
                RaisePropertyChanged();
            }
        }

        public ISecureshellParameter SshParameter {
            get;
            set;
        }


        private string _Name = "";

        public string Name {
            get {
                return _Name;
            }
            set { 
                if (_Name == value) {
                    return;
                }
                _Name = value;
                RaisePropertyChanged();
            }
        }


        private string _ServerName = "";

        public string ServerName {
            get {
                return _ServerName;
            }
            set { 
                if (_ServerName == value) {
                    return;
                }
                _ServerName = value;
                RaisePropertyChanged();
            }
        }


        private int? _Port;

        public int? Port {
            get {
                return _Port;
            }
            set { 
                if (_Port == value) {
                    return;
                }
                _Port = value;
                RaisePropertyChanged();
            }
        }


        private string _DatabaseName = "";

        public string DatabaseName {
            get {
                return _DatabaseName;
            }
            set { 
                if (_DatabaseName == value) {
                    return;
                }
                _DatabaseName = value;
                RaisePropertyChanged();
            }
        }

        private string _UserName = "";

        public string UserName {
            get {
                return _UserName;
            }
            set { 
                if (_UserName == value) {
                    return;
                }
                _UserName = value;
                RaisePropertyChanged();
            }
        }
        private string _Password = "";

        public string Password {
            get {
                return _Password;
            }
            set { 
                if (_Password == value) {
                    return;
                }
                _Password = value;
                RaisePropertyChanged();
            }
        }
    }
}
