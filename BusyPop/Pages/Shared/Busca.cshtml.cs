using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace BusyPop.Pages.Shared
{
    public class BuscaModel : PageModel
    {
        public List<BuscaInfo> BuscaList = new List<BuscaInfo>();
        public void OnGet()
        {
            String name = Request.Query["fname"];
            try
            {
                String conn = DataBaseSession.DataBaseString;

                using (SqlConnection connection = new SqlConnection(conn))
                {
                    connection.Open();
                    string sql = "SELECT * FROM Category WHERE category=@cat";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@cat", name);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                BuscaInfo info = new BuscaInfo();
                                info.id = "" + reader.GetInt32(0);
                                info.name = reader.GetString(1);
                                info.tipo = 1;

                                BuscaList.Add(info);
                            }
                        }
                    }
                    string sql2 = "SELECT * FROM Feiras WHERE nome=@name";
                    using (SqlCommand command = new SqlCommand(sql2, connection))
                    {
                        command.Parameters.AddWithValue("@name", name);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                BuscaInfo info = new BuscaInfo();
                                info.id = "" + reader.GetInt32(0);
                                info.name = reader.GetString(1);
                                info.tipo = 0;

                                BuscaList.Add(info);
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
    public class BuscaInfo
    {
        public String id;
        public String name;
        public int tipo;
    }
}
