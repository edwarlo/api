using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuController : ControllerBase
    {
        private readonly string cadenaSQL;

        public UsuController(IConfiguration config)
        {
            cadenaSQL = config.GetConnectionString("nombreCadenaBD");
        }

        [HttpGet]
        [Route("lista")]
        public IActionResult Lista()
        {
            List<Usuario> lista = new List<Usuario>();

            try
            {
                using (var conexion = new SqlConnection(cadenaSQL))
                {
                    conexion.Open();
                    var cmd = new SqlCommand("procesoAlmacenado", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    using (var rd = cmd.ExecuteReader())
                    {
                        while (rd.Read())
                        {
                            lista.Add(new Usuario()
                            {
                                Id_usuario = Convert.ToInt32(rd["Id_usuario"]),
                                nombre = rd["nombre"].ToString(),
                                apellido = rd["apellido "].ToString(),
                                documento = rd["documento"].ToString()
                            });
                        }
                    }
                }

                return StatusCode(StatusCodes.Status200OK, new { mensaje = "ok", Response = lista });

            } catch (Exception error) {

                return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = error.Message, Response = lista });

            }
        }
    }
}
