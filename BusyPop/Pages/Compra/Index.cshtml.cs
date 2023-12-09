using BusyPop;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace BusyPop.Pages.Compra
{
    public class IndexModel : PageModel
    {
        public ProductInfo info = new ProductInfo();
        public ProductInfo info2 = new ProductInfo();
        public String errorMsg = "";
        public String sucessMsg = "";
        public int show = 0;
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
                            while (reader.Read())
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
            String quantidade = Request.Form["quantidade"];
            String preco = Request.Form["preco"];
            String idComprador = SessaoDeUtilizacao.Uid;
            int status = 1;
            
            if(info2.quantidade.Length == 0 || info2.price.Length == 0)
            {
                errorMsg = "Preencha todos os campos!!";
            }

            if(Int16.Parse(info.quantidade) >= Int16.Parse(info2.quantidade))
            {
                if (info.price != info2.price)
                {
                    //negoceia
                    status = 1;
                }
                else
                {
                    //compra direto
                    status = 0;
                }
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
                            command.Parameters.AddWithValue("@idComprador", idComprador);
                            command.Parameters.AddWithValue("@quantidade", quantidade);
                            command.Parameters.AddWithValue("@preco", preco);
                            command.Parameters.AddWithValue("@status", status);

                            command.ExecuteNonQuery();
                        }

                        String SQL2 = "UPDATE Produtos " +
                                "SET quantidade=@quantidade" +
                                "WHERE id=@id";

                        using (SqlCommand command2 = new SqlCommand(SQL2, connection))
                        {
                            command2.Parameters.AddWithValue("@quantidade", Int16.Parse(info.quantidade)-Int16.Parse(quantidade));
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
            }
            else
            {
                errorMsg = "Não existe tanta quantidade em stock!!";            }
            }

    }

    public class ProductInfo
    {
        public String id;
        public String nome;
        public String quantidade;
        public String price;
        public String imagelink;
        public String standId;
        public String idVendedor;
        public String descri;
    }

}