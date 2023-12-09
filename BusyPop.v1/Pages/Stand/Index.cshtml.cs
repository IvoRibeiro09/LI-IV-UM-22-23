using BusyPop.Pages.Categorias;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace BusyPop.Pages.Stand
{
    public class IndexModel : PageModel
    {
       
        public List<StandInfo> StandInfos = new List<StandInfo>();
        public void OnGet()
        {
            String vendedorId = SessaoDeUtilizacao.Uid;

			try
            {
                String conn = DataBaseSession.DataBaseString;

                using (SqlConnection connection = new SqlConnection(conn))
                {
                    connection.Open();
                    String sql = "SELECT * FROM Stand  WHERE vendedorId=@vendedorId";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
						command.Parameters.AddWithValue("@vendedorId", vendedorId);
						using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
								StandInfo info = new StandInfo();
                                info.id = "" + reader.GetInt32(0);
                                info.nome = reader.GetString(1);
                                info.vendedor = "" + reader.GetInt32(2);
                                info.feiraid = "" + reader.GetInt32(3);

								StandInfos.Add(info);
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

    public class StandInfo
	{
        public String id;
        public String nome;
        public String vendedor;
        public String feiraid;
    }

}
