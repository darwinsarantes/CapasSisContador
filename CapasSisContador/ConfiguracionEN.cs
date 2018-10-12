using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidad
{
    public class ConfiguracionEN
    {
        //IdConfiguracion, RutaRespaldos, RutaRespaldosDeExcel
        public int IdConfiguracion { set; get; }
        public string RutaRespaldos { set; get; }
        public string RutaRespaldosDeExcel { set; get; }
        public int NivelesDeLaCuenta { set; get; }
        public string PathMysSQLDump { set; get; }
        public string PathMySQL { set; get; }

        public LoginEN oLoginEN = new LoginEN();

        public string Where { set; get; }
        public string OrderBy { set; get; }

    }
}
