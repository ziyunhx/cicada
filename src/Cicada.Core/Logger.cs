using NLog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using LogLevel = Cicada.Core.Enums.LogLevel;

namespace Cicada.Core
{
    public class Logger
    {
        private static Dictionary<MethodBase, NLog.Logger> _loggerCache = new Dictionary<MethodBase, NLog.Logger>();

        private static NLog.Logger GetLoggerFromMethod(MethodBase methodName)
        {
            if (!_loggerCache.ContainsKey(methodName))
            {
                lock (methodName)
                {
                    if (!_loggerCache.ContainsKey(methodName))
                    {
                        NLog.Logger logger = LogManager.GetLogger(methodName.DeclaringType.FullName, methodName.DeclaringType);
                        _loggerCache[methodName] = logger;
                    }
                }
            }

            return _loggerCache[methodName];
        }

        /// <summary>
        /// 生成日志
        /// </summary>
        /// <param name="message"></param>
        /// <param name="level"></param>
        /// <param name="type"></param>
        /// <param name="ex"></param>
        public static void WriteLog(LogLevel level, string message, Exception ex = null)
        {
            try
            {
                StackTrace trace = new StackTrace();
                MethodBase methodName = null;
                try
                {
                    methodName = trace.GetFrame(2).GetMethod();
                }
                catch
                {
                    methodName = MethodBase.GetCurrentMethod();
                }

                NLog.Logger logger = GetLoggerFromMethod(methodName);

                switch (level) // Fatal级的日志被用于监控和指标记录
                {
                    case LogLevel.Debug:
                        if (!logger.IsDebugEnabled)
                            break;
                        if (ex != null)
                            logger.Debug(ex, message);
                        else
                            logger.Debug(message);
                        return;

                    case LogLevel.Info:
                        if (!logger.IsInfoEnabled)
                            break;
                        if (ex != null)
                            logger.Info(ex, message);
                        else
                            logger.Info(message);
                        return;

                    case LogLevel.Warn:
                        if (!logger.IsWarnEnabled)
                            break;
                        if (ex != null)
                            logger.Warn(ex, message);
                        else
                            logger.Warn(message);
                        return;

                    case LogLevel.Error:
                        if (!logger.IsErrorEnabled)
                            break;
                        if (ex != null)
                            logger.Error(ex, message);
                        else
                            logger.Error(message);
                        return;

                    default:
                        return;
                }
            }
            catch (Exception exception)
            {
                //throw exception;
            }
        }

        /// <summary>
        /// 信息日志
        /// </summary>
        /// <param name="Message"></param>
        /// <param name="Ex"></param>
        public static void Info(string Message, Exception Ex = null)
        {
            Logger.WriteLog(LogLevel.Info, Message, Ex);
        }

        /// <summary>
        /// Debug日志
        /// </summary>
        /// <param name="Message"></param>
        /// <param name="Ex"></param>
        public static void Debug(string Message, Exception Ex = null)
        {
            Logger.WriteLog(LogLevel.Debug, Message, Ex);
        }

        /// <summary>
        /// 特别定制：InjectionStopwatchExecTime程序使用
        /// </summary>
        /// <param name="Message"></param>
        /// <param name="Ex"></param>
        public static void InjectionStopwatchExecTime(string Message)
        {
            Logger.WriteLog(LogLevel.Debug, Message, null);
        }

        /// <summary>
        /// 警告日志
        /// </summary>
        /// <param name="Message"></param>
        /// <param name="Ex"></param>
        public static void Warn(string Message, Exception Ex = null)
        {
            Logger.WriteLog(LogLevel.Warn, Message, Ex);
        }

        /// <summary>
        /// 异常日志
        /// </summary>
        /// <param name="Message"></param>
        /// <param name="Ex"></param>
        public static void Error(string Message, Exception Ex = null)
        {
            Logger.WriteLog(LogLevel.Error, Message, Ex);
        }
    }
}
