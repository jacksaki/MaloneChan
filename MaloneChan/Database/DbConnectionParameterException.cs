using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaloneChan {
    public class DbConnectionParameterException:ApplicationException {
        public DbConnectionParameterException() :base() {
        }
        public DbConnectionParameterException(string message) : base(message) {
        }
        public DbConnectionParameterException(string message,Exception innerException) : base(message,innerException) { 
        }
    }
}
