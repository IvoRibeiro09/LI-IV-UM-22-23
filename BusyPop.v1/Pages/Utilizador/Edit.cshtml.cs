using BusyPop.Pages.Categorias;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace BusyPop.Pages.Utilizador
{
    public class EditModel : PageModel
    {
        public UtilizadoresInfo info = new UtilizadoresInfo();
        public String errorMsg = "";
        public String sucessMsg = "";

        public void OnGet()
        {
            String id = Request.Query["id"];

            try
            {
                String conn = DataBaseSession.DataBaseString;

                using (SqlConnection connection = new SqlConnection(conn))
                {
                    connection.Open();
                    String sql = "SELECT * FROM Utilizadores WHERE id=@id";
                    using (SqlCommand cmd = new SqlCommand(sql, connection))
                    {
                        cmd.Parameters.AddWithValue("@id", id);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                info.id = "" + reader.GetInt32(0);
                                info.nome = reader.GetString(1);
                                info.email =reader.GetString(2);
                                info.pass = reader.GetString(3);
                                info.tipo = "" + reader.GetInt32(4);
                            }
                        }
                    }
                }

            }
            catch (Exception e)
            {
                errorMsg = e.Message;
                return;
            }
        }

        public void OnPost()
        {
            info.id = Request.Form["id"];
            info.nome = Request.Form["nome"];
            info.email = Request.Form["email"];
            info.pass = Request.Form["pass"];
            info.tipo = Request.Form["tipo"];

            if (info.nome.Length == 0 || info.email.Length == 0 || info.pass.Length == 0)
            {
                errorMsg = "Preencha todos os campos!!";
                return;
            }

            try
            {
                String conn = "Data Source=LAPTOP-TTD3TLM1;Initial Catalog=BusyPopDB;Integrated Security=True";

                using (SqlConnection connection = new SqlConnection(conn))
                {
                    connection.Open();
                    String SQL = "UPDATE Utilizadores " +
                                 "SET nome=@nome,email=@email,pass=@pass,tipo=@tipo " +
                                 "WHERE id=@id";

                    using (SqlCommand command = new SqlCommand(SQL, connection))
                    {
                        command.Parameters.AddWithValue("@nome", info.nome);
                        command.Parameters.AddWithValue("@email", info.email);
                        command.Parameters.AddWithValue("@pass", info.pass);
                        command.Parameters.AddWithValue("@tipo", info.tipo);
                        command.Parameters.AddWithValue("@id", info.id);

                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception e)
            {
                errorMsg = e.Message;
                return;
            }

            Response.Redirect("/Utilizador/Index");
        }
    }
}
