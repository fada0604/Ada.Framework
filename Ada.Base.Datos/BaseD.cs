﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ada.Base.BusinessObject;
using System.Reflection;
using System.Data;

namespace Ada.Base.Data
{
    public abstract class BaseD<T, U> 
        where U: BaseBO
        
    {
        #region Propiedades
        public String cadenaConexion { get; set; }
        public String nombreSPInsertar { get; set; }
        public String nombreSPActualizar { get; set; }
        public String nombreSPConsultar { get; set; }
        public String nombreSPEliminar { get; set; }
        public String nombreSPListar { get; set; }
        #endregion
        
        #region Constructores
        public BaseD()
        {
            obtenerCadenaConexion();
        }
        #endregion

        #region Metodos
        public String obtenerCadenaConexion()
        {
            SqlConnectionStringBuilder cnnSb = new SqlConnectionStringBuilder();
            cnnSb.DataSource = @"FRANCIAD\SQLEXPRESS";
            cnnSb.InitialCatalog = "Maestra";
            cnnSb.IntegratedSecurity = true;
            cadenaConexion = cnnSb.ToString();
            return cadenaConexion;
        }

        /// <summary>
        /// Inserta registros en la base de datos
        /// </summary>
        /// <param name="objeto">Definición del objeto</param>
        /// <param name="definicion">Datos</param>
        /// <returns>Entero, 1 es satisfactorio</returns>
        public virtual int Insertar(T objeto, U definicion)
        {
            int resultado = 0;
            try
            {
                SqlConnection cnn = new SqlConnection(cadenaConexion);
                using (cnn)
                {
                    cnn.Open();
                    SqlCommand cmd = new SqlCommand(nombreSPInsertar, cnn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    AgregarParametrosConValor(definicion, cmd, 0);

                    resultado = cmd.ExecuteNonQuery();

                    cnn.Close();
                }

                return resultado;
            }
            catch (ExcepcionesPersonalizadas)
            {
                throw new ExcepcionesPersonalizadas("Se ha producido un error ingresando registros en la base de datos.", new Exception("Capa -> Data. Método -> Insertar"));
            }
            catch (SqlException sqlEx)
            {
                throw new ExcepcionesPersonalizadas("Se ha producido un error ingresando registros en la base de datos.", new Exception(sqlEx.InnerException + "Capa -> Data. Método -> Insertar"));
            }
            catch (Exception ex)
            {
                throw new Exception("Error general, BaseD", ex.InnerException);
            }
        }

        /// <summary>
        /// Actualiza registros en la base de datos
        /// </summary>
        /// <param name="objeto">Definición del objeto</param>
        /// <param name="definicion">Datos</param>
        /// <returns>Entero, 1 es satisfactorio</returns>
        public virtual int Actualizar(T objeto, U definicion)
        {
            int resultado = 0;
            try
            {
                SqlConnection cnn = new SqlConnection(cadenaConexion);
                using (cnn)
                {
                    cnn.Open();
                    SqlCommand cmd = new SqlCommand(nombreSPActualizar, cnn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;

                    AgregarParametrosConValor(definicion, cmd, 1);

                    resultado = cmd.ExecuteNonQuery();

                    cnn.Close();
                }

                return resultado;
            }
            catch (ExcepcionesPersonalizadas)
            {
                throw new ExcepcionesPersonalizadas("Se ha producido un error actualizando registros en la base de datos.", new Exception("Capa -> Data. Método -> Actualizar"));
            }
            catch (SqlException sqlEx)
            {
                throw new ExcepcionesPersonalizadas("Se ha producido un error actualizando registros en la base de datos.", new Exception(sqlEx.InnerException + "Capa -> Data. Método -> Actualizar"));
            }
            catch (Exception ex)
            {
                throw new Exception("Error general, BaseD", ex.InnerException);
            }
        }

        /// <summary>
        /// Elimina registros en la base de datos
        /// </summary>
        /// <param name="objeto">Definición del objeto</param>
        /// <param name="definicion">Datos</param>
        /// <returns>Entero, 1 es satisfactorio</returns>
        public virtual int Eliminar(T objeto, U definicion)
        {
            int resultado = 0;

            try
            {
                SqlConnection cnn = new SqlConnection(cadenaConexion);
                using (cnn)
                {
                    cnn.Open();
                    SqlCommand cmd = new SqlCommand(nombreSPEliminar, cnn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;

                    AgregarParametrosConValor(definicion, cmd, 2);

                    resultado = cmd.ExecuteNonQuery();

                    cnn.Close();
                }

                return resultado;
            }
            catch (ExcepcionesPersonalizadas)
            {
                throw new ExcepcionesPersonalizadas("Se ha producido un error eliminando registros en la base de datos.", new Exception("Capa -> Data. Método -> Eliminar"));
            }
            catch (SqlException sqlEx)
            {
                throw new ExcepcionesPersonalizadas("Se ha producido un error eliminando registros en la base de datos.", new Exception(sqlEx.InnerException + "Capa -> Data. Método -> Eliminar"));
            }
            catch (Exception ex)
            {
                throw new Exception("Error general, BaseD", ex.InnerException);
            }
        }

        /// <summary>
        /// Consulta registros en la base de datos con filtros
        /// </summary>
        /// <param name="objeto">Definición del objeto</param>
        /// <param name="definicion">Datos</param>
        /// <returns>Retorna el objeto modificado</returns>
        public virtual U Consultar(T objeto, ref U definicion)
        {
            U objetoResultante = (U)Activator.CreateInstance(definicion.GetType(), true);
            
            try
            {
                SqlConnection cnn = new SqlConnection(cadenaConexion);
                using (cnn)
                {

                    cnn.Open();
                    SqlCommand cmd = new SqlCommand(nombreSPConsultar, cnn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;

                    AgregarParametrosConValor(definicion, cmd, 3);
                    
                    var lectura = cmd.ExecuteReader();
                
                    if (lectura.HasRows)
                    {
                        if (lectura.Read())
                        {
                            DataTable esquemaTabla = lectura.GetSchemaTable();

                            var query = from datos in esquemaTabla.AsEnumerable()
                                        select new
                                        {
                                            id = datos.Field<int>("ColumnOrdinal"),
                                            nombre = datos.Field<string>("ColumnName")
                                        };

                            var propiedades = definicion.GetType().GetProperties();

                            foreach (var fila in query)
                            {
                                propiedades.Where(a => a.Name.Equals(fila.nombre)).FirstOrDefault().SetValue(objetoResultante, lectura[fila.id]);
                            }
                        }
                    }

                    cnn.Close();
                }
                return objetoResultante;
            }
            catch (ExcepcionesPersonalizadas)
            {
                throw new ExcepcionesPersonalizadas("Se ha producido un error consultando registros en la base de datos.", new Exception("Capa -> Data.  Método -> Consultar"));
            }
            catch (SqlException sqlEx)
            {
                throw new ExcepcionesPersonalizadas("Se ha producido un error consultando registros en la base de datos.", new Exception(sqlEx.InnerException + "Capa -> Data. Método -> Consultar"));
            }
            catch (Exception ex)
            {
                throw new Exception("Error general, BaseD", ex.InnerException);
            }
        }

        /// <summary>
        /// Lista todos los registros de un objeto dado. No hay filtros
        /// </summary>
        /// <param name="objeto">Definición del objeto</param>
        /// <param name="definicion">Datos</param>
        /// <returns>Retorna una lista de objetos según la definición indicada</returns>
        public virtual List<U> Listar(T objeto, U definicion)
        {
            List<U> listaResultante = (List<U>)Activator.CreateInstance(typeof(List<U>), true);

            try
            {
                SqlConnection cnn = new SqlConnection(cadenaConexion);
                using (cnn)
                {

                    cnn.Open();
                    SqlCommand cmd = new SqlCommand(nombreSPListar, cnn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    
                    var lectura = cmd.ExecuteReader();

                    if (lectura.HasRows)
                    {
                        while (lectura.Read())
                        {
                            DataTable esquemaTabla = lectura.GetSchemaTable();

                            var query = from datos in esquemaTabla.AsEnumerable()
                                        select new
                                        {
                                            id = datos.Field<int>("ColumnOrdinal"),
                                            nombre = datos.Field<string>("ColumnName")
                                        };

                            var propiedades = definicion.GetType().GetProperties();
                            U objetoResultante = (U)Activator.CreateInstance(definicion.GetType(), true);

                            foreach (var fila in query)
                            {

                                propiedades.Where(a => a.Name.Equals(fila.nombre)).FirstOrDefault().SetValue(objetoResultante, lectura[fila.id]);
                            }

                            listaResultante.Add(objetoResultante);
                        }
                    }

                    cnn.Close();
                }

                return listaResultante;
            }
            catch (ExcepcionesPersonalizadas)
            {
                throw new ExcepcionesPersonalizadas("Se ha producido un error listando registros en la base de datos.", new Exception("Capa -> Data.  Método -> Listar"));
            }
            catch (SqlException sqlEx)
            {
                throw new ExcepcionesPersonalizadas("Se ha producido un error listando registros en la base de datos.", new Exception(sqlEx.InnerException + "Capa -> Data. Método -> Listar"));
            }
            catch (Exception ex)
            {
                throw new Exception("Error general, BaseD", ex.InnerException);
            }
        }


        /// <summary>
        /// Retorna los campos formateados a fin de que viajen a la base de datos con el arroba, la inicial del tipo de datos y el nombre del campo
        /// </summary>
        /// <param name="nombreCampo">Nombre del campo</param>
        /// <param name="tipo">Tipo de datos</param>
        /// <returns>Retorna el string formateado</returns>
        private String RetornarNombreCampoFormateado(String nombreCampo, Enumerados.TipoDatos tipo)
        {
            String resultado = "";
            switch (tipo)
            {
                case Enumerados.TipoDatos.Bit:
                    resultado = String.Format("@b{0}", nombreCampo);
                    break;
                case Enumerados.TipoDatos.Char:
                    resultado = String.Format("@c{0}", nombreCampo);
                    break;
                case Enumerados.TipoDatos.Varchar:
                    resultado = String.Format("@s{0}", nombreCampo);
                    break;
                case Enumerados.TipoDatos.Int:
                    resultado = String.Format("@i{0}", nombreCampo);
                    break;
                case Enumerados.TipoDatos.Decimal:
                    resultado = String.Format("@d{0}", nombreCampo);
                    break;
                case Enumerados.TipoDatos.DateTime:
                    resultado = String.Format("@dt{0}", nombreCampo);
                    break;
                case Enumerados.TipoDatos.Guid:
                    resultado = String.Format("@g{0}", nombreCampo);
                    break;
                default:
                    resultado = String.Format("@{0}", nombreCampo);
                    break;
            }
            return resultado;
        }

        /// <summary>
        /// Método encargado de agregar los parámetros en el comando de forma dinámica
        /// </summary>
        /// <param name="definicion">BO del cual se van a recuperar las propiedades</param>
        /// <param name="cmd">Comando</param>
        /// <param name="tipo">Tipo de operación: 0 para insertar, 1 para actualizar, 2 para eliminar, 3 para consultar</param>
        private void AgregarParametrosConValor(U definicion, SqlCommand cmd, byte tipo)
        {
            var propiedades = definicion.GetType().GetProperties();
            foreach (var propiedad in propiedades)
            {
                foreach (var a in propiedad.GetCustomAttributes())
                {
                    if( (tipo == 0 && !((Ada.Base.BusinessObject.DatosAttribute)a).esPk) //Para insertar van todos menos el pk
                        || (tipo == 1) //Para actualizar van todos
                        || ((tipo == 2 || tipo == 3) && ((Ada.Base.BusinessObject.DatosAttribute)a).esPk)) //Para eliminar y consultar va solo el pk
                    {
                        cmd.Parameters.AddWithValue(
                            RetornarNombreCampoFormateado(
                                ((Ada.Base.BusinessObject.DatosAttribute)a).nombreCampo,
                                ((Ada.Base.BusinessObject.DatosAttribute)a).Tipo),
                                definicion.GetType().GetProperty(((Ada.Base.BusinessObject.DatosAttribute)a).nombreCampo).GetValue(definicion));
                    }
                }
            }
        }

        #endregion
    }

}
