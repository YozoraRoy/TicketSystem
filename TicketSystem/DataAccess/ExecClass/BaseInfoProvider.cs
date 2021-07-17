using Dapper;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper.Contrib.Extensions;
using System.Globalization;
using Platform.IOC;

namespace DataAccess.ExecClass
{
    [RegisterIOC(IocType.Transient)]
    public class BaseInfoProvider
    {
        private int _connectionTimeout = 60;
        // private string dbPath = @".\Test.sqlite";
        private string cnStr = "data source=" + @".\Test.sqlite";

        public BaseInfoProvider()
        {

        }

        /// <summary>
        /// 查詢資料
        /// </summary>
        /// <typeparam name="TReturn">回覆的資料類型</typeparam>
        /// <param name="querySql">SQL敘述</param>
        /// <param name="param">查詢參數物件</param>        
        /// <param name="commandType">敘述類型</param>
        /// <returns>資料物件</returns>x
        public async Task<IEnumerable<TReturn>> QueryAsync<TReturn>(string querySql, object param = null, CommandType commandType = CommandType.Text)
        {
            using (var con = new SqliteConnection(cnStr))
            {
                return await con.QueryAsync<TReturn>(querySql, param, null, _connectionTimeout, commandType).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Excute Non-Query SQL，允許一次傳入多道SQL指令
        /// </summary>
        /// <param name="excuteSql">SQL敘述</param>
        /// <param name="param">參數物件</param>
        /// <param name="enableTransaction">包Transaction執行</param>
        /// <param name="commandType">敘述類型</param>
        /// <returns>影響資料筆數</returns>
        public async Task<int> ExecuteNonQueryAsync(string excuteSql, object param = null, bool enableTransaction = false, CommandType commandType = CommandType.Text)
        {
            using (var con = new SqliteConnection(cnStr))
            {
                if (!enableTransaction)
                {
                    return await con.ExecuteAsync(excuteSql, param, null, _connectionTimeout, commandType).ConfigureAwait(false);
                }
                else
                {
                    using (var trans = con.BeginTransaction())
                    {
                        try
                        {
                            var result = await con.ExecuteAsync(excuteSql, param, trans, _connectionTimeout, commandType).ConfigureAwait(false);
                            trans.Commit();
                            return result;
                        }
                        catch
                        {
                            trans.Rollback();
                            throw;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// ExecuteScalar，執行查詢並傳回第一個資料列的第一個資料行中查詢所傳回的結果
        /// </summary>
        /// <param name="excuteSql">SQL敘述</param>
        /// <param name="param">參數物件</param>
        /// <param name="enableTransaction">包Transaction執行</param>
        /// <param name="commandType">敘述類型</param>
        /// <returns>執行回覆結果</returns>
        public async Task<object> ExecuteScalarAsync(string excuteSql, object param = null, bool enableTransaction = false, CommandType commandType = CommandType.Text)
        {
            using (var con = new SqliteConnection(cnStr))
            {
                if (!enableTransaction)
                {
                    return await con.ExecuteScalarAsync(excuteSql, param, null, _connectionTimeout, commandType).ConfigureAwait(false);
                }
                else
                {
                    using (var trans = con.BeginTransaction())
                    {
                        try
                        {
                            var result = await con.ExecuteScalarAsync(excuteSql, param, trans, _connectionTimeout, commandType).ConfigureAwait(false);
                            trans.Commit();
                            return result;
                        }
                        catch
                        {
                            trans.Rollback();
                            throw;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 新增單筆
        /// </summary>
        /// <typeparam name="T">新增資料物件Type or IEnumable</typeparam>
        /// <param name="insertEntity">新增物件</param>        
        /// <returns>The ID(primary key) of the newly inserted record if it is identity using the defined type, otherwise null</returns>
        public async Task<int> InsertAsync<T>(T insertEntity)
            where T : class
        {
            using (var con = new SqliteConnection(cnStr))
            {
                var insertResult = await con.InsertAsync(insertEntity, null, _connectionTimeout).ConfigureAwait(false);                
                return insertResult;
            }
        }

        /// <summary>
        /// 更新單筆資料
        /// </summary>
        /// <typeparam name="T">更新資料物件Type</typeparam>
        /// <param name="updateEntity">更新物件</param>
        /// <returns></returns>
        public async Task<bool> UpdateAsync<T>(T updateEntity)
            where T : class
        {
            using (var con = new SqliteConnection(cnStr))
            {
                return await con.UpdateAsync(updateEntity, null, _connectionTimeout).ConfigureAwait(false);
            }
        }

    }
}
