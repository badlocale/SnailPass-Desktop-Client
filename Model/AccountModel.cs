using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnailPass_Desktop.Model
{
    internal class AccountModel
    {
        public string ID { get; set; }
        public string ServiceName { get; set; }
        public string? Login { get; set; }
        public string Password { get; set; }
        public string Nonce { get; set; }
        public string IsFavorite { get; set; }
        public string UserId { get; set; }
    }
}
