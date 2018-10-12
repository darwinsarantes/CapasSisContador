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
    public class CuentaLN
    {

        public string Error { set; get; }

        private CuentaAD oCuentaAD = new CuentaAD();

        public bool Agregar(CuentaEN oREgistroEN, DatosDeConexionEN oDatos)
        {

            if (oCuentaAD.Agregar(oREgistroEN, oDatos))
            {
                Error = string.Empty;
                return true;
            }
            else {
                Error = oCuentaAD.Error;
                return false;
            }

        }

        public bool Actualizar(CuentaEN oREgistroEN, DatosDeConexionEN oDatos)
        {

            if (string.IsNullOrEmpty(oREgistroEN.NoCuenta.ToString()) || oREgistroEN.NoCuenta == 0) {

                this.Error = @"Se debe de seleccionar un elemento de la lista";
                return false;
            }

            if (oCuentaAD.Actualizar(oREgistroEN, oDatos))
            {
                Error = string.Empty;
                return true;
            }
            else
            {
                Error = oCuentaAD.Error;
                return false;
            }

        }

        public bool Eliminar(CuentaEN oREgistroEN, DatosDeConexionEN oDatos)
        {

            if (string.IsNullOrEmpty(oREgistroEN.NoCuenta.ToString()) || oREgistroEN.NoCuenta == 0)
            {

                this.Error = @"Se debe de seleccionar un elemento de la lista";
                return false;
            }

            if (oCuentaAD.Eliminar(oREgistroEN, oDatos))
            {
                Error = string.Empty;
                return true;
            }
            else
            {
                Error = oCuentaAD.Error;
                return false;
            }

        }

        public bool Listado(CuentaEN oREgistroEN, DatosDeConexionEN oDatos)
        {

            if (oCuentaAD.Listado(oREgistroEN, oDatos))
            {
                Error = string.Empty;
                return true;
            }
            else
            {
                Error = oCuentaAD.Error;
                return false;
            }

        }

        public bool TraerInformacionDelAsociado(CuentaEN oREgistroEN, DatosDeConexionEN oDatos)
        {

            if (oCuentaAD.TraerInformacionDelAsociado(oREgistroEN, oDatos))
            {
                Error = string.Empty;
                return true;
            }
            else
            {
                Error = oCuentaAD.Error;
                return false;
            }

        }

        public bool ListadoPorIdentificador(CuentaEN oREgistroEN, DatosDeConexionEN oDatos)
        {

            if (oCuentaAD.ListadoPorIdentificador(oREgistroEN, oDatos))
            {
                Error = string.Empty;
                return true;
            }
            else
            {
                Error = oCuentaAD.Error;
                return false;
            }

        }

        public bool ListadoParaCombos(CuentaEN oREgistroEN, DatosDeConexionEN oDatos)
        {

            if (oCuentaAD.ListadoParaCombos(oREgistroEN, oDatos))
            {
                Error = string.Empty;
                return true;
            }
            else
            {
                Error = oCuentaAD.Error;
                return false;
            }

        }

        public bool ListadoParaReportes(CuentaEN oREgistroEN, DatosDeConexionEN oDatos)
        {

            if (oCuentaAD.ListadoParaReportes(oREgistroEN, oDatos))
            {
                Error = string.Empty;
                return true;
            }
            else
            {
                Error = oCuentaAD.Error;
                return false;
            }

        }

        public bool ValidarRegistroDuplicadoPorElIdentificadorDeLaCuenta(CuentaEN oREgistroEN, DatosDeConexionEN oDatos, string TipoDeOperacion)
        {

            if (oCuentaAD.ValidarRegistroDuplicadoPorElIdentificadorDeLaCuenta(oREgistroEN, oDatos, TipoDeOperacion))
            {
                Error = oCuentaAD.Error;
                return true;
            }
            else
            {
                Error = string.Empty;
                return false;
            }

        }
        public bool ValidarRegistroDuplicadoPorCategoria(CuentaEN oREgistroEN, DatosDeConexionEN oDatos, string TipoDeOperacion)
        {

            if (oCuentaAD.ValidarRegistroDuplicadoPorCategoria(oREgistroEN, oDatos, TipoDeOperacion))
            {
                Error = oCuentaAD.Error;
                return true;
            }
            else
            {
                Error = string.Empty;
                return false;
            }

        }
        public bool ValidarRegistroDuplicadoPorDescripcionDeLaCuenta(CuentaEN oREgistroEN, DatosDeConexionEN oDatos, string TipoDeOperacion)
        {

            if (oCuentaAD.ValidarRegistroDuplicadoPorDescripcionDeLaCuenta(oREgistroEN, oDatos, TipoDeOperacion))
            {
                Error = oCuentaAD.Error;
                return true;
            }
            else
            {
                Error = string.Empty;
                return false;
            }

        }
        public bool ValidarSiElRegistroEstaVinculado(CuentaEN oREgistroEN, DatosDeConexionEN oDatos, string TipoDeOperacion)
        {

            if (oCuentaAD.ValidarSiElRegistroEstaVinculado(oREgistroEN, oDatos, TipoDeOperacion))
            {
                Error = oCuentaAD.Error;
                return true;
            }
            else
            {
                Error = string.Empty;
                return false;
            }

        }
        public bool EvaluarSiElRegistrosEstaAsociado(CuentaEN oREgistroEN, DatosDeConexionEN oDatos, string TipoDeOperacion)
        {

            if (oCuentaAD.EvaluarSiElRegistrosEstaAsociado(oREgistroEN, oDatos, TipoDeOperacion))
            {
                Error = oCuentaAD.Error;
                return true;
            }
            else
            {
                Error = string.Empty;
                return false;
            }

        }

        public DataTable TraerDatos() {

            return oCuentaAD.TraerDatos();

        }

        public int TotalRegistros()
        {
            return oCuentaAD.TraerDatos().Rows.Count;
        }
        

    }
}
