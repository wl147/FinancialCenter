using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;

namespace FC.DAL.EFBase
{
    /// <summary>
    /// 数据访问基类，封装基础方法
    /// </summary>
    /// <typeparam name="TEntity">操作实体类型</typeparam>
    public abstract class RepositoryBase<TEntity> : IDisposable
        where TEntity : class
    {
        private DbContext entities;
        /// <summary>
        /// 数据库访问对象
        /// </summary>
        protected DbContext Entities { get { return entities; } }

        private DbSet<TEntity> entitySet;
        /// <summary>
        /// 操作实体集合
        /// </summary>
        protected DbSet<TEntity> EntitySet { get { return entitySet; } }

        /// <summary>
        /// 获取指定类型上下文集合
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <returns></returns>
        protected DbSet<T> GetDbSetSet<T>() where T : class
        {
            return entities.GetType().GetProperty(typeof(T).Name).GetValue(entities, null) as DbSet<T>;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public RepositoryBase()
        {
            entities = new DbContext(System.Configuration.ConfigurationManager.AppSettings["EntityConnection"]);//(DbContext)Activator.CreateInstance(Type.GetType(System.Configuration.ConfigurationManager.AppSettings["EntityContainerType"]), new object[] { System.Configuration.ConfigurationManager.AppSettings["EntityConnection"] });
            entitySet = entities.Set<TEntity>(); //entities.GetType().GetProperty(typeof(TEntity).Name).GetValue(entities, null);
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entity">实体对象</param>
        public virtual TEntity Create(TEntity entity)
        {
            EntitySet.Add(entity);
            Entities.SaveChanges();
            return entity;
        }

        /// <summary>
        /// 批量新增
        /// </summary>
        /// <param name="entity">实体对象</param>
        public virtual bool CreateSet(List<TEntity> entitySet)
        {
            foreach (var entity in entitySet)
            {
                EntitySet.Add(entity);
            }
            Entities.SaveChanges();
            return true;
        }

        /// <summary>
        /// 通过主键获取
        /// </summary>
        /// <param name="keys">主键对</param>
        /// <returns></returns>
        public virtual TEntity Get(List<KeyValuePair<string, object>> keys)
        {
            return Entities.Set<TEntity>().Find(keys);
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <typeparam name="TKey">排序键</typeparam>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="whereLambda"></param>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        public virtual List<TEntity> GetList<TKey>(int pageIndex, int pageSize,
            Expression<Func<TEntity, bool>> whereLambda, Expression<Func<TEntity, TKey>> orderBy)
        {
            return EntitySet.Where(whereLambda).OrderBy(orderBy).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
        }

        /// <summary>
        /// 通过主键获取
        /// </summary>
        /// <param name="key">主键值</param>
        /// <returns></returns>
        public virtual TEntity Get(object key)
        {
            return Entities.Set<TEntity>().Find(key);
        }

        /// <summary>
        /// 根据属性键值对查询对象集合
        /// </summary>
        /// <param name="paramGroup">属性键值对</param>
        /// <returns></returns>
        public virtual List<TEntity> Get(PropertyParamGroup paramGroup)
        {
            List<SqlParameter> paras = paramGroup.GetParams();
            SqlParameter[] pars = paras == null ? new SqlParameter[0] : paras.ToArray();
            string sql = string.Format("SELECT * FROM [{0}] WHERE {1}", EntitySet.GetType().Name, paramGroup.ToString());
            return Entities.Database.SqlQuery<TEntity>(sql, pars).ToList();
        }

        /// <summary>
        /// 更新实体所有属性
        /// </summary>
        /// <param name="entity">更新的实体</param>
        public virtual bool Update(TEntity entity)
        {
            EntitySet.Attach(entity);
            Entities.Entry<TEntity>(entity).State = EntityState.Modified;
            Entities.SaveChanges();
            return true;
        }

        /// <summary>
        /// 批量更新实体所有属性
        /// </summary>
        /// <param name="entitySet">需更新实体的集合</param>
        /// <returns></returns>
        public virtual bool UpdateSet(List<TEntity> entitySet)
        {
            foreach (var entity in entitySet)
            {
                EntitySet.Attach(entity);
                Entities.Entry<TEntity>(entity).State = EntityState.Modified;
            }
            Entities.SaveChanges();
            return true;
        }

        /// <summary>
        /// 获取指定类型上下文集合
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <returns></returns>
        protected DbSet<T> GetObjectSet<T>() where T : class
        {
            return entities.Set<T>();//GetType().GetProperty(typeof(T).Name).GetValue(entities, null) as ObjectSet<T>;
        }

        /// <summary>
        /// 根据指定属性更新对象
        /// </summary>
        /// <param name="entity">实体</param>
        /// <param name="updatePropertyNames">属性名称集合</param>
        /// <returns></returns>
        //public virtual bool Update(TEntity entity, List<string> updatePropertyNames)
        //{
        //    EntitySet.Attach(entity);
        //    var entry = Entities.Entry<TEntity>(entity).State;
        //    if (updatePropertyNames != null && updatePropertyNames.Count > 0)
        //        updatePropertyNames.ForEach(entry = EntityState.Modified);
        //    Entities.SaveChanges();
        //    return true;
        //}

        /// <summary>
        /// 根据键值删除
        /// </summary>
        /// <param name="key">键值</param>
        public virtual bool Delete(object key)
        {
            string sql = string.Format("DELETE FROM [{0}] WHERE [{1}]=@key", EntitySet.GetType().Name, EntitySet.GetType().GetProperties().First().Name);
            SqlParameter[] paras = new SqlParameter[1];
            paras[0] = new SqlParameter("@key", key);
            return Entities.Database.ExecuteSqlCommand(sql, paras) > 0;
        }

        public virtual bool DeleteSet(List<TEntity> entitySet)
        {
            foreach (var entity in entitySet)
            {
                EntitySet.Attach(entity);
                EntitySet.Remove(entity);
            }
            Entities.SaveChanges();
            return true;
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public virtual bool Delete(TEntity entity)
        {
            EntitySet.Attach(entity);
            EntitySet.Remove(entity);
            Entities.SaveChanges();
            return true;
        }
        /// <summary>
        /// 根据条件删除
        /// </summary>
        /// <param name="keys">键值集合</param>
        public virtual bool Delete(PropertyParamGroup paramGroup)
        {
            List<SqlParameter> pars = paramGroup.GetParams();
            string sql = string.Format("DELETE FROM [{0}] WHERE {1}", EntitySet.GetType().Name, paramGroup.ToString());
            SqlParameter[] paras = pars == null ? null : pars.ToArray();
            return Entities.Database.ExecuteSqlCommand(sql, paras) > 0;
        }

        /// <summary>
        /// 获取所有成员
        /// </summary>
        /// <returns></returns>
        public virtual List<TEntity> GetAll()
        {
            return EntitySet.ToList();
        }

        /// <summary>
        /// 获取所有成员
        /// Iqueryable
        /// </summary>
        /// <returns></returns>
        public virtual IQueryable<TEntity> GetAllIqueryable()
        {
            return EntitySet.AsQueryable<TEntity>();
        }

        ///// <summary>
        ///// 查询函数
        ///// </summary>
        ///// <typeparam name="T">映射结果类型</typeparam>
        ///// <param name="fucName">函数名称</param>
        ///// <param name="paras">参数</param>
        ///// <returns></returns>
        //public virtual List<T> ExecuteFuc<T>(string fucName, List<KeyValuePair<string, object>> paras)
        //{
        //    var p = paras == null ? null : paras.Select(pa => new ObjectParameter(pa.Key, pa.Value)).ToArray();
        //    return Entities.Database.ExecuteSqlCommand(fucName, p).ToList();
        //}

        /// <summary>
        /// 释放数据访问实体资源
        /// </summary>
        public virtual void Dispose()
        {
            entities.Dispose();
            entities = null;
        }
    }
}
