using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidad
{
    public class TransaccionDetalleEN
    {      
        public int idTransaccionDetalle { set; get; }
        public decimal Debe { set; get; }
        public decimal Haber { set; get; }
        
        public int IdUsuarioDeCreacion { set; get; }
        public DateTime FechaDeCreacion { set; get; }
        public int IdUsuarioDeModificacion { set; get; }
        public DateTime FechaDeModificacion { set; get; }

        public LoginEN oLoginEN = new LoginEN();
        public CuentaEN oCuentaEN = new CuentaEN();
        public TransaccionEN oTransaccionesEN = new TransaccionEN();

        public string Where { set; get; }
        public string OrderBy { set; get; }
        public string TituloDelReporte { set; get; }

    }
}
