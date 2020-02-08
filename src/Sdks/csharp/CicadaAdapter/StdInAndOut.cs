using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using Newtonsoft.Json;

namespace CicadaAdapter
{
    public class StdInAndOut
    {
        private static object sendMsglock = new object();

        /// <summary>
        /// write lines to default stream.
        /// </summary>
        /// <param name="message">stdout</param>
        public static void SendMsg(CicadaMsg message)
        {
            if (message == null)
                return;

            lock (sendMsglock) //keep send msg together.
            {
                //fix output bug on mono.
                var encoding = new UTF8Encoding(false);
                Console.OutputEncoding = encoding;
                Console.WriteLine("cicadaMsg:" + JsonConvert.SerializeObject(message));
                Console.WriteLine("cicadaMsg:end");
            }
        }

        /// <summary>
        /// reads lines and reconstructs newlines appropriately
        /// </summary>
        /// <returns>the stdin message string</returns>
        public static CicadaMsg ReadMsg()
        {
            StringBuilder message = new StringBuilder();
            Stream inputStream = Console.OpenStandardInput();

            do
            {
                List<byte> bytes = new List<byte>();
                do
                {
                    byte[] _bytes = new byte[1];
                    int outputLength = inputStream.Read(_bytes, 0, 1);
                    if (outputLength < 1 || _bytes[0] == 10)
                        break;

                    bytes.AddRange(_bytes);
                }
                while (true);

                string line = Encoding.UTF8.GetString(bytes.ToArray()).TrimEnd('\r');

                if (string.IsNullOrEmpty(line) || !line.StartsWith("cicadaMsg:", StringComparison.CurrentCultureIgnoreCase))
                    continue;

                if (line == "cicadaMsg:end")
                    break;

                message.AppendLine(line.Replace("cicadaMsg:", ""));
            }
            while (true);

            try
            {
                return JsonConvert.DeserializeObject<CicadaMsg>(message.ToString());
            }
            catch(Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// sent pid to cicada
        /// </summary>
        private static void SendPid()
        {
            Process currentProcess = Process.GetCurrentProcess();
            int pid = currentProcess.Id;
            SendMsg(new CicadaMsg() { pid = pid.ToString() });
        }

        public static void Sync()
        {
            SendMsg(new CicadaMsg() { command = "sync" });
        }

        public static void Ack(string msgId)
        {
            SendMsg(new CicadaMsg() { command = "ack", id = msgId });
        }

        public static void Fail(string msgId)
        {
            SendMsg(new CicadaMsg() { command = "fail", id = msgId });
        }
    }
}
