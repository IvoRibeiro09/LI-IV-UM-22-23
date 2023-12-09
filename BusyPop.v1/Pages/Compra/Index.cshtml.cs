using BusyPop;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace BusyPop.Pages.Compra
{
    public class IndexModel : PageModel
    {
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
                            if (reader.Read())
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
            carinfo.nome = Request.Form["nome1"];
            info.quantidade = Request.Form["quanti"];
            info.price = Request.Form["preco1"];
            carinfo.idVendedor = Request.Form["Vendedor"];
            carinfo.quantidade = Request.Form["quantidade"];
            carinfo.idComprador = SessaoDeUtilizacao.Uid;
            carinfo.status = "0";

            if (carinfo.quantidade.Length == 0 || carinfo.idComprador.Length == 0)
            {
                errorMsg = "Preencha todos os campos e certifique-se que esta logado!!";
                return;
            }

            try
            {
                int i = Convert.ToInt16(info.quantidade);
                int j = Convert.ToInt16(carinfo.quantidade);
                double preco;
                double.TryParse(info.price, out preco);
                if (i >= j)
                {
                    try
                    {
                        String conn = DataBaseSession.DataBaseString;

                        using (SqlConnection connection = new SqlConnection(conn))
                        {
                            connection.Open();
                            String SQL = "INSERT INTO Carrinho " +
                                         "(nome,idVendedor,idComprador,quantidade,price,statu) VALUES " +
                                         "(@nome,@idVendedor,@idComprador,@quantidade,@price,@statu);";

                            using (SqlCommand command = new SqlCommand(SQL, connection))
                            {
                                command.Parameters.AddWithValue("@nome", carinfo.nome);
                                command.Parameters.AddWithValue("@idVendedor", carinfo.idVendedor);
                                command.Parameters.AddWithValue("@idComprador", carinfo.idComprador);
                                command.Parameters.AddWithValue("@quantidade", carinfo.quantidade);
                                command.Parameters.AddWithValue("@price", preco);
                                command.Parameters.AddWithValue("@statu", carinfo.status);

                                command.ExecuteNonQuery();
                            }

                            String SQL2 = "UPDATE Produtos " +
                                        "SET quantidade=@quantidade " +
                                        "WHERE id=@id";

                            using (SqlCommand command2 = new SqlCommand(SQL2, connection))
                            {
                                command2.Parameters.AddWithValue("@quantidade", (i - j).ToString());
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
                    Response.Redirect("/Produtos/Index");
                }
                else
                {
                    errorMsg = "Não existe tanta quantidade em stock!!";
                    return;
                }
            }
            
            catch (Exception)
            {
                errorMsg = "Não existe tanta quantidade em stock!!";
                return;
            }
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