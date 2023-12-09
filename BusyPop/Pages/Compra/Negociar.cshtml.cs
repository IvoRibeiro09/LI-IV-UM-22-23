using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace BusyPop.Pages.Compra
{
    public class NegociarModel : PageModel { 

            public ProductInfo info = new ProductInfo();
            public CarrinhoInfo carinfo = new CarrinhoInfo();
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
                        String sql = "SELECT * FROM Produtos WHERE id=@id";
                        using (SqlCommand command = new SqlCommand(sql, connection))
                        {
                            command.Parameters.AddWithValue("@id", id);
                            using (SqlDataReader reader = command.ExecuteReader())
                            {
                                if(reader.Read())
                                {

                                    info.id = "" + reader.GetInt32(0);
                                    info.nome = reader.GetString(1);
                                    info.quantidade = "" + reader.GetInt32(2);
                                    info.price = reader.GetDecimal(3).ToString();
                                    info.imagelink = reader.GetString(4);
                                    info.standId = "" + reader.GetInt32(5);
                                    info.idVendedor = "" + reader.GetInt32(6);
                                    info.descri = reader.GetString(7);
                                }
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("Exception: " + e.ToString());
                }
            }


            public void OnPost()
            {
                info.id = Request.Form["id"];
                info.quantidade = Request.Form["quanti"];
                carinfo.quantidade = Request.Form["quantidade"];
                carinfo.preco = Request.Form["preco"];
                carinfo.idComprador = SessaoDeUtilizacao.Uid;
                int status = 1;

                if (carinfo.quantidade.Length == 0 || carinfo.preco.Length == 0 || carinfo.idComprador.Length == 0)
                {
                    errorMsg = "Preencha todos os campos e certifique-se que esta logado!!";
                    return;
                }
                try
                {
                    int i = Convert.ToInt16(info.quantidade);
                    int j = Convert.ToInt16(carinfo.quantidade);
                if (i >= j)
                {
                    if (info.price != carinfo.preco) status = 1;
                    else status = 0;
                    try
                    {
                        String conn = DataBaseSession.DataBaseString;

                        using (SqlConnection connection = new SqlConnection(conn))
                        {
                            connection.Open();
                            String SQL = "INSERT INTO Carrinho " +
                                         "(nome,idVendedor,idComprador,quantidade,preco,status) VALUES " +
                                         "(@nome,@idVendedor,@idComprador,@quantidade,@preco,@status);";

                            using (SqlCommand command = new SqlCommand(SQL, connection))
                            {
                                command.Parameters.AddWithValue("@nome", info.nome);
                                command.Parameters.AddWithValue("@idVendedor", info.idVendedor);
                                command.Parameters.AddWithValue("@idComprador", carinfo.idComprador);
                                command.Parameters.AddWithValue("@quantidade", carinfo.quantidade);
                                command.Parameters.AddWithValue("@preco", carinfo.preco);
                                command.Parameters.AddWithValue("@status", status);

                                command.ExecuteNonQuery();
                            }

                            String SQL2 = "UPDATE Produtos " +
                                        "SET quantidade=@quantidade " +
                                        "WHERE id=@id";

                            using (SqlCommand command2 = new SqlCommand(SQL2, connection))
                            {
                                command2.Parameters.AddWithValue("@quantidade", i - j);
                                command2.Parameters.AddWithValue("@id", info.id);

                                command2.ExecuteNonQuery();
                            }
                        }
                    }
                    catch (Exception e)
                    {
                            errorMsg = e.Message;
                            return;
                    }
                    Response.Redirect("/Prodotus/Index");
                }
                }
                catch (Exception)
                {
                    errorMsg = "Não existe tanta quantidade em stock!!";
                    return;
                }
            }
        public class CarrinhoInfo
        {
            public String id;
            public String nome;
            public String idVendedor;
            public String idComprador;
            public String quantidade;
            public String preco;
            public String status;
        }
    }

}
