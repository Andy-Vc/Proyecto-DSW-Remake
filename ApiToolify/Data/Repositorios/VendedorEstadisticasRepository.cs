using ApiToolify.Data.Contratos;
using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.Data.SqlClient;
using System.Data;

namespace ApiToolify.Data.Repositorios
{
    public class VendedorEstadisticasRepository : IEstadistica
    {
        private readonly IConfiguration _config;
        private readonly string cadenaConexion;

        public VendedorEstadisticasRepository(IConfiguration config)
        {
            _config = config;
            this.cadenaConexion = _config["ConnectionStrings:DB"];
        }

        public long ContarProductosVendidosPorMes(int id,string fechaMes)
        {
            long resultado = 0;
            using (var con = new SqlConnection(cadenaConexion))
            {
                con.Open();
                using (var cmd = new SqlCommand("usp_contarProductosVendidosPorMes", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@IdUsuario", id);
                    cmd.Parameters.AddWithValue("@FechaMes", fechaMes);

                    var conteo = cmd.ExecuteScalar();
                    if (conteo != null)
                    {
                        resultado = Convert.ToInt64(conteo);
                    }
                }
            }
            return resultado;
        }

        public long ContarVentasPorMes(int id,string fechaMes)
        {
            long resultado = 0;
            using (var con = new SqlConnection(cadenaConexion))
            {
                con.Open();
                using (var cmd = new SqlCommand("usp_contarVentasPorMes", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@FechaMes", fechaMes);
                    cmd.Parameters.AddWithValue("@IdUsuario", id);
                    var conteo = cmd.ExecuteScalar();
                    if (conteo != null)
                    {
                        resultado = Convert.ToInt64(conteo);
                    }
                }
            }
            return resultado;
        }

        public double ObtenerIngresosTotales(int id)
        {
            long resultado = 0;
            using (var con = new SqlConnection(cadenaConexion))
            {
                con.Open();
                using (var cmd = new SqlCommand("usp_obtenerIngresosTotales", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@IdUsuario", id);

                    var conteo = cmd.ExecuteScalar();
                    if (conteo != null)
                    {
                        resultado = Convert.ToInt64(conteo);
                    }
                }
            }
            return resultado;
        }

        public long ObtenerTotalProductosVendidos(int id)
        {
            long resultado = 0;
            using (var con = new SqlConnection(cadenaConexion))
            {
                con.Open();
                using (var cmd = new SqlCommand("usp_obtenerTotalProductosVendidos", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@IdUsuario", id);

                    var conteo = cmd.ExecuteScalar();
                    if (conteo != null)
                    {
                        resultado = Convert.ToInt64(conteo);
                    }
                }
            }
            return resultado;
        }

        public long ObtenerTotalVentas(int id)
        {
            long resultado = 0;
            using (var con = new SqlConnection(cadenaConexion))
            {
                con.Open();
                using (var cmd = new SqlCommand("usp_obtenerTotalVentas", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@IdUsuario", id);

                    var conteo = cmd.ExecuteScalar();
                    if (conteo != null)
                    {
                        resultado = Convert.ToInt64(conteo);
                    }
                }
            }
            return resultado;
        }
    }
}
