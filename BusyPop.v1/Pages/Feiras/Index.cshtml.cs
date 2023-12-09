using BusyPop.Pages.Categorias;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace BusyPop.Pages.Feiras
{
    public class IndexModel : PageModel
    {
       
        public List<FeirasInfo> FeirasList = new List<FeirasInfo>();
        public void OnGet()
        {
            try
            {
                String conn = DataBaseSession.DataBaseString;

                using (SqlConnection connection = new SqlConnection(conn))
                {
                    connection.Open();
                    String sql = "SELECT * FROM Feiras";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                FeirasInfo info = new FeirasInfo();
                                info.id = "" + reader.GetInt32(0);
                                info.nome = reader.GetString(1);
                                info.nome = reader.GetString(1);
                                info.categoryid = "" + reader.GetInt32(2);
                                info.numSlots = "" + reader.GetInt32(3);
                                info.descri = reader.GetString(4);

                                FeirasList.Add(info);
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

    public class FeirasInfo
    {
        public String id;
        public String nome;
        public String categoryid;
        public String numSlots;
        public String descri;
    }

}
