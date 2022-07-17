using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
namespace MaloneChan {
    public interface IDbConnectionParameter {
        string Name {
            get;
            set;
        }
        string OrgName {
            get;
        }
        string ConnectionString {
            get;
        }
        int? Port {
            get;
            set;
        }
        string ServerName {
            get;
            set;
        }
        string DatabaseName {
            get;
            set;
        }
        string UserName {
            get;
            set;
        }
        string Password {
            get;
            set;
        }
        bool UseSsh {
            get;
            set;
        }
        ISecureshellParameter SshParameter {
            get;
            set;
        }
        public XElement ToXElement();
        void Validate();
        void Connect();
    }
}
