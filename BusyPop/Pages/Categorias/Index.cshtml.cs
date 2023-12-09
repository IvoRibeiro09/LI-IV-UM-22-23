using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace BusyPop.Pages.Categorias
{
    public class IndexModel : PageModel
    {

        public List<CategoryInfo> list = new List<CategoryInfo>();

        public void OnGet()
        {
            try
            {

                String conn = DataBaseSession.DataBaseString;

                using(SqlConnection connection = new SqlConnection(conn))
                {
                    connection.Open();
                    String sql = "SELECT * FROM Category";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader =command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                CategoryInfo info = new CategoryInfo();
                                info.id = "" + reader.GetInt32(0);
                                info.category = reader.GetString(1);
                                info.capacity = "" + reader.GetInt32(2);
                                info.description = reader.GetString(3);

                                list.Add(info);
                            }
                        }                    
                    }
                }
            }
            catch(Exception e)
            {
                Console.WriteLine("Exception: " + e.ToString());
            }
        }

    }

    public class CategoryInfo
    {
        public String? id;
        public String? category;
        public String? capacity;
        public String? description;
    }
}
