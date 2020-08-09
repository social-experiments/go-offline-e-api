using System;
using System.Collections.Generic;
using System.Text;

namespace Educati.Azure.Function.Api.Models
{
    public class EmailRequest
    {
        public string Subject { get; set; }
        public string To { get; set; }

        public string Name { get; set; }
        public string PlainTextContent { get; set; }
        public string HtmlContent { get; set; }
    }
}
