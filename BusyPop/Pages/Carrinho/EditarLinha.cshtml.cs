using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace BusyPop.Pages.Carrinho
{
    public class EditarLinhaModel : PageModel
    {
        public CarrinhoInfo info = new CarrinhoInfo();
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
                    String sql = "SELECT * FROM Carrinho WHERE id=@id";
                    using (SqlCommand cmd = new SqlCommand(sql, connection))
                    {
                        cmd.Parameters.AddWithValue("@id", id);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                info.id = "" + reader.GetInt32(0);
                                info.nome = reader.GetString(1);
                                info.idVendedor = "" + reader.GetInt32(2);
                                info.idComprador = reader.GetDecimal(3).ToString();
                                info.quantidade = reader.GetString(4);
                                info.preco = "" + reader.GetInt32(5);
                                info.status = reader.GetString(6);
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
            info.nome = Request.Form["nome do produto"];
            info.idVendedor = Request.Form["vendedor"];
            info.idComprador = Request.Form["comprador"];
            info.quantidade = Request.Form["quantidade"];
            info.preco = Request.Form["preco"];
            info.status = Request.Form["status"];

            if (info.nome.Length == 0 || info.idVendedor.Length == 0 || info.idComprador.Length == 0 || info.quantidade.Length == 0 || info.preco.Length == 0 || info.status.Length == 0)
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
                    String SQL = "UPDATE Carrinho " +
                                 "SET Carrinho=@nome_do_produto,@id_vendedor,@id_comprador,@quantidade,@price,@status " +
                                 "WHERE id=@id";

                    using (SqlCommand command = new SqlCommand(SQL, connection))
                    {
                        command.Parameters.AddWithValue("@nome_do_produto", info.nome);
                        command.Parameters.AddWithValue("@id_vendedor", info.idVendedor);
                        command.Parameters.AddWithValue("@id_comprador", info.idComprador);
                        command.Parameters.AddWithValue("@quantidade", info.quantidade);
                        command.Parameters.AddWithValue("@price", info.preco);
                        command.Parameters.AddWithValue("@status", info.status);

                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception e)
            {
                errorMsg = e.Message;
                return;
            }

            Response.Redirect("/Carrinho/AdminIndex");
        }


    }
}

