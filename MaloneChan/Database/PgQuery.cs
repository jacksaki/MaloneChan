using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Threading.Tasks;
using Npgsql;
namespace MaloneChan {
    public class PgQuery : DbQueryBase {
        private static IDbConnectionParameter _parameter;
        public PgQuery(IDbConnectionParameter parameter) : base(parameter) {
        }
        public PgQuery() : base(_parameter) {
        }

        public static void SetParameter(IDbConnectionParameter parameter) {
            using(var query = new PgQuery(parameter)) {
                _parameter = parameter;
            }
        }
        protected override IDbConnection GenerateConnection(string connectionString) {
            return new NpgsqlConnection(connectionString);
        }
        protected override IDbDataParameter CreateParameter(string name, object value) {
            return new NpgsqlParameter(name, value);
        }

    }
}