using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FC.DAL.EFBase
{
    /// <summary>
    /// 属性条件查询参数
    /// </summary>
    public class PropertyParam
    {
        /// <summary>
        /// 键
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// 值
        /// </summary>
        public object Value { get; set; }

        /// <summary>
        /// 键值关系
        /// </summary>
        public string KeyValueRelation { get; set; }

        /// <summary>
        /// 与前属性关系
        /// </summary>
        public string ContextRelation { get; set; }

        /// <summary>
        /// 子组
        /// </summary>
        public List<PropertyParamGroup> Groups { get; set; }

        /// <summary>
        /// 获取sql参数
        /// </summary>
        /// <returns></returns>
        public List<SqlParameter> GetParams()
        {
            List<SqlParameter> paras = new List<SqlParameter>();
            paras.Add(new SqlParameter("@" + Key, KeyValueRelation == EFBase.ContextRelation.Like ? ("%" + Value.ToString() + "%") : Value));
            if (Groups != null && Groups.Count > 0)
                Groups.ForEach(g => paras.AddRange(g.GetParams()));
            return paras;
        }
    }

    /// <summary>
    /// 属性条件查询参数组
    /// </summary>
    public class PropertyParamGroup
    {
        /// <summary>
        /// 查询参数集合
        /// </summary>
        public List<PropertyParam> PropertyParams { get; set; }

        /// <summary>
        /// 与前查询条件关系
        /// </summary>
        public string ContextRelation { get; set; }

        /// <summary>
        /// 获取PropertyParamGroup的出条件文本
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            if (PropertyParams == null || PropertyParams.Count == 0)
                return string.Empty;
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat("{0} ( {1} )", ContextRelation, PropertyParams.ToPropertyParamString());
            return builder.ToString();
        }

        /// <summary>
        /// 获取sql参数
        /// </summary>
        /// <returns></returns>
        public List<SqlParameter> GetParams()
        {
            if (PropertyParams == null || PropertyParams.Count == 0)
                return null;
            List<SqlParameter> paras = new List<SqlParameter>();
            PropertyParams.ForEach(p => paras.AddRange(p.GetParams()));
            return paras;
        }
    }

    /// <summary>
    /// 条件与前条件关系
    /// </summary>
    public class ContextRelation
    {
        /// <summary>
        /// 与前条件无关系
        /// </summary>
        public const string NULL = "";
        /// <summary>
        /// 与前条件为OR关系
        /// </summary>
        public const string OR = "OR";
        /// <summary>
        /// 与前条件为AND关系
        /// </summary>
        public const string AND = "AND";

        /// <summary>
        ///  键值关系 &gt;
        /// </summary>
        public const string Greater = ">";
        /// <summary>
        ///  键值关系 &gt;=
        /// </summary>
        public const string GreaterEqual = ">=";
        /// <summary>
        /// 键值关系 =
        /// </summary>
        public const string Equal = "=";
        /// <summary>
        ///  键值关系 &lt;&gt;
        /// </summary>
        public const string NotEqual = "<>";
        /// <summary>
        ///  键值关系 &lt;
        /// </summary>
        public const string Less = "<";
        /// <summary>
        /// 键值关系 &lt;=
        /// </summary>
        public const string LessEqual = "<=";
        /// <summary>
        /// 键值关系 like
        /// </summary>
        public const string Like = "LIKE";
    }

    public static class PropertyParamExtend
    {
        /// <summary>
        /// 由PropertyParam集合得出条件文本
        /// </summary>
        /// <param name="propertyParams">PropertyParam集合</param>
        /// <returns></returns>
        public static string ToPropertyParamString(this List<PropertyParam> propertyParams)
        {
            if (propertyParams == null || propertyParams.Count == 0)
                return string.Empty;
            StringBuilder builder = new StringBuilder();
            propertyParams.ForEach(new Action<PropertyParam>(delegate (PropertyParam param)
            {
                builder.AppendFormat("{0} ( [{1}] {2} @{1}", param.ContextRelation, param.Key, param.KeyValueRelation);
                if (param.Groups != null && param.Groups.Count > 0)
                    param.Groups.ForEach(g => builder.AppendFormat(" {0}", g.ToString()));
                builder.Append(" )");
            }));
            return builder.ToString();
        }
    }
}
