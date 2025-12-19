using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using UniversityMvc.Models;

namespace UniversityMvc.Controllers
{
    public class CustomerController : Controller
    {
        private readonly string _connStr;

        public CustomerController(IConfiguration config)
        {
            _connStr = config.GetConnectionString("DefaultConnection");
        }

        public IActionResult Index()
        {
            List<Customer> customers = new List<Customer>();

            using (SqlConnection con = new SqlConnection(_connStr))
            {
                con.Open();
                string sql = "SELECT IdNumber, FirstName, LastName FROM tblCustomer";
                SqlCommand cmd = new SqlCommand(sql, con);

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    customers.Add(new Customer
                    {
                        IdNumber = reader.GetString(0),
                        FirstName = reader.GetString(1),
                        LastName = reader.GetString(2)
                    });
                }
            }

            return View(customers);
        }
        public IActionResult InsertTest()
        {
            using (SqlConnection con = new SqlConnection(_connStr))
            {
                con.Open();

                string sql = @"
            INSERT INTO tblCustomer (IdNumber, FirstName, LastName)
            VALUES ('999999999', N'בדיקה', N'MVC');
        ";

                SqlCommand cmd = new SqlCommand(sql, con);
                cmd.ExecuteNonQuery();
            }

            return Content("נוסף לקוח לבדיקה – בדוק ב-DB");
        }
    }
}