using BusyPop.Pages.Categorias;
using BusyPop.Pages.ProdutosAdmin;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace BusyPop.Pages.ProdutosAdmin
{
    public class AdicionarProdutoModel : PageModel
    {
        public ProductInfo info = new ProductInfo();
        public string errorMsg = "";
        public string sucessMsg = "";

        public void OnGet()
        {
        }

        public void OnPost()
        {
            info.nome = Request.Form["nome"];
            info.quantidade = Request.Form["quantidade"];
            info.price = Request.Form["price"];
            info.imagelink = Request.Form["imagelink"];
            info.standId = Request.Form["standId"];
            String idVendedor = SessaoDeUtilizacao.Uid;
            info.descri = Request.Form["descri"];

            if (info.nome.Length == 0 || info.quantidade.Length == 0 || info.price.Length == 0 || info.standId.Length == 0 || info.descri.Length == 0)
            {
                errorMsg = "Preencha todos os campos!!";
                return;
            }

            //inserir na base de dados
            try
            {
                string conn = DataBaseSession.DataBaseString;

                using (SqlConnection connection = new SqlConnection(conn))
                {
                    connection.Open();
                    string SQL = "INSERT INTO Produtos " +
                                 "(nome,quantidade,price,imagelink,standId,idVendedor,descri) VALUES " +
                                 "(@nome,@quantidade,@price,@imagelink,@standId,@idVendedor,@descri);";

                    using (SqlCommand command = new SqlCommand(SQL, connection))
                    {
                        command.Parameters.AddWithValue("@nome", info.nome);
                        command.Parameters.AddWithValue("@quantidade", info.quantidade);
                        command.Parameters.AddWithValue("@price", info.price);
                        command.Parameters.AddWithValue("@imagelink", info.imagelink);
                        command.Parameters.AddWithValue("@standId", info.standId);
                        command.Parameters.AddWithValue("@idVendedor", idVendedor);
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
            info.quantidade = "";
            info.price = "";
            info.imagelink = "";
            info.standId = "";
            info.descri = "";
            sucessMsg = "Produto adicionada com sucesso!!";

            Response.Redirect("/ProdutosAdmin/Index");
        }
    }
}
