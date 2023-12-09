using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using System.Security.Cryptography;

namespace BusyPop.Pages.Categorias
{
    public class EditModel : PageModel
    {
        public CategoryInfo info = new CategoryInfo();
        public String errorMsg = "";
        public String sucessMsg = "";

        public void OnGet()
        {
            String id = Request.Query["id"];

            try
            {
                String conn = DataBaseSession.DataBaseString;

                using(SqlConnection connection = new SqlConnection(conn))
                {
                    connection.Open();
                    String sql = "SELECT * FROM Category WHERE id=@id";
                    using (SqlCommand cmd = new SqlCommand(sql, connection))
                    {
                        cmd.Parameters.AddWithValue("@id", id);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                info.id = "" + reader.GetInt32(0);
                                info.category = reader.GetString(1);
                                info.capacity = "" + reader.GetInt32(2);
                                info.description = reader.GetString(3);

                            }
                        }
                    }
                }

            }
            catch(Exception e)
            {
                errorMsg = e.Message;
                return;
            }
        }

        public void OnPost()
        {
            info.id = Request.Form["id"];
            info.category= Request.Form["category"];
            info.capacity = Request.Form["capacity"];
            info.description = Request.Form["description"];

            if (info.category.Length == 0 || info.capacity.Length == 0 || info.description.Length == 0)
            {
                errorMsg = "Preencha todos os campos!!";
                return;
            }

            try
            {
                String conn = DataBaseSession.DataBaseString;

                using (SqlConnection connection = new SqlConnection(conn))
                {
                    connection.Open();
                    String SQL = "UPDATE Category " +
                                 "SET category=@category,capacity=@capacity,description=@description " +
                                 "WHERE id=@id";

                    using (SqlCommand command = new SqlCommand(SQL, connection))
                    {
                        command.Parameters.AddWithValue("@category", info.category);
                        command.Parameters.AddWithValue("@capacity", info.capacity);
                        command.Parameters.AddWithValue("@description", info.description);
                        command.Parameters.AddWithValue("@id", info.id);

                        command.ExecuteNonQuery();
                    }
                }
            }
            catch(Exception e)
            {
                errorMsg = e.Message;
                return ;
            }

            Response.Redirect("/Categorias/Index");
        }

        
    }
}
