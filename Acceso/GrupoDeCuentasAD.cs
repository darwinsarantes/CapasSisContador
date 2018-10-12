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
    public class GrupoDeCuentasAD
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

        public bool Agregar(GrupoDeCuentasEN oRegistroEN, DatosDeConexionEN oDatos) {

            oTransaccionesAD = new TransaccionesAD();

            try
            {

                Cnn = new MySqlConnection(TraerCadenaDeConexion(oDatos));
                Cnn.Open();

                Comando = new MySqlCommand();
                Comando.Connection = Cnn;
                Comando.CommandType = CommandType.Text;

                Consultas = @"
                                
                insert into grupodecuentas 
                (idGrupoDeCuentas, DescGrupoDeCuentas, Debitos, Creditos, IdUsuarioDeCreacion, FechaDeCreacion, IdUsuarioDeModificacion, FechaDeModificacion)
                values
                (IdentificadorGrupoDeCuentas(), @DescGrupoDeCuentas, @Debitos, @Creditos, @IdUsuarioDeCreacion, current_timestamp(), @IdUsuarioDeModificacion, current_timestamp());

                Select max(idGrupoDeCuentas) as 'ID' from grupodecuentas;";

                Comando.CommandText = Consultas;                               

                Comando.Parameters.Add(new MySqlParameter("@DescGrupoDeCuentas", MySqlDbType.VarChar, oRegistroEN.DescGrupoDeCuentas.Trim().Length)).Value = oRegistroEN.DescGrupoDeCuentas.Trim();
                Comando.Parameters.Add(new MySqlParameter("@Debitos", MySqlDbType.VarChar, oRegistroEN.Debitos.Trim().Length)).Value = oRegistroEN.Debitos.Trim();
                Comando.Parameters.Add(new MySqlParameter("@Creditos", MySqlDbType.VarChar, oRegistroEN.Creditos.Trim().Length)).Value = oRegistroEN.Creditos.Trim();
                Comando.Parameters.Add(new MySqlParameter("@IdUsuarioDeCreacion", MySqlDbType.Int32)).Value = oRegistroEN.oLoginEN.idUsuario;
                Comando.Parameters.Add(new MySqlParameter("@IdUsuarioDeModificacion", MySqlDbType.Int32)).Value = oRegistroEN.oLoginEN.idUsuario;

                Adaptador = new MySqlDataAdapter();
                DT = new DataTable();

                Adaptador.SelectCommand = Comando;
                Adaptador.Fill(DT);

                oRegistroEN.idGrupoDeCuentas = Convert.ToInt32(DT.Rows[0].ItemArray[0].ToString());
                
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
                TransaccionesEN oTran = InformacionDelaTransaccion(oRegistroEN, "Agregar", "Agregar Nuevo Registro", "ERROR");
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

        public bool Actualizar(GrupoDeCuentasEN oRegistroEN, DatosDeConexionEN oDatos)
        {
            oTransaccionesAD = new TransaccionesAD();

            try
            {

                Cnn = new MySqlConnection(TraerCadenaDeConexion(oDatos));
                Cnn.Open();

                Comando = new MySqlCommand();
                Comando.Connection = Cnn;
                Comando.CommandType = CommandType.Text;

                Consultas = @"update grupodecuentas set
	                DescGrupoDeCuentas = @DescGrupoDeCuentas, Debitos = @Debitos, Creditos = @Creditos, IdUsuarioDeModificacion = @IdUsuarioDeModificacion, FechaDeModificacion = current_timestamp()
                where idGrupoDeCuentas = @idGrupoDeCuentas;";

                Comando.CommandText = Consultas;

                Comando.Parameters.Add(new MySqlParameter("@idGrupoDeCuentas", MySqlDbType.Int32)).Value = oRegistroEN.idGrupoDeCuentas;
                Comando.Parameters.Add(new MySqlParameter("@DescGrupoDeCuentas", MySqlDbType.VarChar, oRegistroEN.DescGrupoDeCuentas.Trim().Length)).Value = oRegistroEN.DescGrupoDeCuentas.Trim();
                Comando.Parameters.Add(new MySqlParameter("@Debitos", MySqlDbType.VarChar, oRegistroEN.Debitos.Trim().Length)).Value = oRegistroEN.Debitos.Trim();
                Comando.Parameters.Add(new MySqlParameter("@Creditos", MySqlDbType.VarChar, oRegistroEN.Creditos.Trim().Length)).Value = oRegistroEN.Creditos.Trim();
                Comando.Parameters.Add(new MySqlParameter("@IdUsuarioDeCreacion", MySqlDbType.Int32)).Value = oRegistroEN.oLoginEN.idUsuario;
                Comando.Parameters.Add(new MySqlParameter("@IdUsuarioDeModificacion", MySqlDbType.Int32)).Value = oRegistroEN.oLoginEN.idUsuario;

                Comando.ExecuteNonQuery();
                
                DescripcionDeOperacion = string.Format("El registro fue Actualizado Correctamente. {0} {1}", Environment.NewLine, InformacionDelRegistro(oRegistroEN));

                //Agregamos la Transacción....
                TransaccionesEN oTran = InformacionDelaTransaccion(oRegistroEN, "ACTUALIZAR", "Actualizar Registro", "CORRECTO");
                oTransaccionesAD.Agregar(oTran, oDatos);

                return true;

            }
            catch (Exception ex)
            {
                this.Error = ex.Message;

                DescripcionDeOperacion = string.Format("Se produjo el seguiente error: '{2}' al actualizar el registro. {0} {1} ", Environment.NewLine, InformacionDelRegistro(oRegistroEN), ex.Message);
                //Agregamos la Transacción....
                TransaccionesEN oTran = InformacionDelaTransaccion(oRegistroEN, "ACTUALIZAR", "Actualizar Registro", "ERROR");
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

        public bool Eliminar(GrupoDeCuentasEN oRegistroEN, DatosDeConexionEN oDatos)
        {
            oTransaccionesAD = new TransaccionesAD();

            try
            {

                Cnn = new MySqlConnection(TraerCadenaDeConexion(oDatos));
                Cnn.Open();

                Comando = new MySqlCommand();
                Comando.Connection = Cnn;
                Comando.CommandType = CommandType.Text;

                Consultas = @"Delete from GrupoDeCuentas where idGrupoDeCuentas = @idGrupoDeCuentas;";
                Comando.CommandText = Consultas;

                Comando.Parameters.Add(new MySqlParameter("@idGrupoDeCuentas", MySqlDbType.Int32)).Value = oRegistroEN.idGrupoDeCuentas;
                
                Comando.ExecuteNonQuery();

                DescripcionDeOperacion = string.Format("El registro fue Eliminado Correctamente. {0} {1}", Environment.NewLine, InformacionDelRegistro(oRegistroEN));

                //Agregamos la Transacción....
                TransaccionesEN oTran = InformacionDelaTransaccion(oRegistroEN, "ELIMINAR", "Eliminar Registro", "CORRECTO");
                oTransaccionesAD.Agregar(oTran, oDatos);

                return true;

            }
            catch (Exception ex)
            {
                this.Error = ex.Message;

                DescripcionDeOperacion = string.Format("Se produjo el seguiente error: '{2}' al eliminar el registro. {0} {1} ", Environment.NewLine, InformacionDelRegistro(oRegistroEN), ex.Message);
                //Agregamos la Transacción....
                TransaccionesEN oTran = InformacionDelaTransaccion(oRegistroEN, "ELIMINAR", "Eliminar Registro", "ERROR");
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

        public bool Listado(GrupoDeCuentasEN oRegistroEN, DatosDeConexionEN oDatos)
        {

            try
            {

                Cnn = new MySqlConnection(TraerCadenaDeConexion(oDatos));
                Cnn.Open();

                Comando = new MySqlCommand();
                Comando.Connection = Cnn;
                Comando.CommandType = CommandType.Text;

                Consultas = string.Format(@"SELECT idGrupoDeCuentas, DescGrupoDeCuentas, Debitos, Creditos, 
                IdUsuarioDeCreacion, FechaDeCreacion, IdUsuarioDeModificacion, FechaDeModificacion 
                FROM grupodecuentas
                where idGrupoDeCuentas > 0 {0} {1} ", oRegistroEN.Where, oRegistroEN.OrderBy);
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

        public bool ListadoPorIdentificador(GrupoDeCuentasEN oRegistroEN, DatosDeConexionEN oDatos)
        {

            try
            {

                Cnn = new MySqlConnection(TraerCadenaDeConexion(oDatos));
                Cnn.Open();

                Comando = new MySqlCommand();
                Comando.Connection = Cnn;
                Comando.CommandType = CommandType.Text;

                Consultas = string.Format(@"SELECT idGrupoDeCuentas, DescGrupoDeCuentas, Debitos, Creditos, 
                IdUsuarioDeCreacion, FechaDeCreacion, IdUsuarioDeModificacion, FechaDeModificacion 
                FROM grupodecuentas
                where idGrupoDeCuentas = {0} ", oRegistroEN.idGrupoDeCuentas);
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

        public bool ListadoParaCombos(GrupoDeCuentasEN oRegistroEN, DatosDeConexionEN oDatos)
        {

            try
            {

                Cnn = new MySqlConnection(TraerCadenaDeConexion(oDatos));
                Cnn.Open();

                Comando = new MySqlCommand();
                Comando.Connection = Cnn;
                Comando.CommandType = CommandType.Text;

                Consultas = string.Format(@"SELECT idGrupoDeCuentas, DescGrupoDeCuentas 
                FROM grupodecuentas
                where idGrupoDeCuentas > 0 {0} {1} ; ", oRegistroEN.Where, oRegistroEN.OrderBy);
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

        public bool ListadoParaReportes(GrupoDeCuentasEN oRegistroEN, DatosDeConexionEN oDatos)
        {

            try
            {

                Cnn = new MySqlConnection(TraerCadenaDeConexion(oDatos));
                Cnn.Open();

                Comando = new MySqlCommand();
                Comando.Connection = Cnn;
                Comando.CommandType = CommandType.Text;

                Consultas = string.Format(@"SELECT idGrupoDeCuentas, DescGrupoDeCuentas, Debitos, Creditos, 
                IdUsuarioDeCreacion, FechaDeCreacion, IdUsuarioDeModificacion, FechaDeModificacion 
                FROM grupodecuentas
                where idGrupoDeCuentas > 0 {0} {1} ", oRegistroEN.Where, oRegistroEN.OrderBy);
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

        public bool ValidarSiElRegistroEstaVinculado(GrupoDeCuentasEN oRegistroEN, DatosDeConexionEN oDatos, string TipoDeOperacion)
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

                Comando.Parameters.Add(new MySqlParameter("@CampoABuscar_", MySqlDbType.VarChar, 200)).Value = "idGrupoDeCuentas";
                Comando.Parameters.Add(new MySqlParameter("@ValorCampoABuscar", MySqlDbType.Int32)).Value = oRegistroEN.idGrupoDeCuentas;
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

                    this.Error = String.Format("La Operación: '{1}', {0} no se puede completar por que el registro: {0} '{2}', {0} se encuentra asociado con: {0} {3}",Environment.NewLine, TipoDeOperacion, InformacionDelRegistro(oRegistroEN), oTransaccionesAD.ConvertirValorDeLaCadena(DT.Rows[0].ItemArray[0].ToString()));
                    DescripcionDeOperacion = this.Error;                    

                    return true;
                }

            }
            catch (Exception ex)
            {
                this.Error = ex.Message;

                DescripcionDeOperacion = string.Format("Se produjo el seguiente error: '{2}' al validar el registro. {0} {1} ", Environment.NewLine, InformacionDelRegistro(oRegistroEN), ex.Message);
                //Agregamos la Transacción....
                TransaccionesEN oTran = InformacionDelaTransaccion(oRegistroEN, "VALIDAR", "REGISTRO ASOCIADO", "ERROR");
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

        public bool ValidarRegistroDuplicado(GrupoDeCuentasEN oRegistroEN, DatosDeConexionEN oDatos, string TipoDeOperacion)
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

                        Consultas = @"SELECT CASE WHEN EXISTS(SELECT idGrupoDeCuentas FROM GrupoDeCuentas WHERE upper(trim(DescGrupoDeCuentas)) = upper(@DescGrupoDeCuentas)) THEN 1 ELSE 0 END AS 'RES'";
                        Comando.Parameters.Add(new MySqlParameter("@DescGrupoDeCuentas", MySqlDbType.VarChar, oRegistroEN.DescGrupoDeCuentas.Trim().Length)).Value = oRegistroEN.DescGrupoDeCuentas.Trim();

                        break;

                    case "ACTUALIZAR":

                        Consultas = @"SELECT CASE WHEN EXISTS(SELECT idGrupoDeCuentas FROM GrupoDeCuentas WHERE upper(trim(DescGrupoDeCuentas)) = upper(@DescGrupoDeCuentas) and idGrupoDeCuentas <> @idGrupoDeCuentas) THEN 1 ELSE 0 END AS 'RES'";
                        Comando.Parameters.Add(new MySqlParameter("@DescGrupoDeCuentas", MySqlDbType.VarChar, oRegistroEN.DescGrupoDeCuentas.Trim().Length)).Value = oRegistroEN.DescGrupoDeCuentas.Trim();
                        Comando.Parameters.Add(new MySqlParameter("@idGrupoDeCuentas", MySqlDbType.Int32)).Value = oRegistroEN.idGrupoDeCuentas;

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
                TransaccionesEN oTran = InformacionDelaTransaccion(oRegistroEN, "VALIDAR", "REGISTRO DUPLICADOR", "ERROR");
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

        private TransaccionesEN InformacionDelaTransaccion(GrupoDeCuentasEN oGrupoDeCuentas, String TipoDeOperacion, String Descripcion, String Estado)
        {
            TransaccionesEN oRegistroEN = new TransaccionesEN();

            oRegistroEN.idregistro = oGrupoDeCuentas.idGrupoDeCuentas;
            oRegistroEN.Modelo = "GrupoDeCuentasAD";
            oRegistroEN.Modulo = "Cuentas";
            oRegistroEN.Tabla = "GrupoDeCuentas";
            oRegistroEN.tipodeoperacion = TipoDeOperacion;
            oRegistroEN.Estado = Estado;
            oRegistroEN.ip = oGrupoDeCuentas.oLoginEN.NumeroIP;
            oRegistroEN.idusuario = oGrupoDeCuentas.oLoginEN.idUsuario;
            oRegistroEN.idusuarioaprueba = oGrupoDeCuentas.oLoginEN.idUsuario;
            oRegistroEN.descripciondelusuario = DescripcionDeOperacion;
            oRegistroEN.descripcioninterna = Descripcion;
            oRegistroEN.NombreDelEquipo = oGrupoDeCuentas.oLoginEN.NombreDelComputador;

            return oRegistroEN;
        }

        private string InformacionDelRegistro(GrupoDeCuentasEN oRegistroEN) {
            string Cadena = @"idGrupoDeCuentas: {0}, DescGrupoDeCuentas: {1}, Debitos:{2}, Creditos: {3}, IdUsuarioDeCreacion: {4}, FechaDeCreacion: {5}, IdUsuarioDeModificacion: {6}, FechaDeModificacion: {7}";
            Cadena = string.Format(Cadena, oRegistroEN.idGrupoDeCuentas, oRegistroEN.DescGrupoDeCuentas, oRegistroEN.Debitos, oRegistroEN.Creditos, oRegistroEN.IdUsuarioDeCreacion, oRegistroEN.FechaDeCreacion, oRegistroEN.IdUsuarioDeModificacion, oRegistroEN.FechaDeModificacion);
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
