using System;
using System.Collections.Generic;
using System.Text;

namespace Cicada.Core
{
    public class CicadaConfig
    {
        #region Wechat
        public string CorpID { set; get; }
        public string CorpSecret { set; get; }
        #endregion

        #region Mail
        public string SmtpServer { set; get; }
        public int SmtpPort { set; get; }
        public bool EnableSsl { set; get; }
        public string MailUsername { set; get; }
        public string MailPassword { set; get; }
        #endregion

        #region AliyunMessage
        public string AccessKeyId { set; get; }
        public string AccessKeySecret { set; get; }
        public string SignName { set; get; }
        public string TemplateCode { set; get; }
        #endregion

        #region Callback
        public string ApiUri { set; get; }
        public string ApiType { set; get; }
        public string ContentKey { set; get; }
        public string LevelKey { set; get; }
        public string TouserKey { set; get; }
        public string TopartyKey { set; get; }
        public string TotagKey { set; get; }
        #endregion
    }
}
