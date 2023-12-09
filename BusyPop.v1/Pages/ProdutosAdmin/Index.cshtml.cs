using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;

namespace BusyPop.Pages.ProdutosAdmin
{
    public class IndexModel : PageModel
    {

        public List<ProductInfo> ProductList = new List<ProductInfo>();

        public void OnGet()
        { 

            try
            {

                string conn = DataBaseSession.DataBaseString;

                using (SqlConnection connection = new SqlConnection(conn))
                {
                    connection.Open();
                    if (SessaoDeUtilizacao.Utype == "1")
                    {
                        String sql = "SELECT * FROM Produtos";
                        using (SqlCommand command = new SqlCommand(sql, connection))
                        {
                            using (SqlDataReader reader = command.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    ProductInfo info = new ProductInfo();
                                    info.id = "" + reader.GetInt32(0);
                                    info.nome = reader.GetString(1);
                                    info.quantidade = "" + reader.GetInt32(2);
                                    info.price = reader.GetDecimal(3).ToString();
                                    info.imagelink = reader.GetString(4);
                                    info.standId = "" + reader.GetInt32(5);
                                    info.descri = reader.GetString(7);

                                    ProductList.Add(info);
                                }
                            }
                        }
                    }
                    else
                    {
                        String sql = "SELECT * FROM Produtos WHERE idVendedor=@idVendedor";
                        using (SqlCommand command = new SqlCommand(sql, connection))
                        {
                            command.Parameters.AddWithValue("idVendedor", SessaoDeUtilizacao.Uid);
                            using (SqlDataReader reader = command.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    ProductInfo info = new ProductInfo();
                                    info.id = "" + reader.GetInt32(0);
                                    info.nome = reader.GetString(1);
                                    info.quantidade = "" + reader.GetInt32(2);
                                    info.price = reader.GetDecimal(3).ToString();
                                    info.imagelink = reader.GetString(4);
                                    info.standId = "" + reader.GetInt32(5);
                                    info.descri = reader.GetString(7);

                                    ProductList.Add(info);
                                }
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

    }

    public class ProductInfo
    {
        public String? id;
        public String? nome;
        public String? quantidade;
        public String? price;
        public String? imagelink;
        public String? standId;
        public Stream? idVendedor;
        public String? descri;
    }

}
