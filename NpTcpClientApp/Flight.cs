using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NpTcpClientApp
{
    internal class Flight
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string FromCity { get; set; } = "";
        public string ToCity { get; set; } = "";

        public override string ToString()
        {
            return $"Flight bane: {Name}\n\tFrom: {FromCity}\n\tTo: {ToCity}";
        }
    }
}
