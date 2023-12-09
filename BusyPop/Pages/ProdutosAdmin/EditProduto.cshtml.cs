using BusyPop.Pages.Categorias;
using BusyPop.Pages.ProdutosAdmin;
using BusyPop.Pages.Utilizador;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace BusyPop.Pages.ProdutosAdmin
{
    public class EditarProdutoModel : PageModel
    {
        public ProductInfo info = new ProductInfo();
        public string errorMsg = "";
        public string sucessMsg = "";

        public void OnGet()
        {
            String id = Request.Query["id"];

            try
            {
                String conn = DataBaseSession.DataBaseString;

                using (SqlConnection connection = new SqlConnection(conn))
                {
                    connection.Open();
                    String sql = "SELECT * FROM Produtos WHERE id=@id";
                    using (SqlCommand cmd = new SqlCommand(sql, connection))
                    {
                        cmd.Parameters.AddWithValue("@id", id);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                info.id = "" + reader.GetInt32(0);
                                info.nome = reader.GetString(1);
                                info.quantidade = "" + reader.GetInt32(2);
								info.price = "" + reader.GetDecimal(3);
								info.imagelink = reader.GetString(4);
                                info.standId = ""+ reader.GetInt32(5);
                                info.descri = reader.GetString(7);

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
            
            if (SessaoDeUtilizacao.Utype != "1"){
                errorMsg = "Nao tem permissoes!!";
                return;
            }
            info.id = Request.Form["id"];
            info.nome = Request.Form["nome"];
            info.quantidade = Request.Form["quantidade"];
            info.price = Request.Form["price"];
            info.imagelink = Request.Form["imagelink"];
            info.descri = Request.Form["descri"];

            if (info.nome.Length == 0 || info.quantidade.Length == 0 || info.price.Length == 0 || info.descri.Length == 0)
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
                    string SQL = "UPDATE Produtos " +
                                 "SET nome=@nome,quantidade=@quantidade,price=@price,imagelink=@imagelink,descri=@descri " +
                                 "WHERE id=@id";

                    using (SqlCommand command = new SqlCommand(SQL, connection))
                    {
						command.Parameters.AddWithValue("@id", info.id);
						command.Parameters.AddWithValue("@nome", info.nome);
                        command.Parameters.AddWithValue("@quantidade", info.quantidade);
                        command.Parameters.AddWithValue("@price", info.price);
                        command.Parameters.AddWithValue("@imagelink", info.imagelink);
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
            info.idVendedor = "";
            info.descri = "";
            sucessMsg = "Produto adicionada com sucesso!!";

            Response.Redirect("/ProdutosAdmin/Index");
        }
    }
}
