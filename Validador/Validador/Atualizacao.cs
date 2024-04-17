using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Validador
{
    internal class Atualizacao
    {
        public static string Versao
        {
            get
            {
                var versao = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
                return (versao.Length == 7 && versao.EndsWith(".0") ? versao.TrimEnd('0').TrimEnd('.') : versao);
            }
        }
    }
}
