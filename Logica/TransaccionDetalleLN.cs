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
    public class TransaccionDetalleLN
    {

        public string Error { set; get; }

        private TransaccionDetalleAD oTransaccionDetalleAD = new TransaccionDetalleAD();

        public bool Agregar(TransaccionDetalleEN oREgistroEN, DatosDeConexionEN oDatos)
        {

            if (oTransaccionDetalleAD.Agregar(oREgistroEN, oDatos))
            {
                Error = string.Empty;
                return true;
            }
            else {
                Error = oTransaccionDetalleAD.Error;
                return false;
            }

        }

        public bool Actualizar(TransaccionDetalleEN oREgistroEN, DatosDeConexionEN oDatos)
        {

            if (string.IsNullOrEmpty(oREgistroEN.idTransaccionDetalle.ToString()) || oREgistroEN.idTransaccionDetalle == 0) {

                this.Error = @"Se debe de seleccionar un elemento de la lista";
                return false;
            }

            if (oTransaccionDetalleAD.Actualizar(oREgistroEN, oDatos))
            {
                Error = string.Empty;
                return true;
            }
            else
            {
                Error = oTransaccionDetalleAD.Error;
                return false;
            }

        }

        public bool Eliminar(TransaccionDetalleEN oREgistroEN, DatosDeConexionEN oDatos)
        {

            if (string.IsNullOrEmpty(oREgistroEN.idTransaccionDetalle.ToString()) || oREgistroEN.idTransaccionDetalle == 0)
            {

                this.Error = @"Se debe de seleccionar un elemento de la lista";
                return false;
            }

            if (oTransaccionDetalleAD.Eliminar(oREgistroEN, oDatos))
            {
                Error = string.Empty;
                return true;
            }
            else
            {
                Error = oTransaccionDetalleAD.Error;
                return false;
            }

        }

        public bool Listado(TransaccionDetalleEN oREgistroEN, DatosDeConexionEN oDatos)
        {

            if (oTransaccionDetalleAD.Listado(oREgistroEN, oDatos))
            {
                Error = string.Empty;
                return true;
            }
            else
            {
                Error = oTransaccionDetalleAD.Error;
                return false;
            }

        }

        public bool ListadoPorIdentificador(TransaccionDetalleEN oREgistroEN, DatosDeConexionEN oDatos)
        {

            if (oTransaccionDetalleAD.ListadoPorIdentificador(oREgistroEN, oDatos))
            {
                Error = string.Empty;
                return true;
            }
            else
            {
                Error = oTransaccionDetalleAD.Error;
                return false;
            }

        }

        public bool ListadoParaCombos(TransaccionDetalleEN oREgistroEN, DatosDeConexionEN oDatos)
        {

            if (oTransaccionDetalleAD.ListadoParaCombos(oREgistroEN, oDatos))
            {
                Error = string.Empty;
                return true;
            }
            else
            {
                Error = oTransaccionDetalleAD.Error;
                return false;
            }

        }

        public bool ListadoParaReportes(TransaccionDetalleEN oREgistroEN, DatosDeConexionEN oDatos)
        {

            if (oTransaccionDetalleAD.ListadoParaReportes(oREgistroEN, oDatos))
            {
                Error = string.Empty;
                return true;
            }
            else
            {
                Error = oTransaccionDetalleAD.Error;
                return false;
            }

        }

        public bool ValidarRegistroDuplicado(TransaccionDetalleEN oREgistroEN, DatosDeConexionEN oDatos, string TipoDeOperacion)
        {

            if (oTransaccionDetalleAD.ValidarRegistroDuplicado(oREgistroEN, oDatos, TipoDeOperacion))
            {
                Error = oTransaccionDetalleAD.Error;
                return true;
            }
            else
            {
                Error = string.Empty;
                return false;
            }

        }

        public bool ValidarSiElRegistroEstaVinculado(TransaccionDetalleEN oREgistroEN, DatosDeConexionEN oDatos, string TipoDeOperacion)
        {

            if (oTransaccionDetalleAD.ValidarSiElRegistroEstaVinculado(oREgistroEN, oDatos, TipoDeOperacion))
            {
                Error = oTransaccionDetalleAD.Error;
                return true;
            }
            else
            {
                Error = string.Empty;
                return false;
            }

        }

        public DataTable TraerDatos() {

            return oTransaccionDetalleAD.TraerDatos();

        }

        public int TotalRegistros() {
            return oTransaccionDetalleAD.TraerDatos().Rows.Count;
        }



    }
}
