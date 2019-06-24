using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Cameo.Services.Interfaces
{
    public interface IEmailService
    {
        bool Send(string to, string subject, string body, List<Tuple<Stream, string>> attachments = null, string cc = null);
    }
}
