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
    public class CategoriaDeCuentaLN
    {

        public string Error { set; get; }

        private CategoriaDeCuentaAD oCategoriaDeCuentaAD = new CategoriaDeCuentaAD();

        public bool Agregar(CategoriaDeCuentaEN oREgistroEN, DatosDeConexionEN oDatos)
        {

            if (oCategoriaDeCuentaAD.Agregar(oREgistroEN, oDatos))
            {
                Error = string.Empty;
                return true;
            }
            else {
                Error = oCategoriaDeCuentaAD.Error;
                return false;
            }

        }

        public bool Actualizar(CategoriaDeCuentaEN oREgistroEN, DatosDeConexionEN oDatos)
        {

            if (string.IsNullOrEmpty(oREgistroEN.idCategoriaDeCuenta.ToString()) || oREgistroEN.idCategoriaDeCuenta == 0) {

                this.Error = @"Se debe de seleccionar un elemento de la lista";
                return false;
            }

            if (oCategoriaDeCuentaAD.Actualizar(oREgistroEN, oDatos))
            {
                Error = string.Empty;
                return true;
            }
            else
            {
                Error = oCategoriaDeCuentaAD.Error;
                return false;
            }

        }

        public bool Eliminar(CategoriaDeCuentaEN oREgistroEN, DatosDeConexionEN oDatos)
        {

            if (string.IsNullOrEmpty(oREgistroEN.idCategoriaDeCuenta.ToString()) || oREgistroEN.idCategoriaDeCuenta == 0)
            {

                this.Error = @"Se debe de seleccionar un elemento de la lista";
                return false;
            }

            if (oCategoriaDeCuentaAD.Eliminar(oREgistroEN, oDatos))
            {
                Error = string.Empty;
                return true;
            }
            else
            {
                Error = oCategoriaDeCuentaAD.Error;
                return false;
            }

        }

        public bool Listado(CategoriaDeCuentaEN oREgistroEN, DatosDeConexionEN oDatos)
        {

            if (oCategoriaDeCuentaAD.Listado(oREgistroEN, oDatos))
            {
                Error = string.Empty;
                return true;
            }
            else
            {
                Error = oCategoriaDeCuentaAD.Error;
                return false;
            }

        }

        public bool ListadoPorIdentificador(CategoriaDeCuentaEN oREgistroEN, DatosDeConexionEN oDatos)
        {

            if (oCategoriaDeCuentaAD.ListadoPorIdentificador(oREgistroEN, oDatos))
            {
                Error = string.Empty;
                return true;
            }
            else
            {
                Error = oCategoriaDeCuentaAD.Error;
                return false;
            }

        }

        public bool ListadoParaCombos(CategoriaDeCuentaEN oREgistroEN, DatosDeConexionEN oDatos)
        {

            if (oCategoriaDeCuentaAD.ListadoParaCombos(oREgistroEN, oDatos))
            {
                Error = string.Empty;
                return true;
            }
            else
            {
                Error = oCategoriaDeCuentaAD.Error;
                return false;
            }

        }

        public bool ListadoParaReportes(CategoriaDeCuentaEN oREgistroEN, DatosDeConexionEN oDatos)
        {

            if (oCategoriaDeCuentaAD.ListadoParaReportes(oREgistroEN, oDatos))
            {
                Error = string.Empty;
                return true;
            }
            else
            {
                Error = oCategoriaDeCuentaAD.Error;
                return false;
            }

        }

        public bool ValidarRegistroDuplicado(CategoriaDeCuentaEN oREgistroEN, DatosDeConexionEN oDatos, string TipoDeOperacion)
        {

            if (oCategoriaDeCuentaAD.ValidarRegistroDuplicado(oREgistroEN, oDatos, TipoDeOperacion))
            {
                Error = oCategoriaDeCuentaAD.Error;
                return true;
            }
            else
            {
                Error = string.Empty;
                return false;
            }

        }

        public bool ValidarSiElRegistroEstaVinculado(CategoriaDeCuentaEN oREgistroEN, DatosDeConexionEN oDatos, string TipoDeOperacion)
        {

            if (oCategoriaDeCuentaAD.ValidarSiElRegistroEstaVinculado(oREgistroEN, oDatos, TipoDeOperacion))
            {
                Error = oCategoriaDeCuentaAD.Error;
                return true;
            }
            else
            {
                Error = string.Empty;
                return false;
            }

        }

        public DataTable TraerDatos() {

            return oCategoriaDeCuentaAD.TraerDatos();

        }

        public int TotalRegistros()
        {

            return oCategoriaDeCuentaAD.TraerDatos().Rows.Count;

        }


    }
}
