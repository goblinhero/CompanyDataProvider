using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace SuperSchnell.CompanyDataProvider.CreateDB
{
    internal class Runner
    {
        private readonly Assembly _assembly;
        private readonly string _connectionString;
        private readonly string[] _names;
        private readonly string _filePath;

        public Runner()
        {
            _filePath = ConfigurationManager.AppSettings["FilePath"];
            if (!Directory.Exists(_filePath))
                Directory.CreateDirectory(_filePath);
            _assembly = Assembly.GetExecutingAssembly();
            _connectionString = ConfigurationManager.ConnectionStrings["Runner"].ConnectionString;
            _names = _assembly.GetManifestResourceNames()
                .Where(n => n.EndsWith(".sql", StringComparison.InvariantCultureIgnoreCase))
                .ToArray();
        }

        public void Run()
        {
            foreach (var name in _names)
            {
                string sql;
                using (var stream = _assembly.GetManifestResourceStream(name))
                {
                    if (stream == null) continue;
                    var reader = new StreamReader(stream);
                    sql = reader.ReadToEnd();
                }

                ExecuteResult result;
                try
                {
                    using (var connection = new SqlConnection(_connectionString))
                    {
                        connection.Open();
                        Console.WriteLine("Running: {0}", name);
                        result = ExecuteSql(connection, string.Format(sql,_filePath));
                        connection.Close();
                    }
                }
                catch (Exception ex)
                {
                    var builder = new StringBuilder();
                    ReadException(ex, builder);
                    Console.WriteLine(builder);
                    return;
                }

                if (result.Failed)
                {
                    var builder = new StringBuilder();
                    builder.AppendLine(result.Line);
                    ReadException(result.SqlException, builder);
                    Console.WriteLine(builder);
                }
            }
            Console.WriteLine("Done running {0} scripts", _names.Length);
        }

        private ExecuteResult ExecuteSql(SqlConnection connection, string sql)
        {
            var regex = new Regex("^GO", RegexOptions.IgnoreCase | RegexOptions.Multiline);
            string[] lines = regex.Split(sql);

            using (var cmd = connection.CreateCommand())
            {
                foreach (string line in lines.Where(line => line.Length > 0))
                {
                    cmd.CommandText = line;
                    cmd.CommandType = CommandType.Text;
                    try
                    {
                        cmd.ExecuteNonQuery();
                    }
                    catch (SqlException ex)
                    {
                        return new ExecuteResult(ex, line);
                    }
                }
            }
            return new ExecuteResult();
        }

        public void ReadException(Exception ex, StringBuilder builder)
        {
            if (ex == null) return;
            builder.AppendLine(ex.Message);
            ReadException(ex.InnerException, builder);
        }
    }
}