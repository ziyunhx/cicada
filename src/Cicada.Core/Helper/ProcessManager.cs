using Cicada.EFCore.Shared.Models;
using System;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace Cicada.Core.Helper
{
    public class ProcessManager
    {
        private CheckResult _checkResult = null;

        StringBuilder resultStr = new StringBuilder();

        public void DoCommand(TaskInfo taskInfo, ref CheckResult checkResult)
        {
            _checkResult = checkResult;

            try
            {
                Process process = new Process();

                string path = Path.Combine(Path.GetDirectoryName(typeof(ProcessManager).Assembly.Location), "Tasks\\programs\\", taskInfo.TaskId);
                
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.CreateNoWindow = true;
                //判断执行的程序是否在目录存在
                string _exePath = Path.Combine(path, taskInfo.Command);
                if (File.Exists(_exePath)) //用户使用了上传程序的相对路径作为命令输入
                {
                    process.StartInfo.WorkingDirectory = path;
                    process.StartInfo.FileName = _exePath;
                }
                else if (File.Exists(taskInfo.Command)) //用户使用了完整可执行程序路径作为命令输入
                {
                    process.StartInfo.WorkingDirectory = path;
                    process.StartInfo.FileName = taskInfo.Command;
                }
                //else if()
                //用户使用了自定义的环境变量作为命令输入

                else //用户使用了程序运行相对路径的或者系统环境目录下的程序名作为命令输入，部分程序可能会找不到需要的外部资源
                {
                    string fullPath = ProcessExtend.GetFullPath(taskInfo.Command);

                    if (string.IsNullOrWhiteSpace(fullPath)) //可能是一个无效的路径
                    {
                        process.StartInfo.FileName = taskInfo.Command;
                    }
                    else
                    {
                        process.StartInfo.WorkingDirectory = path;
                        process.StartInfo.FileName = fullPath;
                    }
                }                
                process.StartInfo.Arguments = taskInfo.Params;
                process.StartInfo.RedirectStandardInput = true;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.RedirectStandardError = true;
                process.StartInfo.ErrorDialog = false;
                process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;

                process.EnableRaisingEvents = true;

                process.Exited += new EventHandler(p_Exited);
                process.OutputDataReceived += new DataReceivedEventHandler(p_OutputDataReceived);
                process.ErrorDataReceived += new DataReceivedEventHandler(p_ErrorDataReceived);

                process.Start();

                //开始异步读取输出
                process.BeginOutputReadLine();
                process.BeginErrorReadLine();

                checkResult.Status = 200;

                //调用WaitForExit会等待Exited事件完成后再继续往下执行。
                process.WaitForExit();
                process.Close();

                checkResult.MessageInfo = resultStr.ToString();                
            }
            catch (Exception ex)
            {
                resultStr.AppendLine(ex.Message);
                checkResult.MessageInfo = resultStr.ToString();
                checkResult.Status = 500;
            }
        }

        void p_OutputDataReceived(Object sender, DataReceivedEventArgs e)
        {
            //这里是正常的输出
            resultStr.AppendLine(e.Data);
        }

        void p_ErrorDataReceived(Object sender, DataReceivedEventArgs e)
        {
            if (e != null && !string.IsNullOrWhiteSpace(e.Data))
            {
                //这里得到的是错误信息，此时将状态改为 500
                resultStr.AppendLine(e.Data);
                _checkResult.Status = 500;
            }
        }

        void p_Exited(Object sender, EventArgs e)
        {
            resultStr.AppendLine("Finish!");
        }
    }
}
