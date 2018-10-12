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
    public class TransaccionLN
    {

        public string Error { set; get; }

        private TransaccionAD oTransaccionAD = new TransaccionAD();

        public bool Agregar(TransaccionEN oREgistroEN, DatosDeConexionEN oDatos)
        {

            if (oTransaccionAD.Agregar(oREgistroEN, oDatos))
            {
                Error = string.Empty;
                return true;
            }
            else {
                Error = oTransaccionAD.Error;
                return false;
            }

        }

        public bool Actualizar(TransaccionEN oREgistroEN, DatosDeConexionEN oDatos)
        {

            if (string.IsNullOrEmpty(oREgistroEN.idTransacciones.ToString()) || oREgistroEN.idTransacciones == 0) {

                this.Error = @"Se debe de seleccionar un elemento de la lista";
                return false;
            }

            if (oTransaccionAD.Actualizar(oREgistroEN, oDatos))
            {
                Error = string.Empty;
                return true;
            }
            else
            {
                Error = oTransaccionAD.Error;
                return false;
            }

        }

        public bool Eliminar(TransaccionEN oREgistroEN, DatosDeConexionEN oDatos)
        {

            if (string.IsNullOrEmpty(oREgistroEN.idTransacciones.ToString()) || oREgistroEN.idTransacciones == 0)
            {

                this.Error = @"Se debe de seleccionar un elemento de la lista";
                return false;
            }

            if (oTransaccionAD.Eliminar(oREgistroEN, oDatos))
            {
                Error = string.Empty;
                return true;
            }
            else
            {
                Error = oTransaccionAD.Error;
                return false;
            }

        }

        public bool Listado(TransaccionEN oREgistroEN, DatosDeConexionEN oDatos)
        {

            if (oTransaccionAD.Listado(oREgistroEN, oDatos))
            {
                Error = string.Empty;
                return true;
            }
            else
            {
                Error = oTransaccionAD.Error;
                return false;
            }

        }

        public bool ListadoPorIdentificador(TransaccionEN oREgistroEN, DatosDeConexionEN oDatos)
        {

            if (oTransaccionAD.ListadoPorIdentificador(oREgistroEN, oDatos))
            {
                Error = string.Empty;
                return true;
            }
            else
            {
                Error = oTransaccionAD.Error;
                return false;
            }

        }

        public bool ListadoParaCombos(TransaccionEN oREgistroEN, DatosDeConexionEN oDatos)
        {

            if (oTransaccionAD.ListadoParaCombos(oREgistroEN, oDatos))
            {
                Error = string.Empty;
                return true;
            }
            else
            {
                Error = oTransaccionAD.Error;
                return false;
            }

        }

        public bool ListadoParaReportes(TransaccionEN oREgistroEN, DatosDeConexionEN oDatos)
        {

            if (oTransaccionAD.ListadoParaReportes(oREgistroEN, oDatos))
            {
                Error = string.Empty;
                return true;
            }
            else
            {
                Error = oTransaccionAD.Error;
                return false;
            }

        }

        public bool ValidarRegistroDuplicadoDeLaTransaccion(TransaccionEN oREgistroEN, DatosDeConexionEN oDatos, string TipoDeOperacion)
        {

            if (oTransaccionAD.ValidarRegistroDuplicadoDeLaTransaccion(oREgistroEN, oDatos, TipoDeOperacion))
            {
                Error = oTransaccionAD.Error;
                return true;
            }
            else
            {
                Error = string.Empty;
                return false;
            }

        }

        public bool ValidarSiElRegistroEstaVinculado(TransaccionEN oREgistroEN, DatosDeConexionEN oDatos, string TipoDeOperacion)
        {

            if (oTransaccionAD.ValidarSiElRegistroEstaVinculado(oREgistroEN, oDatos, TipoDeOperacion))
            {
                Error = oTransaccionAD.Error;
                return true;
            }
            else
            {
                Error = string.Empty;
                return false;
            }

        }

        public DataTable TraerDatos() {

            return oTransaccionAD.TraerDatos();

        }

        public int TotalRegistros() {
            return oTransaccionAD.TraerDatos().Rows.Count;
        }



    }
}
