using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FC.Base.OperationBase
{
    /// <summary>
    /// 操作消息
    /// </summary>
    public class StatusResult : IGetStatusResult
    {
        /// <summary>
        /// 操作执行状态
        /// </summary>
        public Status Status { get; set; }

        /// <summary>
        /// 消息
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 自定义数据
        /// </summary>
        public object UserData { get; set; }
        /// <summary>
        /// 总计行数
        /// </summary>
        public int TotalCount { get; set; }
        /// <summary>
        /// 构造函数
        /// </summary>
        public StatusResult()
        {
            Status = Status.Success;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public StatusResult(Status status, string message)
        {
            Status = status;
            Message = message;
            TotalCount = 0;
        }

        public StatusResult GetResult()
        {
            return this;
        }
    }

    /// <summary>
    /// 执行状态枚举
    /// </summary>
    public enum Status
    {
        /// <summary>
        /// 成功
        /// </summary>
        Success = 0,

        /// <summary>
        /// 错误
        /// </summary>
        Error = 1,

        /// <summary>
        /// 警告
        /// </summary>
        Wrong = 2
    }

    /// <summary>
    /// 获取操作结果接口
    /// </summary>
    public interface IGetStatusResult
    {
        /// <summary>
        /// 获取操作结果
        /// </summary>
        /// <returns></returns>
        StatusResult GetResult();
    }
}
