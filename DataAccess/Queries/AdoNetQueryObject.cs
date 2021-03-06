﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using DanielCook.Core.Extensions;
using DanielCook.Core.Functional;

namespace DanielCook.Core.DataAccess
{
    public abstract class AdoNetQueryObject : IQueryObject
    {
        private const int DefaultTimeout = 5000;

        protected abstract IDbConnection GetConnection();

        protected abstract IDbDataParameter CreateParameter();

        protected IDictionary<string, IDbDataParameter> Parameters { get; }

        protected string ConnectionString { get; }
        protected CommandType CommandType { get; }
        protected string CommandText { get; }

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
            parameter.Value = GetDbValue(value);

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

        public IQueryObject AddOutputParameter<T>(string parameterName, int size)
        {
            var parameter = CreateParameter();

            parameter.Direction = ParameterDirection.Output;
            parameter.ParameterName = parameterName;
            parameter.DbType = GetDbType(typeof(T));
            parameter.Size = size;

            Parameters.Add(parameterName, parameter);

            return this;
        }

        public IQueryObject AddParameter<T>(string parameterName, T value)
        {
            var parameter = CreateParameter();

            parameter.Direction = ParameterDirection.Input;
            parameter.ParameterName = parameterName;
            parameter.DbType = GetDbType(value);
            parameter.Value = GetDbValue(value);

            Parameters.Add(parameterName, parameter);

            return this;
        }

        public abstract IQueryObject AddTableParameter(string parameterName, DataTable table);

        public int Execute() =>
            Disposable.Using(() => GetConnection(),
                cn =>
                {
                    cn.Open();

                    return Disposable.Using(() => GetCommand(cn),
                        cmd => cmd.ExecuteNonQuery());
                });

        public IEnumerable<T> ExecuteList<T>() where T : new() =>
            Disposable.Using(() => ExecuteReader(),
                rdr => rdr.MapList<T>());

        public IEnumerable<T> ExecuteList<T>(IObjectMapper<T> mapper) =>
            Disposable.Using(() => ExecuteReader(),
                rdr => rdr.MapList(mapper));

        public Maybe<T> ExecuteObject<T>() where T : class, new() =>
            Disposable.Using(() => ExecuteReader(),
                rdr => rdr.Read() ? rdr.Map<T>() : default(T)).ToMaybe();

        public Maybe<T> ExecuteObject<T>(IObjectMapper<T> mapper) where T : class =>
            Disposable.Using(() => ExecuteReader(),
                rdr => rdr.Read() ? rdr.Map(mapper) : default(T)).ToMaybe();

        public T ExecuteScalar<T>() =>
            Disposable.Using(() => ExecuteReader(),
                rdr => rdr.Read() ? rdr.Read<T>(0) : default(T));

        public T GetParameterValue<T>(string parameterName)
        {
            var value = Parameters[parameterName].Value;
            return value == DBNull.Value ? default(T) : (T)value;
        }

        private static object GetDbValue(object o) => o ?? DBNull.Value;

        private static DbType GetDbType(object value)
        {
            if (value == null)
                return DbType.String;

            return GetDbType(value.GetType());
        }

        private static DbType GetDbType(Type type)
        {
            if (type.IsGenericType)
                return GetDbType(type.GetGenericArguments()[0]);

            if (type == typeof(DBNull))
                return DbType.String;

            if (type.IsEnum)
                return DbType.Int32;

            try
            {
                return EnumExtensions.Parse<DbType>(type.Name);
            }
            catch
            {
                throw new Exception("Unknown type: " + type.Name);
            }
        }

        private IDbCommand GetCommand(IDbConnection connection)
        {
            var cmd = connection.CreateCommand();

            cmd.CommandText = CommandText;
            cmd.CommandType = CommandType;
            cmd.CommandTimeout = Timeout;
            cmd.Prepare();

            return AddCommandParameters(cmd);
        }

        private IDbCommand AddCommandParameters(IDbCommand cmd)
        {
            Parameters.Each(x => cmd.Parameters.Add(x.Value));
            return cmd;
        }

        public IDataReader ExecuteReader()
        {
            var cn = GetConnection();
            var cmd = GetCommand(cn);

            cn.Open();
            return cmd.ExecuteReader(CommandBehavior.CloseConnection);
        }

        public Task<IEnumerable<T>> ExecuteListAsync<T>(IObjectMapper<T> mapper) =>
            Task.Factory.StartNew(() => ExecuteList(mapper));


        public Task<IEnumerable<T>> ExecuteListAsync<T>() where T : new() =>
            Task.Factory.StartNew(() => ExecuteList<T>());

        public Task<Maybe<T>> ExecuteObjectAsync<T>(IObjectMapper<T> mapper) where T : class =>
            Task.Factory.StartNew(() => ExecuteObject(mapper));

        public Task<Maybe<T>> ExecuteObjectAsync<T>() where T : class, new() =>
            Task.Factory.StartNew(() => ExecuteObject<T>());

        public IEnumerable<IDictionary<string, object>> ExecuteUntypedList() =>
            Disposable.Using(() => ExecuteReader(),
                rdr => rdr.MapList(new UntypedDataRecordMapper()));

        public PagedResult<T> ExecutePagedResult<T>() where T : new() =>
            ExecutePagedResult(ReflectionMapperCache.GetMapper<T>());

        public PagedResult<T> ExecutePagedResult<T>(IObjectMapper<T> mapper) =>
            Disposable.Using(() => ExecuteReader(),
                rdr => new PagedResultMapper<T>(mapper).Map(rdr));

        public Task<int> ExecuteAsync() =>
            Task.Factory.StartNew(() => Execute());

        public Task<T> ExecuteScalarAsync<T>() =>
            Task.Factory.StartNew(() => ExecuteScalar<T>());
    }
}
