using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidad
{
    public class TransaccionEN
    {       
        public int idTransacciones { set; get; }
        public DateTime Fecha { set; get; }
        public int NoCuenta { set; get; }
        public string Concepto { set; get; }
        public decimal Valor { set; get; }
        public string NumeroDeTransaccion { set; get; }
        public string Estado { set; get; }

        public int IdUsuarioDeCreacion { set; get; }
        public DateTime FechaDeCreacion { set; get; }
        public int IdUsuarioDeModificacion { set; get; }
        public DateTime FechaDeModificacion { set; get; }

        public LoginEN oLoginEN = new LoginEN();
        public TipoDeTransaccionEN oTipoDeTransaccionEN = new TipoDeTransaccionEN();

        public string Where { set; get; }
        public string OrderBy { set; get; }
        public string TituloDelReporte { set; get; }

    }
}
