using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Ejercicio2.Models;
using System.Data;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace Ejercicio2.Controllers
{
    public class HomeController : Controller
    {

        string myConnectionString = "server=127.0.0.1;uid=root;pwd=;database=dbtest";
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Index(Person person)
        {
            MySqlConnection conn = new MySqlConnection(myConnectionString);
            try
            {
                Console.WriteLine("Connecting to MySQL...");
                conn.Open();

                string sql = $"INSERT INTO persons (distributor_id, nombre, apellido_paterno, apellido_materno) VALUES ('{person.DistributorId}', '{person.Nombre}', '{person.ApellidoPaterno}', '{person.ApellidoMaterno}');";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            conn.Close();
            Console.WriteLine("Done.");

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Privacy(string distributorid)
        {
            List<Informacion> info = new List<Informacion>();
            MySqlConnection conn = new MySqlConnection(myConnectionString);
            try
        {
            Console.WriteLine("Connecting to MySQL...");
            conn.Open();

            string rtn = "obtenerDatos";
            MySqlCommand cmd = new MySqlCommand(rtn, conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@distributor_id", distributorid);

            MySqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                Informacion infoAux = new Informacion();
                infoAux.NombreCompleto = rdr.GetValue(0).ToString();
                infoAux.Calle = rdr.GetValue(1).ToString();
                infoAux.NumeroCasa = rdr.GetValue(2).ToString();
                infoAux.Colonia = rdr.GetValue(3).ToString();
                info.Add(infoAux);
            }
            rdr.Close();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
        }

            conn.Close();
            Console.WriteLine("Done.");
            return View(info);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
