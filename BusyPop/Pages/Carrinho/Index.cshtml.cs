using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace BusyPop.Pages.Carrinho
{
    public class IndexModel : PageModel
    {

        public List<CarrinhoInfo> CarrinhoList = new List<CarrinhoInfo>();

        public void OnGet()
        {
            try
            {

                String conn = DataBaseSession.DataBaseString;

                using (SqlConnection connection = new SqlConnection(conn))
                {
                    connection.Open();
                    String sql = "SELECT * FROM Produtos";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                CarrinhoInfo info = new CarrinhoInfo();
                                info.id = "" + reader.GetInt32(0);
                                info.nome = reader.GetString(1);
                                info.idVendedor = "" + reader.GetInt32(2);
                                info.idComprador = reader.GetDecimal(3).ToString();
                                info.quantidade = reader.GetString(4);
                                info.preco = "" + reader.GetInt32(5);
                                info.status = reader.GetString(6);

                                CarrinhoList.Add(info);
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
