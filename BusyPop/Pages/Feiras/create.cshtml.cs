using BusyPop.Pages.Categorias;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace BusyPop.Pages.Feiras
{
    public class createModel : PageModel
    {
        public FeirasInfo info = new FeirasInfo();
        public String errorMsg = "";
        public String sucessMsg = "";

        public void OnGet()
        {
        }

        public void OnPost()
        {
            info.nome = Request.Form["nome"];
            info.categoryid = Request.Form["categoryid"];
            info.numSlots = Request.Form["numSlots"];
            info.descri = Request.Form["descri"];

            if (info.nome.Length == 0 || info.categoryid.Length == 0 || info.numSlots.Length == 0 || info.descri.Length == 0)
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
                    String SQL = "INSERT INTO Feiras " +
                                 "(nome,categoryid,numSlots,descri) VALUES " +
                                 "(@nome,@categoryid,@numSlots,@descri);";

                    using (SqlCommand command = new SqlCommand(SQL, connection))
                    {
                        command.Parameters.AddWithValue("@nome", info.nome);
                        command.Parameters.AddWithValue("@categoryid", info.categoryid);
                        command.Parameters.AddWithValue("@numSlots", info.numSlots);
                        command.Parameters.AddWithValue("@descri", info.descri);

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
            info.categoryid = "";
            info.numSlots = "";
            info.descri = "";
            sucessMsg = "Feira adicionada com sucesso!!";

            Response.Redirect("/Feiras/Index");
        }
    }
}
