using System.Threading.Tasks;

namespace Movies
{
    public class Program
    {
        public static async Task Main()
        {
            Search serviceSearch = new Search();
            serviceSearch.ManageConsoleKey();
        }
    }
}
