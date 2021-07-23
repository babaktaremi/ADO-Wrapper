using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdoWrapper.Contracts
{
    public interface IAdoProvider:IDisposable
    {
        /// <summary>
        /// Gets and Maps the Specified Sql Table
        /// </summary>
        /// <typeparam name="T">Value To Be Returned</typeparam>
        /// <param name="sql">Sql query</param>
        /// <returns></returns>
        T GetValueOrDefault<T>(string sql) where T : class, new();

        /// <summary>
        /// Gets And Maps the Specified Table as an asynchronous operation
        /// </summary>
        /// <typeparam name="T">Value To Be Returned</typeparam>
        /// <param name="sql">Sql query</param>
        /// <returns></returns>
        Task<T> GetValueOrDefaultAsync<T>(string sql) where T : class, new();

        /// <summary>
        /// Gets A list of Result in a query
        /// </summary>
        /// <typeparam name="T">Result to be returned</typeparam>
        /// <param name="sql">sql query</param>
        /// <returns></returns>
        List<T> GetValuesOrDefault<T>(string sql) where T : class,new();

        /// <summary>
        /// Gets A list of Result in a query as an asynchronous operation 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <returns></returns>

        Task<List<T>> GetValuesOrDefaultAsync<T>(string sql) where T : class, new();
    }
}
