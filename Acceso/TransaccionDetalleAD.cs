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
    public class TransaccionDetalleAD
    {

        public string Error { set; get; }
        private MySqlConnection Cnn = null;
        private MySqlCommand Comando = null;
        private MySqlDataAdapter Adaptador = null;
        private TransaccionesAD oTransaccionesAD = null;
        string Consultas;
        string DescripcionDeOperacion;
        private DataTable DT { set; get; }
        
        #region "Funciones para datos dll"

        public bool Agregar(TransaccionDetalleEN oRegistroEN, DatosDeConexionEN oDatos) {

            oTransaccionesAD = new TransaccionesAD();

            try
            {

                Cnn = new MySqlConnection(TraerCadenaDeConexion(oDatos));
                Cnn.Open();

                Comando = new MySqlCommand();
                Comando.Connection = Cnn;
                Comando.CommandType = CommandType.Text;

                Consultas = @"
                                
                insert into transacciondetalle
                (Debe, Haber, IdUsuarioDeCreacion, FechaDeCreacion, IdUsuarioDeModificacion, FechaDeModificacion, NoCuenta, idTransacciones)
                values
                (@Debe, @Haber, @IdUsuarioDeCreacion, current_timestamp(), @IdUsuarioDeModificacion, current_timestamp(), @NoCuenta, @idTransacciones);

                select last_insert_id() as 'ID';";

                Comando.CommandText = Consultas;

                Comando.Parameters.Add(new MySqlParameter("@Debe", MySqlDbType.Decimal)).Value = oRegistroEN.Debe;
                Comando.Parameters.Add(new MySqlParameter("@Haber", MySqlDbType.Decimal)).Value = oRegistroEN.Haber;
                Comando.Parameters.Add(new MySqlParameter("@NoCuenta", MySqlDbType.Int32)).Value = oRegistroEN.oCuentaEN.NoCuenta;
                Comando.Parameters.Add(new MySqlParameter("@idTransacciones", MySqlDbType.Int32)).Value = oRegistroEN.oTransaccionesEN.idTransacciones;
                Comando.Parameters.Add(new MySqlParameter("@IdUsuarioDeCreacion", MySqlDbType.Int32)).Value = oRegistroEN.oLoginEN.idUsuario;
                Comando.Parameters.Add(new MySqlParameter("@IdUsuarioDeModificacion", MySqlDbType.Int32)).Value = oRegistroEN.oLoginEN.idUsuario;

                Adaptador = new MySqlDataAdapter();
                DT = new DataTable();

                Adaptador.SelectCommand = Comando;
                Adaptador.Fill(DT);

                oRegistroEN.idTransaccionDetalle = Convert.ToInt32(DT.Rows[0].ItemArray[0].ToString());
                
                DescripcionDeOperacion = string.Format("El registro fue Insertado Correctamente. {0} {1}", Environment.NewLine, InformacionDelRegistro(oRegistroEN));

                //Agregamos la Transacción....
                TransaccionesEN oTran = InformacionDelaTransaccion(oRegistroEN, "Agregar", "Agregar Nuevo Registro", "CORRECTO");
                oTransaccionesAD.Agregar(oTran, oDatos);

            return true;


            }
            catch (Exception ex)
            {
                this.Error = ex.Message;

                DescripcionDeOperacion = string.Format("Se produjo el seguiente error: '{2}' al insertar el registro. {0} {1} ", Environment.NewLine, InformacionDelRegistro(oRegistroEN), ex.Message);

                //Agregamos la Transacción....
                TransaccionesEN oTran = InformacionDelaTransaccion(oRegistroEN, "Agregar", "Error al agregar el registro", "ERROR");
                oTransaccionesAD.Agregar(oTran, oDatos);

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

        public bool Actualizar(TransaccionDetalleEN oRegistroEN, DatosDeConexionEN oDatos)
        {
            oTransaccionesAD = new TransaccionesAD();

            try
            {

                Cnn = new MySqlConnection(TraerCadenaDeConexion(oDatos));
                Cnn.Open();

                Comando = new MySqlCommand();
                Comando.Connection = Cnn;
                Comando.CommandType = CommandType.Text;

                Consultas = @"update transacciondetalle set

	                Debe = @Debe, Haber = @Haber, 
                    IdUsuarioDeModificacion = @IdUsuarioDeModificacion, 
                    FechaDeModificacion = current_timestamp(), NoCuenta = @NoCuenta, idTransacciones = @idTransacciones

                where idTransaccionDetalle = @idTransaccionDetalle;";

                Comando.CommandText = Consultas;

                Comando.Parameters.Add(new MySqlParameter("@idTransaccionDetalle", MySqlDbType.Int32)).Value = oRegistroEN.idTransaccionDetalle;
                Comando.Parameters.Add(new MySqlParameter("@Debe", MySqlDbType.Decimal)).Value = oRegistroEN.Debe;
                Comando.Parameters.Add(new MySqlParameter("@Haber", MySqlDbType.Decimal)).Value = oRegistroEN.Haber;
                Comando.Parameters.Add(new MySqlParameter("@NoCuenta", MySqlDbType.Int32)).Value = oRegistroEN.oCuentaEN.NoCuenta;
                Comando.Parameters.Add(new MySqlParameter("@idTransacciones", MySqlDbType.Int32)).Value = oRegistroEN.oTransaccionesEN.idTransacciones;                
                Comando.Parameters.Add(new MySqlParameter("@IdUsuarioDeModificacion", MySqlDbType.Int32)).Value = oRegistroEN.oLoginEN.idUsuario;

                Comando.ExecuteNonQuery();
                
                DescripcionDeOperacion = string.Format("El registro fue Actualizado Correctamente. {0} {1}", Environment.NewLine, InformacionDelRegistro(oRegistroEN));

                //Agregamos la Transacción....
                TransaccionesEN oTran = InformacionDelaTransaccion(oRegistroEN, "Actualizar", "Registro Actualizado", "CORRECTO");
                oTransaccionesAD.Agregar(oTran, oDatos);

                return true;

            }
            catch (Exception ex)
            {
                this.Error = ex.Message;

                DescripcionDeOperacion = string.Format("Se produjo el seguiente error: '{2}' al actualizar el registro. {0} {1} ", Environment.NewLine, InformacionDelRegistro(oRegistroEN), ex.Message);
                
                //Agregamos la transacción
                TransaccionesEN oTran = InformacionDelaTransaccion(oRegistroEN, "Actualizar", "Error al Actualizar", "CORRECTO");
                oTransaccionesAD.Agregar(oTran, oDatos);

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

        public bool Eliminar(TransaccionDetalleEN oRegistroEN, DatosDeConexionEN oDatos)
        {
            oTransaccionesAD = new TransaccionesAD();

            try
            {

                Cnn = new MySqlConnection(TraerCadenaDeConexion(oDatos));
                Cnn.Open();

                Comando = new MySqlCommand();
                Comando.Connection = Cnn;
                Comando.CommandType = CommandType.Text;

                Consultas = @"Delete from transacciondetalle where idTransaccionDetalle = @idTransaccionDetalle;";
                Comando.CommandText = Consultas;

                Comando.Parameters.Add(new MySqlParameter("@idTransaccionDetalle", MySqlDbType.Int32)).Value = oRegistroEN.idTransaccionDetalle;
                
                Comando.ExecuteNonQuery();

                DescripcionDeOperacion = string.Format("El registro fue Eliminado Correctamente. {0} {1}", Environment.NewLine, InformacionDelRegistro(oRegistroEN));

                //Agregamos la Transacción....
                TransaccionesEN oTran = InformacionDelaTransaccion(oRegistroEN, "Elminar", "Eliminar Registro", "CORRECTO");
                oTransaccionesAD.Agregar(oTran, oDatos);

                return true;

            }
            catch (Exception ex)
            {
                this.Error = ex.Message;

                DescripcionDeOperacion = string.Format("Se produjo el seguiente error: '{2}' al eliminar el registro. {0} {1} ", Environment.NewLine, InformacionDelRegistro(oRegistroEN), ex.Message);
                
                //Agregamos la Transacción....
                TransaccionesEN oTran = InformacionDelaTransaccion(oRegistroEN, "Eliminar", "Error al eliminar el registro", "CORRECTO");
                oTransaccionesAD.Agregar(oTran, oDatos);

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

        public bool Listado(TransaccionDetalleEN oRegistroEN, DatosDeConexionEN oDatos)
        {

            try
            {

                Cnn = new MySqlConnection(TraerCadenaDeConexion(oDatos));
                Cnn.Open();

                Comando = new MySqlCommand();
                Comando.Connection = Cnn;
                Comando.CommandType = CommandType.Text;

                Consultas = string.Format(@"Select 
                idTransaccionDetalle, NoCuenta, idTransacciones, c.idCuenta,c.DescCuenta, Debe, Haber, 
                IdUsuarioDeCreacion, FechaDeCreacion, IdUsuarioDeModificacion, FechaDeModificacion
                from transacciondetalle as tt
                inner join transacciones as t on t.idTransacciones = tt.idTransacciones
                inner join cuenta as c on c.NoCuenta = tt.NoCuenta
                where idTransaccionDetalle > 0  {0} {1} ", oRegistroEN.Where, oRegistroEN.OrderBy);
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

        public bool ListadoPorIdentificadorDeTransaccion(TransaccionDetalleEN oRegistroEN, DatosDeConexionEN oDatos)
        {

            try
            {

                Cnn = new MySqlConnection(TraerCadenaDeConexion(oDatos));
                Cnn.Open();

                Comando = new MySqlCommand();
                Comando.Connection = Cnn;
                Comando.CommandType = CommandType.Text;

                Consultas = string.Format(@"Select 
                idTransaccionDetalle, NoCuenta, idTransacciones, c.idCuenta,c.DescCuenta, Debe, Haber, 
                IdUsuarioDeCreacion, FechaDeCreacion, IdUsuarioDeModificacion, FechaDeModificacion
                from transacciondetalle as tt
                inner join transacciones as t on t.idTransacciones = tt.idTransacciones
                inner join cuenta as c on c.NoCuenta = tt.NoCuenta
                where tt.idTransacciones = {0} ", oRegistroEN.oTransaccionesEN.idTransacciones);
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

        public bool ListadoPorIdentificador(TransaccionDetalleEN oRegistroEN, DatosDeConexionEN oDatos)
        {

            try
            {

                Cnn = new MySqlConnection(TraerCadenaDeConexion(oDatos));
                Cnn.Open();

                Comando = new MySqlCommand();
                Comando.Connection = Cnn;
                Comando.CommandType = CommandType.Text;

                Consultas = string.Format(@"Select 
                idTransaccionDetalle, NoCuenta, idTransacciones, c.idCuenta,c.DescCuenta, Debe, Haber, 
                IdUsuarioDeCreacion, FechaDeCreacion, IdUsuarioDeModificacion, FechaDeModificacion
                from transacciondetalle as tt
                inner join transacciones as t on t.idTransacciones = tt.idTransacciones
                inner join cuenta as c on c.NoCuenta = tt.NoCuenta
                where idTransaccionDetalle = {0} ", oRegistroEN.idTransaccionDetalle);
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

        public bool ListadoParaCombos(TransaccionDetalleEN oRegistroEN, DatosDeConexionEN oDatos)
        {

            try
            {

                Cnn = new MySqlConnection(TraerCadenaDeConexion(oDatos));
                Cnn.Open();

                Comando = new MySqlCommand();
                Comando.Connection = Cnn;
                Comando.CommandType = CommandType.Text;

                Consultas = string.Format(@"Select 
                idTransaccionDetalle, NoCuenta, idTransacciones, c.idCuenta,c.DescCuenta, Debe, Haber, 
                IdUsuarioDeCreacion, FechaDeCreacion, IdUsuarioDeModificacion, FechaDeModificacion
                from transacciondetalle as tt
                inner join transacciones as t on t.idTransacciones = tt.idTransacciones
                inner join cuenta as c on c.NoCuenta = tt.NoCuenta
                where idTransaccionDetalle > 0  {0} {1} ; ", oRegistroEN.Where, oRegistroEN.OrderBy);
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

        public bool ListadoParaReportes(TransaccionDetalleEN oRegistroEN, DatosDeConexionEN oDatos)
        {

            try
            {

                Cnn = new MySqlConnection(TraerCadenaDeConexion(oDatos));
                Cnn.Open();

                Comando = new MySqlCommand();
                Comando.Connection = Cnn;
                Comando.CommandType = CommandType.Text;

                Consultas = string.Format(@"Select 
                idTransaccionDetalle, NoCuenta, idTransacciones, c.idCuenta,c.DescCuenta, Debe, Haber, 
                IdUsuarioDeCreacion, FechaDeCreacion, IdUsuarioDeModificacion, FechaDeModificacion
                from transacciondetalle as tt
                inner join transacciones as t on t.idTransacciones = tt.idTransacciones
                inner join cuenta as c on c.NoCuenta = tt.NoCuenta
                where idTransaccionDetalle > 0  {0} {1} ", oRegistroEN.Where, oRegistroEN.OrderBy);
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

        public bool ValidarSiElRegistroEstaVinculado(TransaccionDetalleEN oRegistroEN, DatosDeConexionEN oDatos, string TipoDeOperacion)
        {
            oTransaccionesAD = new TransaccionesAD();

            try
            {

                Cnn = new MySqlConnection(TraerCadenaDeConexion(oDatos));
                Cnn.Open();

                Comando = new MySqlCommand();
                Comando.Connection = Cnn;
                Comando.CommandType = CommandType.StoredProcedure;
                Comando.CommandText = "ValidarSiElRegistroEstaVinculado";

                Comando.Parameters.Add(new MySqlParameter("@CampoABuscar_", MySqlDbType.VarChar, 200)).Value = "idTransaccionDetalle";
                Comando.Parameters.Add(new MySqlParameter("@ValorCampoABuscar", MySqlDbType.Int32)).Value = oRegistroEN.idTransaccionDetalle;
                Comando.Parameters.Add(new MySqlParameter("@ExcluirTabla_", MySqlDbType.VarChar, 200)).Value = "";

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

                    this.Error = String.Format("La Operación: '{1}', {0} no se puede completar por que el registro: {0} '{2}', {0} se encuentra asociado con: {0} {3}",Environment.NewLine, TipoDeOperacion, InformacionDelRegistro(oRegistroEN), oTransaccionesAD.ConvertirValorDeLaCadena(DT.Rows[0].ItemArray[0].ToString()));
                    DescripcionDeOperacion = this.Error;

                    //Agregamos la Transacción....
                    TransaccionesEN oTran = InformacionDelaTransaccion(oRegistroEN, "Validar", "Validar Si el registro se encunentra vinculado", "CORRECTO");
                    oTransaccionesAD.Agregar(oTran, oDatos);

                    return true;
                }

            }
            catch (Exception ex)
            {
                this.Error = ex.Message;

                DescripcionDeOperacion = string.Format("Se produjo el seguiente error: '{2}' al validar el registro. {0} {1} ", Environment.NewLine, InformacionDelRegistro(oRegistroEN), ex.Message);

                //Agregamos la Transacción....
                TransaccionesEN oTran = InformacionDelaTransaccion(oRegistroEN, "Validar", "Validar Si el registro se encunentra vinculado", "ERROR");
                oTransaccionesAD.Agregar(oTran, oDatos);

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

        public bool ValidarRegistroDuplicado(TransaccionDetalleEN oRegistroEN, DatosDeConexionEN oDatos, string TipoDeOperacion)
        {
            oTransaccionesAD = new TransaccionesAD();

            try
            {

                Cnn = new MySqlConnection(TraerCadenaDeConexion(oDatos));
                Cnn.Open();

                Comando = new MySqlCommand();
                Comando.Connection = Cnn;
                Comando.CommandType = CommandType.Text;

                switch (TipoDeOperacion.Trim().ToUpper()){

                    case "AGREGAR":

                        Consultas = @"SELECT CASE WHEN EXISTS(Select idTransaccionDetalle from transacciondetalle where NoCuenta = @NoCuenta and idTransacciones = @idTransacciones) THEN 1 ELSE 0 END AS 'RES'";
                        Comando.Parameters.Add(new MySqlParameter("@idTransacciones", MySqlDbType.Int32)).Value = oRegistroEN.oTransaccionesEN.idTransacciones;
                        Comando.Parameters.Add(new MySqlParameter("@NoCuenta", MySqlDbType.Int32)).Value = oRegistroEN.oCuentaEN.NoCuenta;

                        break;

                    case "ACTUALIZAR":

                        Consultas = @"SELECT CASE WHEN EXISTS(Select idTransaccionDetalle from transacciondetalle where NoCuenta = @NoCuenta and idTransacciones = @idTransacciones and idTransaccionDetalle <> @idTransaccionDetalle) THEN 1 ELSE 0 END AS 'RES'";
                        Comando.Parameters.Add(new MySqlParameter("@idTransacciones", MySqlDbType.Int32)).Value = oRegistroEN.oTransaccionesEN.idTransacciones;
                        Comando.Parameters.Add(new MySqlParameter("@NoCuenta", MySqlDbType.Int32)).Value = oRegistroEN.oCuentaEN.NoCuenta;
                        Comando.Parameters.Add(new MySqlParameter("@idTransaccionDetalle", MySqlDbType.Int32)).Value = oRegistroEN.idTransaccionDetalle;

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

                //Agregamos la Transacción....
                TransaccionesEN oTran = InformacionDelaTransaccion(oRegistroEN, "VALIDAR", "DUPLICACIÓN DE REGISTRO", "ERROR");
                oTransaccionesAD.Agregar(oTran, oDatos);

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

        private TransaccionesEN InformacionDelaTransaccion(TransaccionDetalleEN oTransaccionDetalle, String TipoDeOperacion, String Descripcion, String Estado)
        {

            TransaccionesEN oRegistroEN = new TransaccionesEN();

            oRegistroEN.idregistro = oTransaccionDetalle.idTransaccionDetalle;
            oRegistroEN.Modelo = "TransaccionAD";
            oRegistroEN.Modulo = "Transacciones";
            oRegistroEN.Tabla = "TransaccionesDetalle";
            oRegistroEN.tipodeoperacion = TipoDeOperacion;
            oRegistroEN.Estado = Estado;
            oRegistroEN.ip = oTransaccionDetalle.oLoginEN.NumeroIP;
            oRegistroEN.idusuario = oTransaccionDetalle.oLoginEN.idUsuario;
            oRegistroEN.idusuarioaprueba = oTransaccionDetalle.oLoginEN.idUsuario;
            oRegistroEN.descripciondelusuario = DescripcionDeOperacion;
            oRegistroEN.descripcioninterna = Descripcion;
            oRegistroEN.NombreDelEquipo = oTransaccionDetalle.oLoginEN.NombreDelComputador;

            return oRegistroEN;

        }

        private string InformacionDelRegistro(TransaccionDetalleEN oRegistroEN) {
            string Cadena = @"idTransaccionDetalle: {0}, Debe: {1}, Haber: {2}, IdUsuarioDeCreacion: {3}, FechaDeCreacion: {4}, IdUsuarioDeModificacion: {5}, FechaDeModificacion: {6}, NoCuenta: {7}, idTransacciones: {8}";
            Cadena = string.Format(Cadena, oRegistroEN.idTransaccionDetalle, oRegistroEN.Debe, oRegistroEN.Haber, oRegistroEN.IdUsuarioDeCreacion, oRegistroEN.FechaDeCreacion, oRegistroEN.IdUsuarioDeModificacion, oRegistroEN.FechaDeModificacion, oRegistroEN.oCuentaEN.NoCuenta, oRegistroEN.oTransaccionesEN.idTransacciones);
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
