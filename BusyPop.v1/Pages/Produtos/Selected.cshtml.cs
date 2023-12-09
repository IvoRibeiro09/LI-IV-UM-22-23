using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace BusyPop.Pages.Produtos
{
    public class SelectedModel : PageModel
    { 
    public List<ProductInfo> ProductList = new List<ProductInfo>();
    String nome;
    public List<String> lista = new List<string>();

    public void OnGet()
    {
        String id = Request.Query["id"];
        nome = Request.Query["name"];
        
        
        try
        {

            String conn = DataBaseSession.DataBaseString;

            using (SqlConnection connection = new SqlConnection(conn))
            {
                connection.Open();
                
                String sql = "SELECT * FROM Stand WHERE feiraId=@feiraId";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("feiraId", id);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                String StandId = "" + reader.GetInt32(0);
                                lista.Add(StandId);
                            }
                        }
                    }

                    foreach(String StandId in lista) 
                    {
                        sql = "SELECT * FROM Produtos WHERE standId=@standId";

                        using (SqlCommand command = new SqlCommand(sql, connection))
                        {
                            command.Parameters.AddWithValue("standId", StandId);
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
        public String id;
        public String nome;
        public String quantidade;
        public String price;
        public String imagelink;
        public String standId;
        public String descri;
    }
}
