using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace BusyPop.Pages.Carrinho
{
    public class NegociadosModel : PageModel
    {
        public List<CarrinhoInfo> CarrinhoList = new List<CarrinhoInfo>();
        public List<CarrinhoInfo> Espera = new List<CarrinhoInfo>();
        public List<CarrinhoInfo> ContraProposta = new List<CarrinhoInfo>();
        public void OnGet()
        {
            try
            {
                String conn = DataBaseSession.DataBaseString;

                using (SqlConnection connection = new SqlConnection(conn))
                {
                    connection.Open();
                    if (SessaoDeUtilizacao.Utype == "2")
                    {
                        String sql = "SELECT * FROM Carrinho WHERE idVendedor=@idVendedor";
                        using (SqlCommand command = new SqlCommand(sql, connection))
                        {   
                            
                            command.Parameters.AddWithValue("@idVendedor", SessaoDeUtilizacao.Uid);
                            using (SqlDataReader reader = command.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    CarrinhoInfo info = new CarrinhoInfo();
                                    info.id = "" + reader.GetInt32(0);
                                    info.nome = reader.GetString(1);
                                    info.idVendedor = "" + reader.GetInt32(2);
                                    info.idComprador = "" + reader.GetInt32(3);
                                    info.quantidade = "" + reader.GetInt32(4);
                                    info.preco = reader.GetDecimal(5).ToString();
                                    info.status = "" + reader.GetInt32(6);

                                    
                                    if(info.status == "1") { Espera.Add(info); }
                     
                                }
                            }
                        }
                    }
                    else if (SessaoDeUtilizacao.Utype == "3")
                    {
                        String sql = "SELECT * FROM Carrinho WHERE idComprador=@idComprador";
                        using (SqlCommand command = new SqlCommand(sql, connection))
                        {
                            command.Parameters.AddWithValue("@idComprador", SessaoDeUtilizacao.Uid);
                            using (SqlDataReader reader = command.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    CarrinhoInfo info = new CarrinhoInfo();
                                    info.id = "" + reader.GetInt32(0);
                                    info.nome = reader.GetString(1);
                                    info.idVendedor = "" + reader.GetInt32(2);
                                    info.idComprador = "" + reader.GetInt32(3);
                                    info.quantidade = "" + reader.GetInt32(4);
                                    info.preco = reader.GetDecimal(5).ToString();
                                    info.status = "" + reader.GetInt32(6);

                                    if (info.status == "0") { CarrinhoList.Add(info); }
                                    else if (info.status == "1") { Espera.Add(info); }
                                    else if (info.status == "2") { ContraProposta.Add(info); }
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
}
