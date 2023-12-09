using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace BusyPop.Pages.Utilizador
{
    public class IndexModel : PageModel
    {

        public List<UtilizadoresInfo> list = new List<UtilizadoresInfo>();

        public void OnGet()
        {
            try
            {

                String conn = DataBaseSession.DataBaseString;

                using (SqlConnection connection = new SqlConnection(conn))
                {
                    connection.Open();
                    String sql = "SELECT * FROM Utilizadores";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                UtilizadoresInfo info = new UtilizadoresInfo();
                                info.id = "" + reader.GetInt32(0);
                                info.nome = reader.GetString(1);
                                info.email = reader.GetString(2);
                                info.pass = reader.GetString(3);
                                info.tipo = "" + reader.GetInt32(4);

                                list.Add(info);
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

    public class UtilizadoresInfo
    {
        public String? id;
        public String? nome;
        public String? email;
        public String? pass;
        public String? tipo;
    }
}
