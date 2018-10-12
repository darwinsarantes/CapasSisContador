using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using MySql.Data.MySqlClient;
using Entidad;
namespace Acceso
{
    public class CategoriaDeCuentaAD
    {

        public string Error { set; get; }
        private MySqlConnection Cnn = null;
        private MySqlCommand Comando = null;
        private MySqlDataAdapter Adaptador = null;
        private TransaccionesAD oTransaccionesAD = null;
        string Consultas;
        string DescripcionDeOperacion;
        private DataTable DT { set; get; }

        public CategoriaDeCuentaAD(){

            oTransaccionesAD = new TransaccionesAD();
            oTransaccionesAD.modulo = "General";
            oTransaccionesAD.modelo = "CategoriaDeCuentaAD";
            oTransaccionesAD.NombreDelEquipo = Environment.MachineName;
            oTransaccionesAD.Fecha_creacion = System.DateTime.Now;
            oTransaccionesAD.tabla = "CategoriaDeCuenta";

        }

        #region "Funciones para datos dll"

        public bool Agregar(CategoriaDeCuentaEN oRegistroEN, DatosDeConexionEN oDatos) {

            try
            {

                Cnn = new MySqlConnection(TraerCadenaDeConexion(oDatos));
                Cnn.Open();

                Comando = new MySqlCommand();
                Comando.Connection = Cnn;
                Comando.CommandType = CommandType.Text;

                Consultas = @"                
                insert into categoriadecuenta
                (idCategoriaDeCuenta, DescCategoriaDeCuenta, IdUsuarioDeCreacion, FechaDeCreacion, IdUsuarioDeModificacion, FechaDeModificacion, idGrupoDeCuentas)
                values
                (IdentificadorDeCategoriaDeCuenta(), @DescCategoriaDeCuenta, @IdUsuarioDeCreacion, current_timestamp(), @IdUsuarioDeModificacion, current_timestamp(), @idGrupoDeCuentas);

                Select max(idCategoriaDeCuenta) from categoriadecuenta order by idCategoriaDeCuenta desc;";

                Comando.CommandText = Consultas;

                Comando.Parameters.Add(new MySqlParameter("@DescCategoriaDeCuenta", MySqlDbType.VarChar, oRegistroEN.DescCategoriaDeCuenta.Trim().Length)).Value = oRegistroEN.DescCategoriaDeCuenta.Trim();
                Comando.Parameters.Add(new MySqlParameter("@idGrupoDeCuentas", MySqlDbType.Int32)).Value = oRegistroEN.oGrupoDeCuentasEN.idGrupoDeCuentas;
                Comando.Parameters.Add(new MySqlParameter("@IdUsuarioDeCreacion", MySqlDbType.Int32)).Value = oRegistroEN.oLoginEN.idUsuario;
                Comando.Parameters.Add(new MySqlParameter("@IdUsuarioDeModificacion", MySqlDbType.Int32)).Value = oRegistroEN.oLoginEN.idUsuario;
                

                Adaptador = new MySqlDataAdapter();
                DT = new DataTable();

                Adaptador.SelectCommand = Comando;
                Adaptador.Fill(DT);

                oRegistroEN.idCategoriaDeCuenta = Convert.ToInt32(DT.Rows[0].ItemArray[0].ToString());

                DescripcionDeOperacion = string.Format("El registro fue Insertado Correctamente. {0} {1}", Environment.NewLine, InformacionDelRegistro(oRegistroEN));

                //oTransaccionesAD.Agregar(oRegistroEN.oLoginEN.idUsuario, oRegistroEN.oLoginEN.NumeroIP,oRegistroEN.idCategoriaDeCuenta, "AGREGAR", "INFORMACIÓN DE LA CATEGORIA DE CUENTAS AGREGADA CORRECTAMENTE", "CORRECTA", DescripcionDeOperacion, oRegistroEN.oLoginEN.idUsuario, oDatos);
                
                return true;

            }
            catch (Exception ex)
            {
                this.Error = ex.Message;

                DescripcionDeOperacion = string.Format("Se produjo el seguiente error: '{2}' al insertar el registro. {0} {1} ", Environment.NewLine, InformacionDelRegistro(oRegistroEN), ex.Message);
                //oTransaccionesAD.Agregar(oRegistroEN.oLoginEN.idUsuario, oRegistroEN.oLoginEN.NumeroIP, oRegistroEN.idCategoriaDeCuenta, "AGREGAR", "ERROR AL AGREGAR LA INFORMACIÓN DE LA CATEGORIA DE CUENTAS", "ERROR", DescripcionDeOperacion, oRegistroEN.oLoginEN.idUsuario, oDatos);
                
                return false;
            }
            finally {

                if (Cnn != null) {

                    if (Cnn.State == ConnectionState.Open) {

                        Cnn.Close();

                    }

                }

                Cnn = null;
                Comando = null;
                Adaptador = null;
                oTransaccionesAD = null;

            }

        }

