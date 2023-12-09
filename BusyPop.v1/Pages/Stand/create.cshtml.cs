using BusyPop.Pages.Categorias;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace BusyPop.Pages.Stand
{
    public class createModel : PageModel
    {
        public String vendedorId;
        public String nome;
        public String feiraid;
        public String errorMsg = "";
        public String sucessMsg = "";

        public void OnGet()
        {
        }

        public void OnPost()
        {
            nome = Request.Form["nome"];
            vendedorId = SessaoDeUtilizacao.Uid;
            feiraid = Request.Form["feiraid"];
            
            if(SessaoDeUtilizacao.Utype != "2")
            {
                errorMsg = "Nao tem permissoes!!";
                return;
            }
            if (nome.Length == 0 || vendedorId.Length == 0 || feiraid.Length == 0)
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
                    String SQL = "INSERT INTO Stand " +
                                 "(nome,vendedorId,feiraid) VALUES " +
                                 "(@nome,@vendedorId,@feiraid);";

                    using (SqlCommand command = new SqlCommand(SQL, connection))
                    {
                        command.Parameters.AddWithValue("@nome", nome);
                        command.Parameters.AddWithValue("@vendedorId", vendedorId);
                        command.Parameters.AddWithValue("@feiraid", feiraid);

                        command.ExecuteNonQuery();
                    }
                }


            }
            catch (Exception e)
            {
                errorMsg = e.Message;
                return;
            }


            nome = "";
            vendedorId = "";
            feiraid = "";
            sucessMsg = "Stand adicionada com sucesso!!";

            Response.Redirect("/Stand/Index");
        }
    }
}
