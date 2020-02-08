using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace CicadaAdapter
{
    public class CicadaLaunch
    {
        private static ConcurrentDictionary<string, DateTime> needAckMessageIds = new ConcurrentDictionary<string, DateTime>();

        public static void Launch()
        {
            Task<bool> heartBeatTask = StartWaitHeartBeat();
            heartBeatTask.Start();
        }

        private static Task<bool> StartWaitHeartBeat()
        {
            Task<bool> T = new Task<bool>(() =>
            {
                while (true)
                {
                    try
                    {
                        CicadaMsg msg = StdInAndOut.ReadMsg();
                        if (msg != null)
                        {
                            if (msg.IsHeartBeat())
                                StdInAndOut.Sync();
                            else if (msg.command == "ack" && !string.IsNullOrWhiteSpace(msg.id))
                            {
                                DateTime startTime;
                                needAckMessageIds.TryRemove(msg.id, out startTime);
                            }
                        }
                        else
                        {
                            Thread.Sleep(50);
                        }
                    }
                    catch (Exception ex)
                    {
                        Thread.Sleep(50);
                        //do some thing and log it.
                    }
                }
            });
            return T;
        }


        /// <summary>
        /// Waits the message be ack.
        /// </summary>
        /// <returns><c>true</c>, if message be ack was waited, <c>false</c> otherwise.</returns>
        /// <param name="msgId">Message identifier.</param>
        /// <param name="timeoutMilliseconds">Timeout milliseconds.</param>
        public static bool WaitMsgBeAck(string msgId, int? timeoutMilliseconds)
        {
            if(!needAckMessageIds.ContainsKey(msgId))
            {
                needAckMessageIds[msgId] = DateTime.Now;
            }

            try
            {
                CallWithTimeout(() => waitMsgIdDisappear(msgId), timeoutMilliseconds ?? Int32.MaxValue);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Calls the with timeout.
        /// </summary>
        /// <param name="action">Action.</param>
        /// <param name="timeoutMilliseconds">Timeout milliseconds.</param>
        private static void CallWithTimeout(Action action, int timeoutMilliseconds)
        {
            Thread threadToKill = null;
            Action wrappedAction = () =>
            {
                threadToKill = Thread.CurrentThread;
                action();
            };

            IAsyncResult result = wrappedAction.BeginInvoke(null, null);
            if (result.AsyncWaitHandle.WaitOne(timeoutMilliseconds))
            {
                wrappedAction.EndInvoke(result);
            }
            else
            {
                threadToKill.Abort();
                throw new TimeoutException();
            }
        }

        /// <summary>
        /// Waits the message identifier disappear.
        /// </summary>
        /// <param name="msgId">Message identifier.</param>
        private static void waitMsgIdDisappear(string msgId)
        {
            do
            {
                if (!needAckMessageIds.ContainsKey(msgId))
                    return;
                else
                    Thread.Sleep(10);
            }
            while (true);
        }
    }
}
