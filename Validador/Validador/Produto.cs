using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Validador
{
    internal class Produto
    {
        public string Codigo { get; set; }
        public string Descricao { get; set; }
        public string NCM { get; set; }
        public bool TemMovimento { get; set; }
    }
}
