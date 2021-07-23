using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using AdoWrapper.Contracts;
using AdoWrapper.Extensions;
using AdoWrapper.Models;
using Microsoft.Extensions.Options;

namespace AdoWrapper.Infrastructure
{
   internal class AdoProvider:IAdoProvider
   {
      
       private readonly IOptions<ConnectionStringModel> _connectionStringModel;

       public AdoProvider(IOptions<ConnectionStringModel> connectionStringModel)
       {
           _connectionStringModel = connectionStringModel;
       }

        public T GetValueOrDefault<T>(string sql) where T:class,new()
        {
            var result = new T();
            var properties = result.GetProperties();

            using SqlConnection connection = new SqlConnection(_connectionStringModel.Value.ConnectionString);
            using SqlCommand command = new SqlCommand(sql, connection);

            connection.Open();

            using SqlDataReader reader = command.ExecuteReader(CommandBehavior.CloseConnection);

            while (reader.Read())
            {
                foreach (var property in properties)
                {
                   result.FillProperty(property.Key,reader.GetValue(reader.GetOrdinal(property.Key))); 
                }
            }

            return result;
        }

        public async Task<T> GetValueOrDefaultAsync<T>(string sql) where T : class, new()
        {
            var result = new T();
            var properties = result.GetProperties();

            await using SqlConnection connection = new SqlConnection(_connectionStringModel.Value.ConnectionString);
            await using SqlCommand command = new SqlCommand(sql, connection);

            connection.Open();

            await using SqlDataReader reader = await command.ExecuteReaderAsync(CommandBehavior.CloseConnection);

            while (reader.Read())
            {
                foreach (var property in properties)
                {
                    result.FillProperty(property.Key,await reader.GetFieldValueAsync<object>(reader.GetOrdinal(property.Key)));
                }
            }

            return result;
        }

        public List<T> GetValuesOrDefault<T>(string sql) where T : class, new()
        {
            var result= new List<T>();

            using SqlConnection connection = new SqlConnection(_connectionStringModel.Value.ConnectionString);
            using SqlCommand command = new SqlCommand(sql, connection);

            connection.Open();

            using SqlDataReader reader = command.ExecuteReader(CommandBehavior.CloseConnection);

            while (reader.Read())
            {
                var temp = new T();
                var properties = temp.GetProperties();

                foreach (var property in properties)
                {
                    temp.FillProperty(property.Key, reader.GetValue(reader.GetOrdinal(property.Key)));
                }

                result.Add(temp);
            }

            return result;
        }

        public async Task<List<T>> GetValuesOrDefaultAsync<T>(string sql) where T : class, new()
        {
            var result = new List<T>();

            await using SqlConnection connection = new SqlConnection(_connectionStringModel.Value.ConnectionString);
            await using SqlCommand command = new SqlCommand(sql, connection);

            connection.Open();

            await using SqlDataReader reader = await command.ExecuteReaderAsync(CommandBehavior.CloseConnection);

            while (reader.Read())
            {
                var temp = new T();
                var properties = temp.GetProperties();

                foreach (var property in properties)
                {
                    temp.FillProperty(property.Key, await reader.GetFieldValueAsync<object>(reader.GetOrdinal(property.Key)));
                }

                result.Add(temp);
            }

            return result;
        }


        public void Dispose()
        {
            
        }
    }
}
