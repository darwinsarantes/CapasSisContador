using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entidad;
using Acceso;
using System.Data;

namespace Logica
{
    public class GrupoDeCuentasLN
    {

        public string Error { set; get; }

        private GrupoDeCuentasAD oGrupoDeCuentasAD = new GrupoDeCuentasAD();

        public bool Agregar(GrupoDeCuentasEN oREgistroEN, DatosDeConexionEN oDatos)
        {

            if (oGrupoDeCuentasAD.Agregar(oREgistroEN, oDatos))
            {
                Error = string.Empty;
                return true;
            }
            else {
                Error = oGrupoDeCuentasAD.Error;
                return false;
            }

        }

        public bool Actualizar(GrupoDeCuentasEN oREgistroEN, DatosDeConexionEN oDatos)
        {

            if (string.IsNullOrEmpty(oREgistroEN.idGrupoDeCuentas.ToString()) || oREgistroEN.idGrupoDeCuentas == 0) {

                this.Error = @"Se debe de seleccionar un elemento de la lista";
                return false;
            }

            if (oGrupoDeCuentasAD.Actualizar(oREgistroEN, oDatos))
            {
                Error = string.Empty;
                return true;
            }
            else
            {
                Error = oGrupoDeCuentasAD.Error;
                return false;
            }

        }

        public bool Eliminar(GrupoDeCuentasEN oREgistroEN, DatosDeConexionEN oDatos)
        {

            if (string.IsNullOrEmpty(oREgistroEN.idGrupoDeCuentas.ToString()) || oREgistroEN.idGrupoDeCuentas == 0)
            {

                this.Error = @"Se debe de seleccionar un elemento de la lista";
                return false;
            }

            if (oGrupoDeCuentasAD.Eliminar(oREgistroEN, oDatos))
            {
                Error = string.Empty;
                return true;
            }
            else
            {
                Error = oGrupoDeCuentasAD.Error;
                return false;
            }

        }

        public bool Listado(GrupoDeCuentasEN oREgistroEN, DatosDeConexionEN oDatos)
        {

            if (oGrupoDeCuentasAD.Listado(oREgistroEN, oDatos))
            {
                Error = string.Empty;
                return true;
            }
            else
            {
                Error = oGrupoDeCuentasAD.Error;
                return false;
            }

        }

        public bool ListadoPorIdentificador(GrupoDeCuentasEN oREgistroEN, DatosDeConexionEN oDatos)
        {

            if (oGrupoDeCuentasAD.ListadoPorIdentificador(oREgistroEN, oDatos))
            {
                Error = string.Empty;
                return true;
            }
            else
            {
                Error = oGrupoDeCuentasAD.Error;
                return false;
            }

        }

        public bool ListadoParaCombos(GrupoDeCuentasEN oREgistroEN, DatosDeConexionEN oDatos)
        {

            if (oGrupoDeCuentasAD.ListadoParaCombos(oREgistroEN, oDatos))
            {
                Error = string.Empty;
                return true;
            }
            else
            {
                Error = oGrupoDeCuentasAD.Error;
                return false;
            }

        }

        public bool ListadoParaReportes(GrupoDeCuentasEN oREgistroEN, DatosDeConexionEN oDatos)
        {

            if (oGrupoDeCuentasAD.ListadoParaReportes(oREgistroEN, oDatos))
            {
                Error = string.Empty;
                return true;
            }
            else
            {
                Error = oGrupoDeCuentasAD.Error;
                return false;
            }

        }

        public bool ValidarRegistroDuplicado(GrupoDeCuentasEN oREgistroEN, DatosDeConexionEN oDatos, string TipoDeOperacion)
        {

            if (oGrupoDeCuentasAD.ValidarRegistroDuplicado(oREgistroEN, oDatos, TipoDeOperacion))
            {
                Error = oGrupoDeCuentasAD.Error;
                return true;
            }
            else
            {
                Error = string.Empty;
                return false;
            }

        }

        public bool ValidarSiElRegistroEstaVinculado(GrupoDeCuentasEN oREgistroEN, DatosDeConexionEN oDatos, string TipoDeOperacion)
        {

            if (oGrupoDeCuentasAD.ValidarSiElRegistroEstaVinculado(oREgistroEN, oDatos, TipoDeOperacion))
            {
                Error = oGrupoDeCuentasAD.Error;
                return true;
            }
            else
            {
                Error = string.Empty;
                return false;
            }

        }

        public DataTable TraerDatos() {

            return oGrupoDeCuentasAD.TraerDatos();

        }

        public int TotalRegistros() {
            return oGrupoDeCuentasAD.TraerDatos().Rows.Count;
        }



    }
}
