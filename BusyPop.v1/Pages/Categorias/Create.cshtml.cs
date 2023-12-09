using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace BusyPop.Pages.Categorias
{
    public class CreateModel : PageModel
    {
        public CategoryInfo info = new CategoryInfo();
        public String errorMsg = "";
        public String sucessMsg = "";

        public void OnGet()
        {
        }

        public void OnPost()
        {
            info.category = Request.Form["category"];
            info.capacity = Request.Form["capacity"];
            info.description = Request.Form["description"];

            if(info.category.Length == 0 || info.capacity.Length == 0 || info.description.Length == 0)
            {
                errorMsg = "Preencha todos os campos!!";
                return;
            }

            //inserir na base de dados
            try {
                String conn = DataBaseSession.DataBaseString;

                using (SqlConnection connection = new SqlConnection(conn))
                {
                    connection.Open();
                    String SQL = "INSERT INTO Category " +
                                 "(category,capacity,description) VALUES " +
                                 "(@category,@capacity,@description);";

                    using (SqlCommand command = new SqlCommand(SQL, connection))
                    {
                        command.Parameters.AddWithValue("@category", info.category);
                        command.Parameters.AddWithValue("@capacity", info.capacity);
                        command.Parameters.AddWithValue("@description",info.description);
                        
                        command.ExecuteNonQuery();
                    }
                }


            }
            catch(Exception e)
            {
                errorMsg = e.Message;
                return;
            }



            info.category = "";
            info.capacity = "";
            info.description = "";
            sucessMsg = "Categoria adicionada com sucesso!!";

            Response.Redirect("/Categorias/Index");
        }

    }
}
