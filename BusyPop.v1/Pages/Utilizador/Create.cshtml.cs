using BusyPop.Pages.Categorias;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace BusyPop.Pages.Utilizador
{
    public class CreateModel : PageModel
    {
        public UtilizadoresInfo info = new UtilizadoresInfo();
        public String errorMsg = "";
        public String sucessMsg = "";

        public void OnGet()
        {
        }

        public void OnPost()
        {
            info.nome = Request.Form["name"];
            info.email = Request.Form["email"];
            info.pass = Request.Form["pass"];

            if (info.nome.Length == 0 || info.email.Length == 0 || info.pass.Length == 0)
            {
                errorMsg = "Preencha todos os campos!!";
                return;
            }

            //inserir na base de dados
            try
            {
                String conn = DataBaseSession.DataBaseString;

                using (SqlConnection connection = new SqlConnection(conn))
                {
                    connection.Open();
                    String SQL = "INSERT INTO Utilizadores " +
                                 "(nome,email,pass,tipo) VALUES " +
                                 "(@nome,@email,@pass,@tipo);";

                    using (SqlCommand command = new SqlCommand(SQL, connection))
                    {
                        command.Parameters.AddWithValue("@nome", info.nome);
                        command.Parameters.AddWithValue("@email", info.email);
                        command.Parameters.AddWithValue("@pass", info.pass);
                        command.Parameters.AddWithValue("@tipo", "3");

                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception e)
            {
                errorMsg = e.Message;
                return;
            }

            info.nome = "";
            info.email = "";
            info.pass = "";
            sucessMsg = "Utilizador adicionada com sucesso!!";

            Response.Redirect("/Categorias/ClienteIndex");
        }
    }
}
