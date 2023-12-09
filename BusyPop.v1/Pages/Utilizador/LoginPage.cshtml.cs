using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace BusyPop.Pages.Utilizador
{
    public class LoginPageModel : PageModel
    {
        public UtilizadoresInfo info = new UtilizadoresInfo(); 
        public String errorMsg = "";
        public String sucessMsg = "";
        public string email;
        public string pass;
        
        public void OnGet()
        {
        }
        public void OnPost()
        {
            email = Request.Form["email"];
            pass = Request.Form["pass"];

            if (email.Length == 0 || pass.Length == 0)
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
                    String SQL1 = "SELECT * FROM Utilizadores WHERE email=@email AND pass=@pass";
                    using (SqlCommand cmd = new SqlCommand(SQL1, connection))
                    {
                        cmd.Parameters.AddWithValue("@email", email);
                        cmd.Parameters.AddWithValue("@pass", pass);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                info.id = "" + reader.GetInt32(0);
                                info.nome = reader.GetString(1);
                                info.email = reader.GetString(2);
                                info.pass = reader.GetString(3);
                                info.tipo = "" + reader.GetInt32(4);

                                SessaoDeUtilizacao.Uname = info.nome;
                                SessaoDeUtilizacao.Utype = info.tipo;
                                SessaoDeUtilizacao.Uid = info.id;
                                
                            }
                        }
                    }
                }
                if (SessaoDeUtilizacao.Utype == "3") Response.Redirect("/Categorias/ClienteIndex");
                else if (SessaoDeUtilizacao.Utype == "2") Response.Redirect("/Stand/Index");
                else Response.Redirect("/Categorias/Index");
            }
            catch (Exception)
            { 
                errorMsg = "Email ou Password incorretas, se de facto já estiver registado verifique se colocou os seus dados de maneira correta!!";
                return;
            }

        }
    }
}
