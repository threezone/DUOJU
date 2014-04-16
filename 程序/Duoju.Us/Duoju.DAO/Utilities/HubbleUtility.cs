using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hubble.SQLClient;
using System.Data;
using System.Data.Common;

namespace Duoju.DAO.Utilities
{
    /// <summary>
    /// Author: jary.zhang
    /// Desc: 全文检索Hubble.net
    /// Blog: http://www.cnblogs.com/three-zone
    /// </summary>
    public class HubbleUtility
    {
        const int cacheTimeout = 0;

        public string ConnectionString { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="connectionString">连接到HubbleDotNet的连接串</param>
        public HubbleUtility(string connectionString)
        {
            ConnectionString = connectionString;
        }

        /// <summary>
        /// 执行查询语句，从索引库中查出数据，并封装为DataTable
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public DataTable ExecuteDataTable(string sql, Dictionary<string, object> parameters)
        {
            return ExecuteDataTable(sql, parameters, CommandType.Text);
        }

        /// <summary>
        /// 执行查询语句，从索引库中查出数据，并封装为DataTable
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        public DataTable ExecuteDataTable(string sql, Dictionary<string, object> parameters, CommandType commandType)
        {
            using (HubbleCommand command = CreateHubbleCommand(sql, parameters, commandType))
            {
                using (HubbleDataAdapter adapter = new HubbleDataAdapter())
                {
                    DataSet ds = new DataSet();
                    adapter.SelectCommand = command;
                    ds = adapter.SelectCommand.Query(cacheTimeout);
                    return ds.Tables[0];
                }
            }
        }

        /// <summary>
        /// 采用Match的方式来进行全文检索
        /// </summary>
        /// <typeparam name="T">被搜索的类</typeparam>
        /// <param name="fields">要模糊搜索的字段，会以OR组合</param>
        /// <param name="keyWord">关键字</param>
        /// <param name="parameters">其他筛选条件，以AND组合</param>
        /// <param name="total">总数</param>
        /// <returns></returns>
        public List<T> MatchForList<T>(List<string> fields, string keyWord, Dictionary<string, object> parameters, out int total) where T : new()
        {
            var type = typeof(T);

            #region Create T-SQL
            var sql = string.Format("select * from {0} where (", type.Name);

            var isFirst = true;
            foreach (var field in fields)
            {
                if (isFirst)
                {
                    sql += string.Format("{0} Match @keyWord ", field);
                    isFirst = false;
                }
                else
                {
                    sql += string.Format("or {0} Match @keyWord ", field);
                }
            }
            sql += ")";



            if (parameters.Count != 0)
            {
                foreach (var par in parameters)
                {
                    sql += string.Format(" and {0}={1} ", par.Key.Replace("@", ""), par.Key);
                }
            }

            parameters.Add("@keyWord", GetKeywordAnalyzerStringFromat(keyWord, type.Name, fields[0]));
            #endregion

            return QueryForList<T>(sql, parameters, out total);
        }

        /// <summary>  
        /// 查询多个实体集合  
        /// </summary>
        /// <typeparam name="T">返回的实体集合类型</typeparam>  
        /// <param name="sql">要执行的查询语句</param>     
        /// <param name="parameters">执行SQL查询语句所需要的参数</param>  
        /// <returns></returns>  
        public List<T> QueryForList<T>(string sql, Dictionary<string, object> parameters, out int total) where T : new()
        {
            return QueryForList<T>(sql, parameters, out total, CommandType.Text);
        }

        /// <summary>  
        ///  查询多个实体集合  
        /// </summary>  
        /// <typeparam name="T">返回的实体集合类型</typeparam>  
        /// <param name="sql">要执行的查询语句</param>     
        /// <param name="parameters">执行SQL查询语句所需要的参数</param>     
        /// <param name="commandType">执行的SQL语句的类型</param>  
        /// <returns></returns> 
        public List<T> QueryForList<T>(string sql, Dictionary<string, object> parameters, out int total, CommandType commandType) where T : new()
        {
            DataTable data = ExecuteDataTable(sql, parameters, commandType);
            total = data.MinimumCapacity;
            return EntityReader.GetEntities<T>(data);
        }

        /// <summary>  
        /// 查询单个实体  
        /// </summary>  
        /// <typeparam name="T">返回的实体集合类型</typeparam>  
        /// <param name="sql">要执行的查询语句</param>     
        /// <param name="parameters">执行SQL查询语句所需要的参数</param>  
        /// <returns></returns>  
        public T QueryForObject<T>(string sql, Dictionary<string, object> parameters) where T : new()
        {
            return QueryForObject<T>(sql, parameters, CommandType.Text);
        }

        /// <summary>  
        /// 查询单个实体  
        /// </summary>  
        /// <typeparam name="T">返回的实体集合类型</typeparam>  
        /// <param name="sql">要执行的查询语句</param>     
        /// <param name="parameters">执行SQL查询语句所需要的参数</param>     
        /// <param name="commandType">执行的SQL语句的类型</param>  
        /// <returns></returns>  
        public T QueryForObject<T>(string sql, Dictionary<string, object> parameters, CommandType commandType) where T : new()
        {
            DataTable data = ExecuteDataTable(sql, parameters, commandType);
            return EntityReader.GetEntities<T>(data)[0];
        }

        /// <summary>
        /// 获取格式化后的关键字
        /// eg. '要出发旅行网' --> '要出发^rank^0 旅行网^rank^1'
        /// 其中，rank为分词后词性的等级，position为词的位置，为计分提供参数
        /// </summary>
        /// <param name="keyWords">搜索的关键字</param>
        /// <param name="tableName">搜索Hubble中对应的表（索引）</param>
        /// <param name="fieldName">被搜索的字段（如果是多字段搜索，只放其中一个字段）</param>
        /// <returns></returns>
        public string GetKeywordAnalyzerStringFromat(string keyWords, string tableName, string fieldName)
        {
            HubbleCommand matchCmd = CreateHubbleCommand();
            string wordssplitbyspace;
            string matchString;
            try
            {
                matchString = matchCmd.GetKeywordAnalyzerStringFromServer(tableName,
                      fieldName, keyWords, int.MaxValue, out wordssplitbyspace);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return matchString;
        }

        /// <summary>
        /// 创建一个Hubble命令实例
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        private HubbleCommand CreateHubbleCommand(string sql, Dictionary<string, object> parameters, CommandType commandType)
        {
            //HubbleAsyncConnection 为异步连接
            //HubbleConnection 连接为同步连接
            //HubbleAsyncConnection 的工作机制理论上比 HubbleConnection 快 10 倍
            HubbleAsyncConnection connection = new HubbleAsyncConnection(ConnectionString);
            HubbleCommand command = new HubbleCommand(sql, connection);
            command.CommandType = commandType;
            command.CacheTimeout = cacheTimeout;
            if (!(parameters == null || parameters.Count == 0))
            {
                foreach (var parameter in parameters)
                {
                    command.Parameters.Add(parameter.Key, parameter.Value);
                }
            }
            return command;
        }

        /// <summary>
        /// 重载，创建一个Hubble命令实例
        /// </summary>
        /// <returns></returns>
        private HubbleCommand CreateHubbleCommand()
        {
            HubbleConnection connection = new HubbleConnection(ConnectionString);
            HubbleCommand command = new HubbleCommand(connection);
            return command;
        }

        /// <summary>
        /// HubbleDotNet系统级别的存储过程SP_Columns，查询每个字段的分析器类型
        /// 该方法封装SP_Columns，对参数中的clumns的分析器进行映射
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="clumns"></param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public string[] MapperAnalyzerNames(HubbleAsyncConnection conn, string[] clumns, string tableName)
        {
            string sql = string.Format("exec SP_Columns '{0}'", tableName.Replace("'", "''"));
            string[] analyers = new string[clumns.Length];

            HubbleCommand cmd = new HubbleCommand(sql, conn);
            DataTable dt = cmd.Query().Tables[0];

            for (var i = 0; i < clumns.Length; i++)
            {
                foreach (DataRow row in dt.Rows)
                {
                    if (row["FieldName"].ToString().Equals(clumns[i], StringComparison.CurrentCultureIgnoreCase))
                    {
                        analyers[i] = row["Analyzer"].ToString();
                    }
                }
            }
            return analyers;
        }
    }
}
