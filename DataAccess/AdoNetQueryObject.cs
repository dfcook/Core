using DanielCook.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Data;

namespace DanielCook.Core.DataAccess
{
    public abstract class AdoNetQueryObject : IQueryObject
    {
        private const int DefaultTimeout = 5000;

        protected abstract IDbConnection GetConnection();

        protected abstract IDbDataParameter CreateParameter();

        protected IDictionary<string, IDbDataParameter> Parameters { get; private set; }

        protected string ConnectionString { get; private set; }
        protected CommandType CommandType { get; private set; }
        protected string CommandText { get; private set; }

        private int Timeout { get; set; }

        protected AdoNetQueryObject(string connectionString, string commandText, CommandType commandType, int timeout)
            : this(connectionString, commandText, commandType)
        {
            Timeout = timeout;
        }

        protected AdoNetQueryObject(string connectionString, string commandText, CommandType commandType)
        {
            Timeout = DefaultTimeout;
            ConnectionString = connectionString;
            CommandText = commandText;
            CommandType = commandType;
            Parameters = new Dictionary<string, IDbDataParameter>();
        }

        public IQueryObject AddInputOutputParameter<T>(string parameterName, T value)
        {
            var parameter = CreateParameter();

            parameter.Direction = ParameterDirection.InputOutput;
            parameter.ParameterName = parameterName;
            parameter.DbType = GetDbType(typeof(T));

            Parameters.Add(parameterName, parameter);

            return this;
        }

        public IQueryObject AddOutputParameter<T>(string parameterName)
        {
            var parameter = CreateParameter();

            parameter.Direction = ParameterDirection.Output;
            parameter.ParameterName = parameterName;
            parameter.DbType = GetDbType(typeof(T));

            Parameters.Add(parameterName, parameter);

            return this;
        }

        public IQueryObject AddParameter<T>(string parameterName, T value)
        {
            var parameter = CreateParameter();

            parameter.Direction = ParameterDirection.Input;
            parameter.ParameterName = parameterName;
            parameter.DbType = GetDbType(typeof(T));
            parameter.Value = GetDbValue(value);

            Parameters.Add(parameterName, parameter);

            return this;
        }

        public abstract IQueryObject AddTableParameter<T>(string parameterName, DataTable table);

        public int Execute()
        {
            using (var cn = GetConnection())
            {
                using (var cmd = GetCommand(cn))
                {
                    cn.Open();
                    return cmd.ExecuteNonQuery();
                }
            }
        }

        public ICollection<T> ExecuteList<T>() where T : new()
        {
            using (var reader = ExecuteReader())
                return reader.MapList<T>();
        }

        public ICollection<T> ExecuteList<T>(IObjectMapper<T> mapper)
        {
            using (var reader = ExecuteReader())
                return reader.MapList(mapper);
        }

        public T ExecuteObject<T>() where T : new()
        {
            using (var reader = ExecuteReader())
                return reader.Read() ? reader.Map<T>() : default(T);
        }

        public T ExecuteObject<T>(IObjectMapper<T> mapper)
        {
            using (var reader = ExecuteReader())
                return reader.Read() ? reader.Map(mapper) : default(T);
        }

        public T ExecuteScalar<T>()
        {
            using (var reader = ExecuteReader())
                return reader.Read() ? reader.Read<T>(0) : default(T);
        }

        public T GetParameterValue<T>(string parameterName)
        {
            var value = Parameters[parameterName].Value;
            return value == DBNull.Value ? default(T) : (T)value;
        }

        private static object GetDbValue(object o)
        {
            return o ?? DBNull.Value;
        }

        private static DbType GetDbType(Type t)
        {
            if (t.IsGenericType)
                return GetDbType(t.GetGenericArguments()[0]);

            if (t == typeof(DBNull))
                return DbType.String;

            if (t.IsEnum)
                return DbType.Int32;

            try
            {
                return (DbType)Enum.Parse(typeof(DbType), t.Name);
            }
            catch
            {
                throw new Exception("Unknown type: " + t.Name);
            }
        }

        private IDbCommand GetCommand(IDbConnection connection)
        {
            var cmd = connection.CreateCommand();

            cmd.CommandText = CommandText;
            cmd.CommandType = CommandType;
            cmd.CommandTimeout = Timeout;

            return AddCommandParameters(cmd);
        }

        private IDbCommand AddCommandParameters(IDbCommand cmd)
        {
            Parameters.Each(x => cmd.Parameters.Add(x.Value));
            return cmd;
        }

        protected IDataReader ExecuteReader()
        {
            var cn = GetConnection();
            var cmd = GetCommand(cn);

            cn.Open();
            return cmd.ExecuteReader(CommandBehavior.CloseConnection);
        }
    }
}