        public bool Actualizar(CategoriaDeCuentaEN oRegistroEN, DatosDeConexionEN oDatos)
        {

            try
            {

                Cnn = new MySqlConnection(TraerCadenaDeConexion(oDatos));
                Cnn.Open();

                Comando = new MySqlCommand();
                Comando.Connection = Cnn;
                Comando.CommandType = CommandType.Text;

                Consultas = @"
                UPDATE categoriadecuenta SET
	                DescCategoriaDeCuenta = @DescCategoriaDeCuenta, IdUsuarioDeModificacion = @IdUsuarioDeModificacion, FechaDeModificacion = current_timestamp(), idGrupoDeCuentas = @idGrupoDeCuentas
                WHERE idCategoriaDeCuenta = @idCategoriaDeCuenta;";

                Comando.CommandText = Consultas;

                Comando.Parameters.Add(new MySqlParameter("@idCategoriaDeCuenta", MySqlDbType.Int32)).Value = oRegistroEN.idCategoriaDeCuenta;
                Comando.Parameters.Add(new MySqlParameter("@DescCategoriaDeCuenta", MySqlDbType.VarChar, oRegistroEN.DescCategoriaDeCuenta.Trim().Length)).Value = oRegistroEN.DescCategoriaDeCuenta.Trim();
                Comando.Parameters.Add(new MySqlParameter("@idGrupoDeCuentas", MySqlDbType.Int32)).Value = oRegistroEN.oGrupoDeCuentasEN.idGrupoDeCuentas;                
                Comando.Parameters.Add(new MySqlParameter("@IdUsuarioDeModificacion", MySqlDbType.Int32)).Value = oRegistroEN.oLoginEN.idUsuario;

                Comando.ExecuteNonQuery();
                
                DescripcionDeOperacion = string.Format("El registro fue Actualizado Correctamente. {0} {1}", Environment.NewLine, InformacionDelRegistro(oRegistroEN));

                //oTransaccionesAD.Agregar(oRegistroEN.oLoginEN.idUsuario, oRegistroEN.oLoginEN.NumeroIP, oRegistroEN.idCategoriaDeCuenta, "ACTUALIZAR", "INFORMACIÓN DE LA CATEGORIA DE LA CUENTA ACTUALIZADA", "CORRECTA", DescripcionDeOperacion, oRegistroEN.oLoginEN.idUsuario, oDatos);

                return true;

            }
            catch (Exception ex)
            {
                this.Error = ex.Message;

                DescripcionDeOperacion = string.Format("Se produjo el seguiente error: '{2}' al actualizar el registro. {0} {1} ", Environment.NewLine, InformacionDelRegistro(oRegistroEN), ex.Message);
                //oTransaccionesAD.Agregar(oRegistroEN.oLoginEN.idUsuario, oRegistroEN.oLoginEN.NumeroIP, oRegistroEN.idCategoriaDeCuenta, "ACTUALIZAR", "ERROR AL ACTUALIZAR LA INFORMACIÓN DE LA CATEGORIA DE LA CUENTA", "ERROR", DescripcionDeOperacion, oRegistroEN.oLoginEN.idUsuario, oDatos);

                return false;
            }
            finally
            {

                if (Cnn != null)
                {

                    if (Cnn.State == ConnectionState.Open)
                    {

                        Cnn.Close();

                    }

                }

                Cnn = null;
                Comando = null;
                Adaptador = null;
                oTransaccionesAD = null;

            }

        }

