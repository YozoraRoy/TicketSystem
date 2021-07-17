using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public interface IBaseInfoProvider
    {        
        Task<IEnumerable<TReturn>> QueryAsync<TReturn>(string querySql, object param = null, CommandType commandType = CommandType.Text);

        Task<int> ExecuteNonQueryAsync(string excuteSql, object param = null, bool enableTransaction = false, CommandType commandType = CommandType.Text);

        Task<object> ExecuteScalarAsync(string excuteSql, object param = null, bool enableTransaction = false, CommandType commandType = CommandType.Text);
    }
}
