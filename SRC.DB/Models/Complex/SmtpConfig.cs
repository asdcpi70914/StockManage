using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SRC.DB.Models.Complex
{
    public class SmtpConfig
    {
        public string From { get; set; }
        public List<string> NotifyEmailList { get; set; }
        public string MailServer { get; set; }
        public string MailServerAccount { get; set; }
        public string MailServerPassword { get; set; }
        public int Port { get; set; }
    }
}