        public bool Eliminar(CategoriaDeCuentaEN oRegistroEN, DatosDeConexionEN oDatos)
        {

            try
            {

                Cnn = new MySqlConnection(TraerCadenaDeConexion(oDatos));
                Cnn.Open();

                Comando = new MySqlCommand();
                Comando.Connection = Cnn;
                Comando.CommandType = CommandType.Text;

                Consultas = @"Delete from CategoriaDeCuenta where idCategoriaDeCuenta = @idCategoriaDeCuenta;";
                Comando.CommandText = Consultas;

                Comando.Parameters.Add(new MySqlParameter("@idCategoriaDeCuenta", MySqlDbType.Int32)).Value = oRegistroEN.idCategoriaDeCuenta;
                
                Comando.ExecuteNonQuery();

                DescripcionDeOperacion = string.Format("El registro fue Eliminado Correctamente. {0} {1}", Environment.NewLine, InformacionDelRegistro(oRegistroEN));

                //oTransaccionesAD.Agregar(oRegistroEN.oLoginEN.idUsuario, oRegistroEN.oLoginEN.NumeroIP, oRegistroEN.idCategoriaDeCuenta, "ELIMINAR", "INFORMACIÓN DE LA CATEGORIA DE LA CUENTA ELIMINADA", "CORRECTA", DescripcionDeOperacion, oRegistroEN.oLoginEN.idUsuario, oDatos);

                return true;

            }
            catch (Exception ex)
            {
                this.Error = ex.Message;

                DescripcionDeOperacion = string.Format("Se produjo el seguiente error: '{2}' al eliminar el registro. {0} {1} ", Environment.NewLine, InformacionDelRegistro(oRegistroEN), ex.Message);
                //oTransaccionesAD.Agregar(oRegistroEN.oLoginEN.idUsuario, oRegistroEN.oLoginEN.NumeroIP, oRegistroEN.idCategoriaDeCuenta, "ELIMINAR", "ERROR AL ELIMINAR LA INFORMACIÓN DE LA CATEGORIA DE LA CUENTA", "ERROR", DescripcionDeOperacion, oRegistroEN.oLoginEN.idUsuario, oDatos);

                return false;
            }
            finally
            {

                if (Cnn != null)
                {

                    if (Cnn.State == ConnectionState.Open)
                    {

                        Cnn.Close();

                    }

                }

                Cnn = null;
                Comando = null;
                Adaptador = null;
                oTransaccionesAD = null;

            }

        }

