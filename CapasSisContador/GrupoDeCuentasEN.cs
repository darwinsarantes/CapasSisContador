using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidad
{
    public class GrupoDeCuentasEN
    {
        
        public int idGrupoDeCuentas { set; get; }
        public string DescGrupoDeCuentas { set; get; }
        public string Debitos { set; get; }
        public string Creditos { set; get; }
        

        public int IdUsuarioDeCreacion { set; get; }
        public DateTime FechaDeCreacion { set; get; }
        public int IdUsuarioDeModificacion { set; get; }
        public DateTime FechaDeModificacion { set; get; }

        public LoginEN oLoginEN = new LoginEN();

        public string Where { set; get; }
        public string OrderBy { set; get; }
        public string TituloDelReporte { set; get; }

    }
}
