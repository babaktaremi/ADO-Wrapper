using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
//using System.Data.SqlClient;
using System.Threading.Tasks;
using AdoWrapper.Contracts;
using AdoWrapper.Extensions;
using AdoWrapper.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;

namespace AdoWrapper.Infrastructure
{
    internal class AdoProvider : IAdoProvider
    {
        private readonly IOptions<ConnectionStringModel> _connectionStringModel;

        public AdoProvider(IOptions<ConnectionStringModel> connectionStringModel)
        {
            _connectionStringModel = connectionStringModel;
        }

        public T GetFirstOrDefault<T>(string sql) where T : class, new()
        {
            var result = default(T);
            var properties = TypeExtensions.GetWritableProperties<T>();

            using SqlConnection connection = new SqlConnection(_connectionStringModel.Value.ConnectionString);
            using SqlCommand command = new SqlCommand(sql, connection);

            connection.Open();

            using SqlDataReader reader = command.ExecuteReader(CommandBehavior.CloseConnection);

            if (reader.Read())
            {
                result = new T();
                foreach (var property in properties)
                {
                    var value = reader.GetValue(reader.GetOrdinal(property.Name));
                    property.SetValue(result, value);
                }
            }

            return result;
        }

        public async Task<T> GetFirstOrDefaultAsync<T>(string sql) where T : class, new()
        {
            var result = default(T);
            var properties = TypeExtensions.GetWritableProperties<T>();

            await using SqlConnection connection = new SqlConnection(_connectionStringModel.Value.ConnectionString);
            await using SqlCommand command = new SqlCommand(sql, connection);

            await connection.OpenAsync().ConfigureAwait(false);

            await using SqlDataReader reader = await command.ExecuteReaderAsync(CommandBehavior.CloseConnection).ConfigureAwait(false);

            if (await reader.ReadAsync().ConfigureAwait(false))
            {
                result = new T();

                foreach (var property in properties)
                {
                    var value = await reader.GetFieldValueAsync<object>(reader.GetOrdinal(property.Name)).ConfigureAwait(false);
                    property.SetValue(result, value);
                }
            }

            return result;
        }

        public List<T> GetList<T>(string sql) where T : class, IEquatable<T>, new()
        {
            var properties = TypeExtensions.GetWritableProperties<T>();
            var result = new List<T>();

            using SqlConnection connection = new SqlConnection(_connectionStringModel.Value.ConnectionString);
            using SqlCommand command = new SqlCommand(sql, connection);

            connection.Open();

            using SqlDataReader reader = command.ExecuteReader(CommandBehavior.CloseConnection);

            while (reader.Read())
            {
                var temp = new T();

                foreach (var property in properties)
                {
                    var value = reader.GetFieldValue<object>(reader.GetOrdinal(property.Name));
                    property.SetValue(temp, value);
                }

                result.Add(temp);
            }

            return result;
        }

        public async Task<List<T>> GetListAsync<T>(string sql) where T : class, IEquatable<T>, new()
        {
            var properties = TypeExtensions.GetWritableProperties<T>();
            var result = new List<T>();
            await using SqlConnection connection = new SqlConnection(_connectionStringModel.Value.ConnectionString);
            await using SqlCommand command = new SqlCommand(sql, connection);

            await connection.OpenAsync().ConfigureAwait(false);

            await using SqlDataReader reader = await command.ExecuteReaderAsync(CommandBehavior.CloseConnection).ConfigureAwait(false);
            
            var listProperties = TypeExtensions.GetListProperties<T>();
            
            while (await reader.ReadAsync().ConfigureAwait(false))
            {
                var temp = new T();
                
                foreach (var property in properties)
                {
                    if (property.IsClassProperty())
                    {
                        if (property.IsPropertyGenericList())
                        {
                            var innerProperty = property.GetGenericArgument();

                            var obj = Activator.CreateInstance(innerProperty);

                            var list = listProperties.FirstOrDefault(c => c.GetType() == property.PropertyType);


                            var objProperties = obj.GetWritableProperties();

                            foreach (var prop in objProperties)
                            {
                                var val = await reader.GetFieldValueAsync<object>(reader.GetOrdinal(prop.Name)).ConfigureAwait(false);
                                prop.SetValue(obj, val);
                            }

                            list.Add(obj);

                            property.SetValue(temp, list);

                        }
                        continue;
                    }

                    var value = await reader.GetFieldValueAsync<object>(reader.GetOrdinal(property.Name)).ConfigureAwait(false);
                    property.SetValue(temp, value);
                }

                if (result.Contains(temp))
                    continue;
                

                result.Add(temp);
            }

            return result;
        }
    }
}