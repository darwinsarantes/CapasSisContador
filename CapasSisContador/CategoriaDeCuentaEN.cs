using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidad
{
    public class CategoriaDeCuentaEN
    {
        
        public int idCategoriaDeCuenta { set; get; }
        public string DescCategoriaDeCuenta { set; get; }

        public int IdUsuarioDeCreacion { set; get; }
        public DateTime FechaDeCreacion { set; get; }
        public int IdUsuarioDeModificacion { set; get; }
        public DateTime FechaDeModificacion { set; get; }

        public LoginEN oLoginEN = new LoginEN();

        public GrupoDeCuentasEN oGrupoDeCuentasEN = new GrupoDeCuentasEN();

        public string Where { set; get; }
        public string OrderBy { set; get; }
        public string TituloDelReporte { set; get; }

    }
}
