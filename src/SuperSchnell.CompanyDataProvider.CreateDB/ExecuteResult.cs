using System.Data.SqlClient;

namespace SuperSchnell.CompanyDataProvider.CreateDB
{
    internal class ExecuteResult
    {
        public ExecuteResult()
        {
        }

        public ExecuteResult(SqlException sqlException, string line)
        {
            SqlException = sqlException;
            Line = line;
            Failed = true;
        }

        public bool Failed { get; private set; }
        public SqlException SqlException { get; private set; }
        public string Line { get; private set; }
    }
}