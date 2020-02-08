using System;
using Newtonsoft.Json;

namespace CicadaAdapter
{
    public class MessageBox
    {
        private string classMsg = "";

        /// <summary>
        /// Initializes a new instance of the <see cref="T:CicadaAdapter.MessageBox"/> class.
        /// </summary>
        /// <param name="className">Class name.</param>
        public MessageBox(string className = "")
        {
            if (!string.IsNullOrEmpty(className))
                classMsg = className + ": ";
        }

        /// <summary>
        /// Trace the specified Message and args.
        /// </summary>
        /// <param name="Message">Message.</param>
        /// <param name="args">Arguments.</param>
        public void Trace(string Message, params object[] args)
        {
            SendMessage(FromatMessage(Message, args), 0);
        }
        /// <summary>
        /// Debug the specified Message and args.
        /// </summary>
        /// <param name="Message">Message.</param>
        /// <param name="args">Arguments.</param>
        public void Debug(string Message, params object[] args)
        {
            SendMessage(FromatMessage(Message, args), 1);
        }
        /// <summary>
        /// Info the specified Message and args.
        /// </summary>
        /// <param name="Message">Message.</param>
        /// <param name="args">Arguments.</param>
        public void Info(string Message, params object[] args)
        {
            SendMessage(FromatMessage(Message, args), 2);
        }
        /// <summary>
        /// Warn the specified Message and args.
        /// </summary>
        /// <param name="Message">Message.</param>
        /// <param name="args">Arguments.</param>
        public void Warn(string Message, params object[] args)
        {
            SendMessage(FromatMessage(Message, args), 3);
        }
        /// <summary>
        /// Error the specified Message and args.
        /// </summary>
        /// <param name="Message">Message.</param>
        /// <param name="args">Arguments.</param>
        public void Error(string Message, params object[] args)
        {
            SendMessage(FromatMessage(Message, args), 4);
        }

        private void SendMessage(string Message, int level = 2)
        {
            CicadaMsg message = new CicadaMsg();
            message.id = Guid.NewGuid().ToString();
            message.command = "log";
            message.msg = classMsg + Message;
            message.level = level;

            StdInAndOut.SendMsg(message);
            //you can also wait the result adopt listen the needAckMessageIds queue.
            //var result = CicadaLaunch.WaitMsgBeAck(message.id, 60000);
        }

        private string FromatMessage(string Message, params object[] args)
        {
            string message = Message;
            try
            {
                if (args != null && args.Length > 0)
                    message = string.Format(Message, args);
            }
            catch { }

            return message;
        }
    }
}