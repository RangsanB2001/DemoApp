using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationModels
{
    public class DataUserAll
    {
        public int id { get; set; }
        public string username { get; set; } = string.Empty;

        public string password { get; set; } = string.Empty;
    }
}
