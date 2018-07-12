using FC.Base.ExceptionHandler;
using FC.Base.OperationBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace FC.DAL.EFBase
{
    /// <summary>
    /// 业务逻辑基类
    /// </summary>
    /// <typeparam name="TEntity">数据库对象</typeparam>
    /// <typeparam name="TModel">业务对象</typeparam>
    /// <typeparam name="TRepository">数据访问实现对象</typeparam>
    public abstract class BusinessBase<TEntity, TRepository> : IDisposable
        where TEntity : class
        where TRepository : RepositoryBase<TEntity>
    {
        private TRepository repository = null;
        /// <summary>
        ///  数据访问类
        /// </summary>
        protected TRepository Repository
        {
            get { return repository; }
        }

        public void RefreshRepository()
        {
            repository = Activator.CreateInstance<TRepository>() as TRepository;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public BusinessBase()
        {
            RefreshRepository();
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entity">数据模型对象</param>
        /// <param name="statusMessage">消息</param>
        public virtual StatusResult Create(TEntity entity)
        {
            StatusResult statusMessage = new StatusResult();
            try
            {
                statusMessage.UserData = Repository.Create(entity);
            }
            catch (Exception e)
            {
                statusMessage.Status = Status.Error;
                statusMessage.Message = "创建发生错误";
                ExceptionHandler.LogExcetion(e, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod());
            }
            statusMessage.Status = Status.Success;
            return statusMessage;
        }

        /// <summary>
        /// 通过键值获取
        /// </summary>
        /// <param name="key">键值</param>
        /// <returns></returns>
        public virtual TEntity Get(object key)
        {
            try
            {
                return Repository.Get(key);
            }
            catch (Exception ex)
            {
                ExceptionHandler.LogExcetion(ex, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod());
                return null;
            }
        }

        /// <summary>
        /// 根据属性键值对查询对象集合
        /// </summary>
        /// <param name="properties">属性键值对</param>
        /// <returns></returns>
        public virtual List<TEntity> Get(PropertyParamGroup paramGroup)
        {
            try
            {
                return Repository.Get(paramGroup);
            }
            catch (Exception ex)
            {
                ExceptionHandler.LogExcetion(ex, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod());
                return null;
            }
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entity">更新的实体</param>
        public virtual StatusResult Update(TEntity entity)
        {
            StatusResult statusMessage = new StatusResult();
            try
            {
                var dbEntity = Repository.Get(entity.GetType().GetProperties().First().GetValue(entity, null));
                if (dbEntity == null)
                {
                    statusMessage.Status = Status.Wrong;
                    statusMessage.Message = "更新的对象不存在";
                }
                var result = Repository.Update(entity);
                if (!result)
                {
                    statusMessage.Status = Status.Wrong;
                    statusMessage.Message = "更新发送错误";
                }
            }
            catch (Exception ex)
            {
                statusMessage.Status = Status.Error;
                statusMessage.Message = "数据访问错误";
                //ExceptionHandler.LogExcetion(ex, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod());
                ExceptionHandler.LogExcetion(ex);
            }
            return statusMessage;
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="key">键值</param>
        public virtual StatusResult Delete(object key)
        {
            StatusResult statusMessage = new StatusResult();
            try
            {
                var entity = Repository.Get(key);
                if (entity == null)
                {
                    statusMessage.Status = Status.Wrong;
                    statusMessage.Message = "数据不存在或已被删除";
                }
                var result = Repository.Delete(key);
                if (!result)
                {
                    statusMessage.Status = Status.Wrong;
                    statusMessage.Message = "删除发生错误";
                }
            }
            catch (Exception ex)
            {
                statusMessage.Status = Status.Error;
                statusMessage.Message = "数据访问错误";
                ExceptionHandler.LogExcetion(ex, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod());
            }
            return statusMessage;
        }

        /// <summary>
        /// 删除部分
        /// </summary>
        /// <param name="keys">键集合</param>
        public virtual StatusResult Delete(PropertyParamGroup paramGroup)
        {
            StatusResult statusMessage = new StatusResult();
            try
            {
                var result = Repository.Delete(paramGroup);
                if (!result)
                {
                    statusMessage.Status = Status.Wrong;
                    statusMessage.Message = "删除发生错误";
                }
            }
            catch (Exception ex)
            {
                statusMessage.Status = Status.Error;
                statusMessage.Message = "数据访问错误";
                ExceptionHandler.LogExcetion(ex, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod());
            }
            return statusMessage;
        }

        /// <summary>
        /// 获取所有成员
        /// </summary>
        /// <returns></returns>
        public virtual List<TEntity> GetAll()
        {
            try
            {
                return Repository.GetAll();
            }
            catch (Exception ex)
            {
                ExceptionHandler.LogExcetion(ex, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod());
                return null;
            }
        }

        ///// <summary>
        ///// 执行存储过程查询函数
        ///// </summary>
        ///// <typeparam name="T">映射结果类型</typeparam>
        ///// <param name="fucName">函数名称</param>
        ///// <param name="paras">参数</param>
        ///// <returns></returns>
        //public virtual List<T> ExecuteFuc<T>(string funName, List<KeyValuePair<string, object>> paras)
        //{
        //    try
        //    {
        //        return Repository.ExecuteFuc<T>(funName, paras);
        //    }
        //    catch (Exception ex)
        //    {
        //        ExceptionHandler.LogExcetion(ex, this.GetType().FullName, System.Reflection.MethodBase.GetCurrentMethod());
        //        return null;
        //    }
        //}

        /// <summary>
        /// 析构函数，释放逻辑访问对象资源
        /// </summary>
        public virtual void Dispose()
        {
            if (repository != null)
            {
                this.repository.Dispose();
                this.repository = null;
            }
        }

        /// <summary>
        /// 数据表转json对象
        /// </summary>
        /// <param name="dataTable">数据表</param>
        /// <returns></returns>
        public string DataTableToJsonArray(System.Data.DataTable dataTable)
        {
            int i = 0, j = 0;
            StringBuilder jsonBuilder = new StringBuilder();
            jsonBuilder.Append("[");
            foreach (System.Data.DataRow row in dataTable.Rows)
            {
                if (i > 0)
                    jsonBuilder.Append(",");
                jsonBuilder.Append("{");
                j = 0;
                foreach (System.Data.DataColumn col in dataTable.Columns)
                {
                    if (j > 0)
                        jsonBuilder.Append(",");
                    string val = row[col].ToString().Replace("'", "&#39");
                    jsonBuilder.AppendFormat("{0}:{1}", col.ColumnName, col.DataType.Equals(typeof(int)) || col.DataType.Equals(typeof(decimal)) || col.DataType.Equals(typeof(float)) ? val : string.Format("'{0}'", val));
                    j++;
                }
                jsonBuilder.Append("}");
                i++;
            }
            jsonBuilder.Append("]");
            return jsonBuilder.ToString();
        }
    }
}
