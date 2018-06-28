using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FC.Base.ExceptionHandler
{
    /// <summary>
    /// 异常处理
    /// </summary>
    public class ExceptionHandler
    {
        public static object locker = new object();
        /// <summary>
        /// 记录异常日志
        /// </summary>
        public static void LogExcetion(Exception ex)
        {
            //TODO exception
            lock (locker)
            {
                //string LogAddress = Environment.CurrentDirectory + '\\' +
                //      DateTime.Now.Year + '-' +
                //      DateTime.Now.Month + '-' +
                //      DateTime.Now.Day + "_Log.log";
                if (Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + "\\Log_Excetion") == false)//如果不存在就创建file文件夹
                {
                    Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + "\\Log_Excetion");
                }
                string LogAddress = AppDomain.CurrentDomain.BaseDirectory + "\\Log_Excetion\\" +
                      DateTime.Now.Year + '-' +
                      DateTime.Now.Month + '-' +
                      DateTime.Now.Day + "_Log.log";
                //把异常信息输出到文件
                StreamWriter sw = new StreamWriter(LogAddress, true);
                sw.WriteLine("当前时间：" + DateTime.Now.ToString());
                sw.WriteLine("异常信息：" + ex.Message);
                sw.WriteLine("异常信息的键值对集合：" + ex.Data);
                sw.WriteLine("异常实例信息：" + ex.InnerException);
                sw.WriteLine("异常对象：" + ex.Source);
                sw.WriteLine("调用堆栈：\n" + ex.StackTrace.Trim());
                sw.WriteLine("触发方法：" + ex.TargetSite);
                sw.WriteLine();
                sw.Close();
            }
        }

        /// <summary>
        /// 记录异常日志
        /// </summary>
        /// <param name="ex">异常</param>
        /// <param name="actionName">触发异常的action</param>
        /// <param name="area">触发异常的区域</param>
        /// <param name="controllerName">触发异常的控制器</param>
        public static void LogExcetion(Exception ex, string area, string controllerName, string actionName, string LogAddress = "")
        {
            //TODO exception
        }

        public static void LogExcetion(Exception ex, string source, System.Reflection.MethodBase a)
        {
            //TODO exception
        }
    }
}
