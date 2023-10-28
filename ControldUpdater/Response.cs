using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControldUpdater
{

    public class Rootobject
    {
        public object[] body { get; set; }
        public bool success { get; set; }
        public Error error { get; set; }
    }

    public class Error
    {
        public string date { get; set; }
        public string message { get; set; }
        public int code { get; set; }
    }

}
