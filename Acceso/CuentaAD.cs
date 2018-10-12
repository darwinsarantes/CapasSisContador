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
    public class CuentaAD
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

        public bool Agregar(CuentaEN oRegistroEN, DatosDeConexionEN oDatos) {

            oTransaccionesAD = new TransaccionesAD();

            try
            {

                Cnn = new MySqlConnection(TraerCadenaDeConexion(oDatos));
                Cnn.Open();

                Comando = new MySqlCommand();
                Comando.Connection = Cnn;
                Comando.CommandType = CommandType.Text;

                Consultas = @"insert into cuenta
                (idCuenta, DescCuenta, SaldoCuenta, NivelCuenta, CuentaMadre, 
                IdUsuarioDeCreacion, FechaDeCreacion, IdUsuarioDeModificacion, FechaDeModificacion, 
                idCategoriaDeCuenta, idTipoDeCuenta, NoCuenta)
                values
                (@idCuenta, @DescCuenta, @SaldoCuenta, @NivelCuenta, @CuentaMadre, 
                @IdUsuarioDeCreacion, current_timestamp(), @IdUsuarioDeModificacion, current_timestamp(), 
                @idCategoriaDeCuenta, @idTipoDeCuenta, IdentificadorDeLaCuenta());

                Select max(NoCuenta) as 'ID' from cuenta;";

                Comando.CommandText = Consultas;

                Comando.Parameters.Add(new MySqlParameter("@idCuenta", MySqlDbType.VarChar, oRegistroEN.idCuenta.Trim().Length)).Value = oRegistroEN.idCuenta.Trim();
                Comando.Parameters.Add(new MySqlParameter("@DescCuenta", MySqlDbType.VarChar, oRegistroEN.DescCuenta.Trim().Length)).Value = oRegistroEN.DescCuenta.Trim();
                Comando.Parameters.Add(new MySqlParameter("@SaldoCuenta", MySqlDbType.Decimal)).Value = oRegistroEN.SaldoCuenta;
                Comando.Parameters.Add(new MySqlParameter("@NivelCuenta", MySqlDbType.Int32)).Value = oRegistroEN.NivelCuenta;
                Comando.Parameters.Add(new MySqlParameter("@CuentaMadre", MySqlDbType.VarChar, oRegistroEN.CuentaMadre.Trim().Length)).Value = oRegistroEN.CuentaMadre.Trim();
                Comando.Parameters.Add(new MySqlParameter("@IdUsuarioDeCreacion", MySqlDbType.Int32)).Value = oRegistroEN.oLoginEN.idUsuario;
                Comando.Parameters.Add(new MySqlParameter("@IdUsuarioDeModificacion", MySqlDbType.Int32)).Value = oRegistroEN.oLoginEN.idUsuario;
                Comando.Parameters.Add(new MySqlParameter("@idCategoriaDeCuenta", MySqlDbType.Int32)).Value = oRegistroEN.oCategoriaDeCuentaEN.idCategoriaDeCuenta;
                Comando.Parameters.Add(new MySqlParameter("@idTipoDeCuenta", MySqlDbType.Int32)).Value = oRegistroEN.oTipoDeCuentaEN.idTipoDeCuenta;

                Adaptador = new MySqlDataAdapter();
                DT = new DataTable();

                Adaptador.SelectCommand = Comando;
                Adaptador.Fill(DT);

                oRegistroEN.NoCuenta = Convert.ToInt32(DT.Rows[0].ItemArray[0].ToString());

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

        public bool Actualizar(CuentaEN oRegistroEN, DatosDeConexionEN oDatos)
        {
            oTransaccionesAD = new TransaccionesAD();

            try
            {

                Cnn = new MySqlConnection(TraerCadenaDeConexion(oDatos));
                Cnn.Open();

                Comando = new MySqlCommand();
                Comando.Connection = Cnn;
                Comando.CommandType = CommandType.Text;

                Consultas = @"update cuenta set
	                idCuenta = @idCuenta, DescCuenta = @DescCuenta, SaldoCuenta = @SaldoCuenta, NivelCuenta = @NivelCuenta, CuentaMadre = @CuentaMadre, 
	                IdUsuarioDeModificacion = @IdUsuarioDeModificacion, FechaDeModificacion = current_timestamp(), 
	                idCategoriaDeCuenta = @idCategoriaDeCuenta, idTipoDeCuenta = @idTipoDeCuenta
                where  NoCuenta = @NoCuenta;";

                Comando.CommandText = Consultas;

                Comando.Parameters.Add(new MySqlParameter("@NoCuenta", MySqlDbType.Int32)).Value = oRegistroEN.NoCuenta;
                Comando.Parameters.Add(new MySqlParameter("@idCuenta", MySqlDbType.VarChar, oRegistroEN.idCuenta.Trim().Length)).Value = oRegistroEN.idCuenta.Trim();
                Comando.Parameters.Add(new MySqlParameter("@DescCuenta", MySqlDbType.VarChar, oRegistroEN.DescCuenta.Trim().Length)).Value = oRegistroEN.DescCuenta.Trim();
                Comando.Parameters.Add(new MySqlParameter("@SaldoCuenta", MySqlDbType.Decimal)).Value = oRegistroEN.SaldoCuenta;
                Comando.Parameters.Add(new MySqlParameter("@NivelCuenta", MySqlDbType.Int32)).Value = oRegistroEN.NivelCuenta;
                Comando.Parameters.Add(new MySqlParameter("@CuentaMadre", MySqlDbType.VarChar, oRegistroEN.CuentaMadre.Trim().Length)).Value = oRegistroEN.CuentaMadre.Trim();                
                Comando.Parameters.Add(new MySqlParameter("@IdUsuarioDeModificacion", MySqlDbType.Int32)).Value = oRegistroEN.oLoginEN.idUsuario;
                Comando.Parameters.Add(new MySqlParameter("@idCategoriaDeCuenta", MySqlDbType.Int32)).Value = oRegistroEN.oCategoriaDeCuentaEN.idCategoriaDeCuenta;
                Comando.Parameters.Add(new MySqlParameter("@idTipoDeCuenta", MySqlDbType.Int32)).Value = oRegistroEN.oTipoDeCuentaEN.idTipoDeCuenta;

                Comando.ExecuteNonQuery();
                
                DescripcionDeOperacion = string.Format("El registro fue Actualizado Correctamente. {0} {1}", Environment.NewLine, InformacionDelRegistro(oRegistroEN));

                //Agregamos la Transacción....
                TransaccionesEN oTran = InformacionDelaTransaccion(oRegistroEN, "Actualizar", "Actualizar Registro", "CORRECTO");
                oTransaccionesAD.Agregar(oTran, oDatos);

                return true;

            }
            catch (Exception ex)
            {
                this.Error = ex.Message;

                DescripcionDeOperacion = string.Format("Se produjo el seguiente error: '{2}' al actualizar el registro. {0} {1} ", Environment.NewLine, InformacionDelRegistro(oRegistroEN), ex.Message);
                //Agregamos la Transacción....
                TransaccionesEN oTran = InformacionDelaTransaccion(oRegistroEN, "Actualizar", "Actualizar Registro", "ERROR");
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

        public bool Eliminar(CuentaEN oRegistroEN, DatosDeConexionEN oDatos)
        {
            oTransaccionesAD = new TransaccionesAD();

            try
            {

                Cnn = new MySqlConnection(TraerCadenaDeConexion(oDatos));
                Cnn.Open();

                Comando = new MySqlCommand();
                Comando.Connection = Cnn;
                Comando.CommandType = CommandType.Text;

                Consultas = @"Delete from Cuenta where NoCuenta = @NoCuenta;";
                Comando.CommandText = Consultas;

                Comando.Parameters.Add(new MySqlParameter("@NoCuenta", MySqlDbType.Int32)).Value = oRegistroEN.NoCuenta ;
                
                Comando.ExecuteNonQuery();

                DescripcionDeOperacion = string.Format("El registro fue Eliminado Correctamente. {0} {1}", Environment.NewLine, InformacionDelRegistro(oRegistroEN));

                //Agregamos la Transacción....
                TransaccionesEN oTran = InformacionDelaTransaccion(oRegistroEN, "Eliminar", "Eliminar Registro", "CORRECTO");
                oTransaccionesAD.Agregar(oTran, oDatos);

                return true;

            }
            catch (Exception ex)
            {
                this.Error = ex.Message;

                DescripcionDeOperacion = string.Format("Se produjo el seguiente error: '{2}' al eliminar el registro. {0} {1} ", Environment.NewLine, InformacionDelRegistro(oRegistroEN), ex.Message);
                //Agregamos la Transacción....
                TransaccionesEN oTran = InformacionDelaTransaccion(oRegistroEN, "Eliminar", "Eliminar Registro", "ERROR");
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

        public bool Listado(CuentaEN oRegistroEN, DatosDeConexionEN oDatos)
        {

            try
            {

                Cnn = new MySqlConnection(TraerCadenaDeConexion(oDatos));
                Cnn.Open();

                Comando = new MySqlCommand();
                Comando.Connection = Cnn;
                Comando.CommandType = CommandType.Text;

                Consultas = string.Format(@"Select 
                c.idCategoriaDeCuenta, c.idTipoDeCuenta, gc.idGrupoDeCuentas,c.NoCuenta,c.idCuenta, 
                c.DescCuenta,cc.DescCategoriaDeCuenta,gc.DescGrupoDeCuentas, c.SaldoCuenta, c.NivelCuenta, c.CuentaMadre, 
                c.IdUsuarioDeCreacion, c.FechaDeCreacion, c.IdUsuarioDeModificacion, 
                c.FechaDeModificacion, tc.DescTipoDeCuenta
                from cuenta as c
                inner join categoriadecuenta as cc on cc.idCategoriaDeCuenta = c.idCategoriaDeCuenta
                inner join tipodecuenta as tc on tc.idTipoDeCuenta = c.idTipoDeCuenta
                inner join grupodecuentas as gc on gc.idGrupoDeCuentas = cc.idGrupoDeCuentas
                where NoCuenta > 0 {0} {1} ", oRegistroEN.Where, oRegistroEN.OrderBy);

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

        public bool TraerInformacionDelAsociado(CuentaEN oRegistroEN, DatosDeConexionEN oDatos)
        {

            try
            {

                Cnn = new MySqlConnection(TraerCadenaDeConexion(oDatos));
                Cnn.Open();

                Comando = new MySqlCommand();
                Comando.Connection = Cnn;
                Comando.CommandType = CommandType.Text;

                Consultas = string.Format(@"Select 
                c.idCategoriaDeCuenta, c.idTipoDeCuenta, gc.idGrupoDeCuentas,c.NoCuenta,c.idCuenta, 
                c.DescCuenta,cc.DescCategoriaDeCuenta,gc.DescGrupoDeCuentas, c.SaldoCuenta, c.NivelCuenta, c.CuentaMadre, 
                c.IdUsuarioDeCreacion, c.FechaDeCreacion, c.IdUsuarioDeModificacion, 
                c.FechaDeModificacion, tc.DescTipoDeCuenta
                from cuenta as c
                inner join categoriadecuenta as cc on cc.idCategoriaDeCuenta = c.idCategoriaDeCuenta
                inner join tipodecuenta as tc on tc.idTipoDeCuenta = c.idTipoDeCuenta
                inner join grupodecuentas as gc on gc.idGrupoDeCuentas = cc.idGrupoDeCuentas
                where c.NoCuenta = (select c1.CuentaMadre from cuenta as c1 where  c1.NoCuenta = {0} and c1.CuentaMadre = '{1}') ", oRegistroEN.NoCuenta, oRegistroEN.CuentaMadre);

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

        public bool ListadoPorIdentificador(CuentaEN oRegistroEN, DatosDeConexionEN oDatos)
        {

            try
            {

                Cnn = new MySqlConnection(TraerCadenaDeConexion(oDatos));
                Cnn.Open();

                Comando = new MySqlCommand();
                Comando.Connection = Cnn;
                Comando.CommandType = CommandType.Text;

                Consultas = string.Format(@"Select 
                c.idCategoriaDeCuenta, c.idTipoDeCuenta, gc.idGrupoDeCuentas,c.NoCuenta,c.idCuenta, c.DescCuenta,cc.DescCategoriaDeCuenta,gc.DescGrupoDeCuentas, c.SaldoCuenta, c.NivelCuenta, c.CuentaMadre, 
                c.IdUsuarioDeCreacion, c.FechaDeCreacion, c.IdUsuarioDeModificacion, 
                c.FechaDeModificacion
                from cuenta as c
                inner join categoriadecuenta as cc on cc.idCategoriaDeCuenta = c.idCategoriaDeCuenta
                inner join tipodecuenta tc on tc.idTipoDeCuenta = c.idTipoDeCuenta
                inner join grupodecuentas as gc on gc.idGrupoDeCuentas = cc.idGrupoDeCuentas
                where NoCuenta = {0} ", oRegistroEN.NoCuenta);
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

        public bool ListadoParaCombos(CuentaEN oRegistroEN, DatosDeConexionEN oDatos)
        {

            try
            {

                Cnn = new MySqlConnection(TraerCadenaDeConexion(oDatos));
                Cnn.Open();

                Comando = new MySqlCommand();
                Comando.Connection = Cnn;
                Comando.CommandType = CommandType.Text;

                Consultas = string.Format(@"Select 
	                c.NoCuenta,c.idCuenta, c.DescCuenta, c.SaldoCuenta, c.NivelCuenta, c.CuentaMadre, 
                    c.IdUsuarioDeCreacion, c.FechaDeCreacion, c.IdUsuarioDeModificacion, 
                    c.FechaDeModificacion, c.idCategoriaDeCuenta, c.idTipoDeCuenta
                from cuenta as c
                inner join categoriadecuenta as cc on cc.idCategoriaDeCuenta = c.idCategoriaDeCuenta
                inner join tipodecuenta tc on tc.idTipoDeCuenta = c.idTipoDeCuenta
                where NoCuenta > 0 {0} {1}; ", oRegistroEN.Where, oRegistroEN.OrderBy);
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

        public bool ListadoParaReportes(CuentaEN oRegistroEN, DatosDeConexionEN oDatos)
        {

            try
            {

                Cnn = new MySqlConnection(TraerCadenaDeConexion(oDatos));
                Cnn.Open();

                Comando = new MySqlCommand();
                Comando.Connection = Cnn;
                Comando.CommandType = CommandType.Text;

                Consultas = string.Format(@"Select 
                c.idCategoriaDeCuenta, c.idTipoDeCuenta, gc.idGrupoDeCuentas,c.NoCuenta,c.idCuenta, 
                c.DescCuenta,cc.DescCategoriaDeCuenta,gc.DescGrupoDeCuentas, c.SaldoCuenta, c.NivelCuenta, c.CuentaMadre, 
                c.IdUsuarioDeCreacion, c.FechaDeCreacion, c.IdUsuarioDeModificacion, 
                c.FechaDeModificacion, tc.DescTipoDeCuenta
                from cuenta as c
                inner join categoriadecuenta as cc on cc.idCategoriaDeCuenta = c.idCategoriaDeCuenta
                inner join tipodecuenta as tc on tc.idTipoDeCuenta = c.idTipoDeCuenta
                inner join grupodecuentas as gc on gc.idGrupoDeCuentas = cc.idGrupoDeCuentas
                where NoCuenta > 0 {0} {1} ", oRegistroEN.Where, oRegistroEN.OrderBy);
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

        public bool ValidarRegistroDuplicadoPorElIdentificadorDeLaCuenta(CuentaEN oRegistroEN, DatosDeConexionEN oDatos, string TipoDeOperacion)
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

                        Consultas = @"SELECT CASE WHEN EXISTS(SELECT idCuenta FROM Cuenta WHERE upper(trim(idCuenta)) = upper(@idCuenta)) THEN 1 ELSE 0 END AS 'RES'";
                        Comando.Parameters.Add(new MySqlParameter("@idCuenta", MySqlDbType.VarChar, oRegistroEN.idCuenta.Trim().Length)).Value = oRegistroEN.idCuenta.Trim();

                        break;

                    case "ACTUALIZAR":

                        Consultas = @"SELECT CASE WHEN EXISTS(SELECT idCuenta FROM Cuenta WHERE upper(trim(idCuenta)) = upper(@idCuenta) and NoCuenta <> @NoCuenta) THEN 1 ELSE 0 END AS 'RES'";
                        Comando.Parameters.Add(new MySqlParameter("@idCuenta", MySqlDbType.VarChar, oRegistroEN.idCuenta.Trim().Length)).Value = oRegistroEN.idCuenta.Trim();
                        Comando.Parameters.Add(new MySqlParameter("@NoCuenta", MySqlDbType.Int32)).Value = oRegistroEN.NoCuenta;

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
                    
                    DescripcionDeOperacion = string.Format("Ya existe el Número de Cuenta dentro de nuestro sistema: {0} {1}", Environment.NewLine, InformacionDelRegistro(oRegistroEN));
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
                TransaccionesEN oTran = InformacionDelaTransaccion(oRegistroEN, "VALIDAR", "REGISTRO DUPLICADO POR IDENTIFICADOR DE LA CUENTA", "ERROR");
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

        public bool ValidarRegistroDuplicadoPorDescripcionDeLaCuenta(CuentaEN oRegistroEN, DatosDeConexionEN oDatos, string TipoDeOperacion)
        {
            oTransaccionesAD = new TransaccionesAD();

            try
            {

                Cnn = new MySqlConnection(TraerCadenaDeConexion(oDatos));
                Cnn.Open();

                Comando = new MySqlCommand();
                Comando.Connection = Cnn;
                Comando.CommandType = CommandType.Text;

                switch (TipoDeOperacion.Trim().ToUpper())
                {

                    case "AGREGAR":

                        Consultas = @"SELECT CASE WHEN EXISTS(SELECT idCuenta FROM Cuenta WHERE upper(idCuenta) = upper(@idCuenta)  and upper(trim(DescCuenta)) = upper(@DescCuenta)) THEN 1 ELSE 0 END AS 'RES'";
                        Comando.Parameters.Add(new MySqlParameter("@idCuenta", MySqlDbType.VarChar, oRegistroEN.idCuenta.Trim().Length)).Value = oRegistroEN.idCuenta.Trim();
                        Comando.Parameters.Add(new MySqlParameter("@DescCuenta", MySqlDbType.VarChar, oRegistroEN.DescCuenta.Trim().Length)).Value = oRegistroEN.DescCuenta.Trim();

                        break;

                    case "ACTUALIZAR":

                        Consultas = @"SELECT CASE WHEN EXISTS(SELECT idCuenta FROM Cuenta WHERE upper(idCuenta) = upper(@idCuenta) and upper(trim(DescCuenta)) = upper(@DescCuenta) and NoCuenta <> @DescCuenta) THEN 1 ELSE 0 END AS 'RES'";
                        Comando.Parameters.Add(new MySqlParameter("@idCuenta", MySqlDbType.VarChar, oRegistroEN.idCuenta.Trim().Length)).Value = oRegistroEN.idCuenta.Trim();
                        Comando.Parameters.Add(new MySqlParameter("@DescCuenta", MySqlDbType.VarChar, oRegistroEN.DescCuenta.Trim().Length)).Value = oRegistroEN.DescCuenta.Trim();
                        Comando.Parameters.Add(new MySqlParameter("@NoCuenta", MySqlDbType.Int32)).Value = oRegistroEN.NoCuenta;

                        break;

                    default:
                        throw new ArgumentException("La aperación solicitada no esta disponible");

                }

                Comando.CommandText = Consultas;

                Adaptador = new MySqlDataAdapter();
                DT = new DataTable();

                Adaptador.SelectCommand = Comando;
                Adaptador.Fill(DT);

                if (Convert.ToInt32(DT.Rows[0]["RES"].ToString()) > 0)
                {

                    DescripcionDeOperacion = string.Format("Ya existe una cuenta  dentro de nuestro sistema con la misma descripción: {0} {1}", Environment.NewLine, InformacionDelRegistro(oRegistroEN));
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
                TransaccionesEN oTran = InformacionDelaTransaccion(oRegistroEN, "VALIDAR", "REGISTRO DUPLICADO POR IDENTIFICADOR Y DESCRIPCION DE LA CUENTA", "ERROR");
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

        public bool ValidarRegistroDuplicadoPorCategoria(CuentaEN oRegistroEN, DatosDeConexionEN oDatos, string TipoDeOperacion)
        {
            oTransaccionesAD = new TransaccionesAD();

            try
            {

                Cnn = new MySqlConnection(TraerCadenaDeConexion(oDatos));
                Cnn.Open();

                Comando = new MySqlCommand();
                Comando.Connection = Cnn;
                Comando.CommandType = CommandType.Text;

                switch (TipoDeOperacion.Trim().ToUpper())
                {

                    case "AGREGAR":

                        Consultas = @"SELECT CASE WHEN EXISTS(SELECT idCuenta FROM Cuenta WHERE upper(idCuenta) = upper(@idCuenta)  and upper(trim(DescCuenta)) = upper(@DescCuenta) and idCategoriaDeCuenta = @idCategoriaDeCuenta ) THEN 1 ELSE 0 END AS 'RES'";
                        Comando.Parameters.Add(new MySqlParameter("@idCuenta", MySqlDbType.VarChar, oRegistroEN.idCuenta.Trim().Length)).Value = oRegistroEN.idCuenta.Trim();
                        Comando.Parameters.Add(new MySqlParameter("@DescCuenta", MySqlDbType.VarChar, oRegistroEN.DescCuenta.Trim().Length)).Value = oRegistroEN.DescCuenta.Trim();
                        Comando.Parameters.Add(new MySqlParameter("@idCategoriaDeCuenta", MySqlDbType.Int32)).Value = oRegistroEN.oCategoriaDeCuentaEN.idCategoriaDeCuenta;

                        break;

                    case "ACTUALIZAR":

                        Consultas = @"SELECT CASE WHEN EXISTS(SELECT idCuenta FROM Cuenta WHERE upper(idCuenta) = upper(@idCuenta) and upper(trim(DescCuenta)) = upper(@DescCuenta) and idCategoriaDeCuenta = @idCategoriaDeCuenta and NoCuenta <> @DescCuenta) THEN 1 ELSE 0 END AS 'RES'";
                        Comando.Parameters.Add(new MySqlParameter("@idCuenta", MySqlDbType.VarChar, oRegistroEN.idCuenta.Trim().Length)).Value = oRegistroEN.idCuenta.Trim();
                        Comando.Parameters.Add(new MySqlParameter("@DescCuenta", MySqlDbType.VarChar, oRegistroEN.DescCuenta.Trim().Length)).Value = oRegistroEN.DescCuenta.Trim();
                        Comando.Parameters.Add(new MySqlParameter("@idCategoriaDeCuenta", MySqlDbType.Int32)).Value = oRegistroEN.oCategoriaDeCuentaEN.idCategoriaDeCuenta;
                        Comando.Parameters.Add(new MySqlParameter("@NoCuenta", MySqlDbType.Int32)).Value = oRegistroEN.NoCuenta;

                        break;

                    default:
                        throw new ArgumentException("La aperación solicitada no esta disponible");

                }

                Comando.CommandText = Consultas;

                Adaptador = new MySqlDataAdapter();
                DT = new DataTable();

                Adaptador.SelectCommand = Comando;
                Adaptador.Fill(DT);

                if (Convert.ToInt32(DT.Rows[0]["RES"].ToString()) > 0)
                {

                    DescripcionDeOperacion = string.Format("Ya existe una cuenta  dentro de nuestro sistema con la misma descripción: {0} {1}", Environment.NewLine, InformacionDelRegistro(oRegistroEN));
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
                TransaccionesEN oTran = InformacionDelaTransaccion(oRegistroEN, "VALIDAR", "REGISTRO DUPLICADO POR IDENTIFICADOR,  DESCRIPCION DE LA CUENTA Y TIPO DE CUENTA", "ERROR");
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

        public bool ValidarSiElRegistroEstaVinculado(CuentaEN oRegistroEN, DatosDeConexionEN oDatos, string TipoDeOperacion)
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

                Comando.Parameters.Add(new MySqlParameter("@CampoABuscar_", MySqlDbType.VarChar, 200)).Value = "NoCuenta";
                Comando.Parameters.Add(new MySqlParameter("@ValorCampoABuscar", MySqlDbType.Int32)).Value = oRegistroEN.NoCuenta;
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
                    //Agregamos la Transacción....
                    TransaccionesEN oTran = InformacionDelaTransaccion(oRegistroEN, "VALIDAR", "REGISTRO VINCULADO A OTRAS TABLAS", "CORRECTO");
                    oTransaccionesAD.Agregar(oTran, oDatos);

                    return true;
                }

            }
            catch (Exception ex)
            {
                this.Error = ex.Message;

                DescripcionDeOperacion = string.Format("Se produjo el seguiente error: '{2}' al validar el registro. {0} {1} ", Environment.NewLine, InformacionDelRegistro(oRegistroEN), ex.Message);
                TransaccionesEN oTran = InformacionDelaTransaccion(oRegistroEN, "VALIDAR", "REGISTRO VINCULADO A OTRAS TABLAS", "CORRECTO");
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

        public bool EvaluarSiElRegistrosEstaAsociado(CuentaEN oRegistroEN, DatosDeConexionEN oDatos, string TipoDeOperacion)
        {
            oTransaccionesAD = new TransaccionesAD();

            try
            {

                Cnn = new MySqlConnection(TraerCadenaDeConexion(oDatos));
                Cnn.Open();

                Comando = new MySqlCommand();
                Comando.Connection = Cnn;
                Comando.CommandType = CommandType.Text;
                Comando.CommandText = @"select case when exists(Select NoCuenta from cuenta where CuentaMadre = @NoCuenta) then 1 else 0 end as 'RES'";

                Comando.Parameters.Add(new MySqlParameter("@NoCuenta", MySqlDbType.Int32)).Value = oRegistroEN.NoCuenta;
               
                Adaptador = new MySqlDataAdapter();
                DT = new DataTable();

                Adaptador.SelectCommand = Comando;
                Adaptador.Fill(DT);

                if (Convert.ToInt32(DT.Rows[0].ItemArray[0].ToString()) == 0)
                {
                    return false;
                }
                else
                {

                    this.Error = string.Format("La operación: '{0}' no se puede completar por el registro esta asociado a otras cuentas", TipoDeOperacion);
                    DescripcionDeOperacion = this.Error;
                    
                    return true;
                }

            }
            catch (Exception ex)
            {
                this.Error = ex.Message;

                DescripcionDeOperacion = string.Format("Se produjo el seguiente error: '{2}' al validar el registro. {0} {1} ", Environment.NewLine, InformacionDelRegistro(oRegistroEN), ex.Message);
                TransaccionesEN oTran = InformacionDelaTransaccion(oRegistroEN, "VALIDAR", "RIGISTRO ASOCIADO A CUENTA MADRE", "ERROR");
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

        private TransaccionesEN InformacionDelaTransaccion(CuentaEN oCuenta, String TipoDeOperacion, String Descripcion, String Estado)
        {
            TransaccionesEN oRegistroEN = new TransaccionesEN();

            oRegistroEN.idregistro = oCuenta.NoCuenta;
            oRegistroEN.Modelo = "CuentasAD";
            oRegistroEN.Modulo = "Cuentas";
            oRegistroEN.Tabla = "Cuenta";
            oRegistroEN.tipodeoperacion = TipoDeOperacion;
            oRegistroEN.Estado = Estado;
            oRegistroEN.ip = oCuenta.oLoginEN.NumeroIP;
            oRegistroEN.idusuario = oCuenta.oLoginEN.idUsuario;
            oRegistroEN.idusuarioaprueba = oCuenta.oLoginEN.idUsuario;
            oRegistroEN.descripciondelusuario = DescripcionDeOperacion;
            oRegistroEN.descripcioninterna = Descripcion;
            oRegistroEN.NombreDelEquipo = oCuenta.oLoginEN.NombreDelComputador;

            return oRegistroEN;
        }

        private string InformacionDelRegistro(CuentaEN oRegistroEN) {

            string Cadena = @"idCuenta: {0}, DescCuenta: {1}, SaldoCuenta: {2}, NivelCuenta: {3}, CuentaMadre: {4}, IdUsuarioDeCreacion: {5}, FechaDeCreacion: {6}, IdUsuarioDeModificacion: {7}, FechaDeModificacion: {8}, idCategoriaDeCuenta: {9}, idTipoDeCuenta: {10}, NoCuenta: {11}";
            Cadena = string.Format(Cadena, oRegistroEN.idCuenta,oRegistroEN.DescCuenta, oRegistroEN.SaldoCuenta, oRegistroEN.NivelCuenta, oRegistroEN.CuentaMadre, oRegistroEN.IdUsuarioDeCreacion, oRegistroEN.FechaDeCreacion, oRegistroEN.IdUsuarioDeModificacion, oRegistroEN.FechaDeModificacion, oRegistroEN.oCategoriaDeCuentaEN.idCategoriaDeCuenta, oRegistroEN.oTipoDeCuentaEN.idTipoDeCuenta, oRegistroEN.NoCuenta);
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