        public bool Listado(CategoriaDeCuentaEN oRegistroEN, DatosDeConexionEN oDatos)
        {

            try
            {

                Cnn = new MySqlConnection(TraerCadenaDeConexion(oDatos));
                Cnn.Open();

                Comando = new MySqlCommand();
                Comando.Connection = Cnn;
                Comando.CommandType = CommandType.Text;

                Consultas = string.Format(@"select idCategoriaDeCuenta,cc.idGrupoDeCuentas, DescCategoriaDeCuenta, gc.DescGrupoDeCuentas, 
                cc.IdUsuarioDeCreacion, cc.FechaDeCreacion, cc.IdUsuarioDeModificacion, cc.FechaDeModificacion 
                from categoriadecuenta as cc
                inner join grupodecuentas as gc on gc.idGrupoDeCuentas = cc.idGrupoDeCuentas
                where idCategoriaDeCuenta > 0 {0} {1} ", oRegistroEN.Where, oRegistroEN.OrderBy);
                Comando.CommandText = Consultas;
                
                Adaptador = new MySqlDataAdapter();
                DT = new DataTable();

                Adaptador.SelectCommand = Comando;
                Adaptador.Fill(DT);

                return true;

            }
            catch (Exception ex)
            {
                this.Error = ex.Message;
                
                return false;
            }
            finally
            {

                if (Cnn != null)
                {

                    if (Cnn.State == ConnectionState.Open)
                    {

                        Cnn.Close();

                    }

                }

                Cnn = null;
                Comando = null;
                Adaptador = null;              

            }

        }

        public bool ListadoPorIdentificador(CategoriaDeCuentaEN oRegistroEN, DatosDeConexionEN oDatos)
        {

            try
            {

                Cnn = new MySqlConnection(TraerCadenaDeConexion(oDatos));
                Cnn.Open();

                Comando = new MySqlCommand();
                Comando.Connection = Cnn;
                Comando.CommandType = CommandType.Text;

                Consultas = string.Format(@"select idCategoriaDeCuenta,cc.idGrupoDeCuentas, DescCategoriaDeCuenta, gc.DescGrupoDeCuentas, 
                cc.IdUsuarioDeCreacion, cc.FechaDeCreacion, cc.IdUsuarioDeModificacion, cc.FechaDeModificacion 
                from categoriadecuenta as cc
                inner join grupodecuentas as gc on gc.idGrupoDeCuentas = cc.idGrupoDeCuentas
                where idCategoriaDeCuenta >= {0} ", oRegistroEN.idCategoriaDeCuenta);
                Comando.CommandText = Consultas;

                Adaptador = new MySqlDataAdapter();
                DT = new DataTable();

                Adaptador.SelectCommand = Comando;
                Adaptador.Fill(DT);

                return true;

            }
            catch (Exception ex)
            {
                this.Error = ex.Message;

                return false;
            }
            finally
            {

                if (Cnn != null)
                {

                    if (Cnn.State == ConnectionState.Open)
                    {

                        Cnn.Close();

                    }

                }

                Cnn = null;
                Comando = null;
                Adaptador = null;

            }

        }

        public bool ListadoParaCombos(CategoriaDeCuentaEN oRegistroEN, DatosDeConexionEN oDatos)
        {

            try
            {

                Cnn = new MySqlConnection(TraerCadenaDeConexion(oDatos));
                Cnn.Open();

                Comando = new MySqlCommand();
                Comando.Connection = Cnn;
                Comando.CommandType = CommandType.Text;

                Consultas = string.Format(@"select idCategoriaDeCuenta,cc.idGrupoDeCuentas, DescCategoriaDeCuenta, gc.DescGrupoDeCuentas, 
                cc.IdUsuarioDeCreacion, cc.FechaDeCreacion, cc.IdUsuarioDeModificacion, cc.FechaDeModificacion 
                from categoriadecuenta as cc
                inner join grupodecuentas as gc on gc.idGrupoDeCuentas = cc.idGrupoDeCuentas
                where idCategoriaDeCuenta > 0 {0} {1}; ", oRegistroEN.Where, oRegistroEN.OrderBy);
                Comando.CommandText = Consultas;

                Adaptador = new MySqlDataAdapter();
                DT = new DataTable();

                Adaptador.SelectCommand = Comando;
                Adaptador.Fill(DT);

                return true;

            }
            catch (Exception ex)
            {
                this.Error = ex.Message;

                return false;
            }
            finally
            {

                if (Cnn != null)
                {

                    if (Cnn.State == ConnectionState.Open)
                    {

                        Cnn.Close();

                    }

                }

                Cnn = null;
                Comando = null;
                Adaptador = null;

            }

        }

        public bool ListadoParaReportes(CategoriaDeCuentaEN oRegistroEN, DatosDeConexionEN oDatos)
        {

            try
            {

                Cnn = new MySqlConnection(TraerCadenaDeConexion(oDatos));
                Cnn.Open();

                Comando = new MySqlCommand();
                Comando.Connection = Cnn;
                Comando.CommandType = CommandType.Text;

                Consultas = string.Format(@"select idCategoriaDeCuenta,cc.idGrupoDeCuentas, DescCategoriaDeCuenta, gc.DescGrupoDeCuentas, 
                cc.IdUsuarioDeCreacion, cc.FechaDeCreacion, cc.IdUsuarioDeModificacion, cc.FechaDeModificacion 
                from categoriadecuenta as cc
                inner join grupodecuentas as gc on gc.idGrupoDeCuentas = cc.idGrupoDeCuentas
                where idCategoriaDeCuenta > 0 {0} {1} ", oRegistroEN.Where, oRegistroEN.OrderBy);
                Comando.CommandText = Consultas;

                Adaptador = new MySqlDataAdapter();
                DT = new DataTable();

                Adaptador.SelectCommand = Comando;
                Adaptador.Fill(DT);

                return true;

            }
            catch (Exception ex)
            {
                this.Error = ex.Message;

                return false;
            }
            finally
            {

                if (Cnn != null)
                {

                    if (Cnn.State == ConnectionState.Open)
                    {

                        Cnn.Close();

                    }

                }

                Cnn = null;
                Comando = null;
                Adaptador = null;

            }

        }

        #endregion

        #region "Funciones de Validación"

        public bool ValidarRegistroDuplicado(CategoriaDeCuentaEN oRegistroEN, DatosDeConexionEN oDatos, string TipoDeOperacion)
        {

            try
            {

                Cnn = new MySqlConnection(TraerCadenaDeConexion(oDatos));
                Cnn.Open();

                Comando = new MySqlCommand();
                Comando.Connection = Cnn;
                Comando.CommandType = CommandType.Text;

                switch (TipoDeOperacion.Trim().ToUpper()){

                    case "AGREGAR":

                        Consultas = @"SELECT CASE WHEN EXISTS(SELECT idCategoriaDeCuenta FROM CategoriaDeCuenta WHERE upper(trim(DescCategoriaDeCuenta)) = upper(@DescCategoriaDeCuenta) and idGrupoDeCuentas = @idGrupoDeCuentas) THEN 1 ELSE 0 END AS 'RES'";
                        Comando.Parameters.Add(new MySqlParameter("@DescCategoriaDeCuenta", MySqlDbType.VarChar, oRegistroEN.DescCategoriaDeCuenta.Trim().Length)).Value = oRegistroEN.DescCategoriaDeCuenta.Trim();
                        Comando.Parameters.Add(new MySqlParameter("@idGrupoDeCuentas", MySqlDbType.Int32)).Value = oRegistroEN.oGrupoDeCuentasEN.idGrupoDeCuentas;

                        break;

                    case "ACTUALIZAR":

                        Consultas = @"SELECT CASE WHEN EXISTS(SELECT idCategoriaDeCuenta FROM CategoriaDeCuenta WHERE upper(trim(DescCategoriaDeCuenta)) = upper(@DescCategoriaDeCuenta) and idGrupoDeCuentas = @idGrupoDeCuentas <> @idCategoriaDeCuenta) THEN 1 ELSE 0 END AS 'RES'";
                        Comando.Parameters.Add(new MySqlParameter("@DescCategoriaDeCuenta", MySqlDbType.VarChar, oRegistroEN.DescCategoriaDeCuenta.Trim().Length)).Value = oRegistroEN.DescCategoriaDeCuenta.Trim();
                        Comando.Parameters.Add(new MySqlParameter("@idGrupoDeCuentas", MySqlDbType.Int32)).Value = oRegistroEN.oGrupoDeCuentasEN.idGrupoDeCuentas;
                        Comando.Parameters.Add(new MySqlParameter("@idCategoriaDeCuenta", MySqlDbType.Int32)).Value = oRegistroEN.idCategoriaDeCuenta;

                        break;

                    default:
                        throw new ArgumentException( "La aperación solicitada no esta disponible");                        

                }
                
                Comando.CommandText = Consultas;

                Adaptador = new MySqlDataAdapter();
                DT = new DataTable();

                Adaptador.SelectCommand = Comando;
                Adaptador.Fill(DT);

                if (Convert.ToInt32(DT.Rows[0]["RES"].ToString()) > 0) {
                    
                    DescripcionDeOperacion = string.Format("Ya existe información del Registro dentro de nuestro sistema: {0} {1}", Environment.NewLine, InformacionDelRegistro(oRegistroEN));
                    this.Error = DescripcionDeOperacion;
                    return true;

                }

                return false;

            }
            catch (Exception ex)
            {
                this.Error = ex.Message;

                DescripcionDeOperacion = string.Format("Se produjo el seguiente error: '{2}' al validar el registro. {0} {1} ", Environment.NewLine, InformacionDelRegistro(oRegistroEN), ex.Message);
                //oTransaccionesAD.Agregar(oRegistroEN.oLoginEN.idUsuario, oRegistroEN.oLoginEN.NumeroIP, oRegistroEN.idCategoriaDeCuenta, "VALIDAR", "ERROR AL VALIDAR LA INFORMACIÓN DE LA CATEGORIA DE LA CUENTA", "ERROR", DescripcionDeOperacion, oRegistroEN.oLoginEN.idUsuario, oDatos);

                return false;
            }
            finally
            {

                if (Cnn != null)
                {

                    if (Cnn.State == ConnectionState.Open)
                    {

                        Cnn.Close();

                    }

                }

                Cnn = null;
                Comando = null;
                Adaptador = null;
                oTransaccionesAD = null;

            }

        }

        public bool ValidarSiElRegistroEstaVinculado(CategoriaDeCuentaEN oRegistroEN, DatosDeConexionEN oDatos, string TipoDeOperacion)
        {

            try
            {

                Cnn = new MySqlConnection(TraerCadenaDeConexion(oDatos));
                Cnn.Open();

                Comando = new MySqlCommand();
                Comando.Connection = Cnn;
                Comando.CommandType = CommandType.StoredProcedure;
                Comando.CommandText = "ValidarSiElRegistroEstaVinculado";

                Comando.Parameters.Add(new MySqlParameter("@CampoABuscar_", MySqlDbType.VarChar, 200)).Value = "idCategoriaDeCuenta";
                Comando.Parameters.Add(new MySqlParameter("@ValorCampoABuscar", MySqlDbType.Int32)).Value = oRegistroEN.idCategoriaDeCuenta;
                Comando.Parameters.Add(new MySqlParameter("@ExcluirTabla_", MySqlDbType.VarChar, 200)).Value = string.Empty;

                Adaptador = new MySqlDataAdapter();
                DT = new DataTable();

                Adaptador.SelectCommand = Comando;
                Adaptador.Fill(DT);

                if (DT.Rows[0].ItemArray[0].ToString().ToUpper() == "NINGUNA".ToUpper())
                {
                    return false;
                }
                else
                {

                    this.Error = String.Format("La Operación: '{1}', {0} no se puede completar por que el registro: {0} '{2}', {0} se encuentra asociado con: {0} {3}", Environment.NewLine, TipoDeOperacion, InformacionDelRegistro(oRegistroEN), oTransaccionesAD.ConvertirValorDeLaCadena(DT.Rows[0].ItemArray[0].ToString()));
                    DescripcionDeOperacion = this.Error;
                    //oTransaccionesAD.Agregar(oRegistroEN.oLoginEN.idUsuario, oRegistroEN.oLoginEN.NumeroIP, oRegistroEN.idGrupoDeCuentas, "VALIDAR", "VALIDAR ASOCIACION DE REGISTRO", "ERROR", DescripcionDeOperacion, oRegistroEN.oLoginEN.idUsuario, oDatos);

                    return true;
                }

            }
            catch (Exception ex)
            {
                this.Error = ex.Message;

                DescripcionDeOperacion = string.Format("Se produjo el seguiente error: '{2}' al validar el registro. {0} {1} ", Environment.NewLine, InformacionDelRegistro(oRegistroEN), ex.Message);
                //oTransaccionesAD.Agregar(oRegistroEN.oLoginEN.idUsuario, oRegistroEN.oLoginEN.NumeroIP, oRegistroEN.idGrupoDeCuentas, "VALIDAR", "ERROR AL VALIDAR LA INFORMACIÓN DE LA GRUPO DE CUENTAS", "ERROR", DescripcionDeOperacion, oRegistroEN.oLoginEN.idUsuario, oDatos);

                return false;
            }
            finally
            {

                if (Cnn != null)
                {

                    if (Cnn.State == ConnectionState.Open)
                    {

                        Cnn.Close();

                    }

                }

                Cnn = null;
                Comando = null;
                Adaptador = null;
                oTransaccionesAD = null;

            }

        }

        #endregion

        #region "Funciones que retornan información"

        private string InformacionDelRegistro(CategoriaDeCuentaEN oRegistroEN) {

            string Cadena = @"idCategoriaDeCuenta: {0}, DescCategoriaDeCuenta: {1}, IdUsuarioDeCreacion: {2}, FechaDeCreacion: {3}, IdUsuarioDeModificacion: {4}, FechaDeModificacion: {5}, idGrupoDeCuentas: {6}";
            Cadena = string.Format(Cadena, oRegistroEN.idCategoriaDeCuenta, oRegistroEN.DescCategoriaDeCuenta, oRegistroEN.IdUsuarioDeCreacion, oRegistroEN.FechaDeCreacion, oRegistroEN.IdUsuarioDeModificacion, oRegistroEN.FechaDeModificacion, oRegistroEN.oGrupoDeCuentasEN.idGrupoDeCuentas);
            Cadena = Cadena.Replace(",", Environment.NewLine);
            return Cadena;            

        }

        private string TraerCadenaDeConexion(DatosDeConexionEN oDatos) {
            string cadena = string.Format("Data Source='{0}';Initial Catalog='{1}';Persist Security Info=True;User ID='{2}';Password='{3}'", oDatos.Servidor, oDatos.BaseDeDatos, oDatos.Usuario, oDatos.Contraseña);
            return cadena;
        }

        public DataTable TraerDatos() {
            return DT;
        }

        #endregion


    }
}
