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
    public class TipoDeTransaccionLN
    {

        public string Error { set; get; }

        private TipoDeTransaccionAD oTipoDeTransaccionAD = new TipoDeTransaccionAD();

        public bool Agregar(TipoDeTransaccionEN oREgistroEN, DatosDeConexionEN oDatos)
        {

            if (oTipoDeTransaccionAD.Agregar(oREgistroEN, oDatos))
            {
                Error = string.Empty;
                return true;
            }
            else {
                Error = oTipoDeTransaccionAD.Error;
                return false;
            }

        }

        public bool Actualizar(TipoDeTransaccionEN oREgistroEN, DatosDeConexionEN oDatos)
        {

            if (string.IsNullOrEmpty(oREgistroEN.idTipoDeTransaccion.ToString()) || oREgistroEN.idTipoDeTransaccion == 0) {

                this.Error = @"Se debe de seleccionar un elemento de la lista";
                return false;
            }

            if (oTipoDeTransaccionAD.Actualizar(oREgistroEN, oDatos))
            {
                Error = string.Empty;
                return true;
            }
            else
            {
                Error = oTipoDeTransaccionAD.Error;
                return false;
            }

        }

        public bool Eliminar(TipoDeTransaccionEN oREgistroEN, DatosDeConexionEN oDatos)
        {

            if (string.IsNullOrEmpty(oREgistroEN.idTipoDeTransaccion.ToString()) || oREgistroEN.idTipoDeTransaccion == 0)
            {

                this.Error = @"Se debe de seleccionar un elemento de la lista";
                return false;
            }

            if (oTipoDeTransaccionAD.Eliminar(oREgistroEN, oDatos))
            {
                Error = string.Empty;
                return true;
            }
            else
            {
                Error = oTipoDeTransaccionAD.Error;
                return false;
            }

        }

        public bool Listado(TipoDeTransaccionEN oREgistroEN, DatosDeConexionEN oDatos)
        {

            if (oTipoDeTransaccionAD.Listado(oREgistroEN, oDatos))
            {
                Error = string.Empty;
                return true;
            }
            else
            {
                Error = oTipoDeTransaccionAD.Error;
                return false;
            }

        }

        public bool ListadoPorIdentificador(TipoDeTransaccionEN oREgistroEN, DatosDeConexionEN oDatos)
        {

            if (oTipoDeTransaccionAD.ListadoPorIdentificador(oREgistroEN, oDatos))
            {
                Error = string.Empty;
                return true;
            }
            else
            {
                Error = oTipoDeTransaccionAD.Error;
                return false;
            }

        }

        public bool ListadoParaCombos(TipoDeTransaccionEN oREgistroEN, DatosDeConexionEN oDatos)
        {

            if (oTipoDeTransaccionAD.ListadoParaCombos(oREgistroEN, oDatos))
            {
                Error = string.Empty;
                return true;
            }
            else
            {
                Error = oTipoDeTransaccionAD.Error;
                return false;
            }

        }

        public bool ListadoParaReportes(TipoDeTransaccionEN oREgistroEN, DatosDeConexionEN oDatos)
        {

            if (oTipoDeTransaccionAD.ListadoParaReportes(oREgistroEN, oDatos))
            {
                Error = string.Empty;
                return true;
            }
            else
            {
                Error = oTipoDeTransaccionAD.Error;
                return false;
            }

        }

        public bool ValidarRegistroDuplicado(TipoDeTransaccionEN oREgistroEN, DatosDeConexionEN oDatos, string TipoDeOperacion)
        {

            if (oTipoDeTransaccionAD.ValidarRegistroDuplicado(oREgistroEN, oDatos, TipoDeOperacion))
            {
                Error = oTipoDeTransaccionAD.Error;
                return true;
            }
            else
            {
                Error = string.Empty;
                return false;
            }

        }

        public bool ValidarSiElRegistroEstaVinculado(TipoDeTransaccionEN oREgistroEN, DatosDeConexionEN oDatos, string TipoDeOperacion)
        {

            if (oTipoDeTransaccionAD.ValidarSiElRegistroEstaVinculado(oREgistroEN, oDatos, TipoDeOperacion))
            {
                Error = oTipoDeTransaccionAD.Error;
                return true;
            }
            else
            {
                Error = string.Empty;
                return false;
            }

        }

        public DataTable TraerDatos() {

            return oTipoDeTransaccionAD.TraerDatos();

        }

        public int TotalRegistros() {
            return oTipoDeTransaccionAD.TraerDatos().Rows.Count;
        }



    }
}
