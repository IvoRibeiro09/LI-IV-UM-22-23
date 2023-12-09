using BusyPop.Pages.Utilizador;
using Microsoft.Extensions.FileSystemGlobbing.Internal.PathSegments;

namespace BusyPop
{
	public static class SessaoDeUtilizacao
    {
        private static string Username = "";
        public static string Uname
        {
            get { return Username; }
            set { Username = value; }
        }
        private static string Type = "";
        public static string Utype
        {
            get { return Type; }
            set { Type = value; }
        }
        private static string Id = "";
        public static string Uid
        {
            get { return Id; }
            set { Id = value; }
        }
    }

}
