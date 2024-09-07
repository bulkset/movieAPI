using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Movies;
public class Search
{
    private static int page = 1;
    private static bool search = false;
    public static int qator = 0;
    private static string requestKinoName = "";
    static string id = "";
    public const string DEF_URL = "http://www.omdbapi.com/?apikey=";
    
    private const string API_KEY = "ed7b702d";
    private string request = "";
    private static List<SearchRequest> movie;

    public string PromptOfUser()
    {
        Console.WriteLine("Kinoning nomini kiriting: ");
        string kinoName = Console.ReadLine()!;
        return $"&s={kinoName}";
    }

    public async Task ListOfMovies()
    {
        using (HttpClient client = new HttpClient())
        {
            if (!search)
                requestKinoName = PromptOfUser();

            request = $"{DEF_URL}{API_KEY}{requestKinoName}&page={page}";

            HttpResponseMessage response = await client.GetAsync(request);
            response.EnsureSuccessStatusCode();

            string responseString = await response.Content.ReadAsStringAsync();
            var listOfMoviesResponse = JsonSerializer.Deserialize<ResponseSeachClass>(responseString);

            if (listOfMoviesResponse != null && listOfMoviesResponse.Response == "True")
            {
                movie = listOfMoviesResponse.Search;
                Console.WriteLine("Kinolar:\n");
                for (int i = 0; i < movie.Count; i++)
                {
                    if (qator == i)
                        Console.WriteLine($" |>  {movie[i].Title}");
                    else
                        Console.WriteLine($"   {movie[i].Title}");
                    
                    if (qator < movie.Count) 
                        id = movie[qator].imdbID;
                }

                int totalResults = int.Parse(listOfMoviesResponse.totalResults);
                int totalPages = (totalResults + 9) / 10; 

                string pageDisplay = (page + 1 <= totalPages)
                    ? $"\nbetlar: 1.. <{page - 1} <<( {page} )>> {page + 1}> ..{totalPages}  umumiy qidiruv natijalari: {listOfMoviesResponse.totalResults}"
                    : $"\nbetlar: 1.. <{page - 1} <<( {page} )>> {totalPages}> ..{totalPages}  umumiy qidiruv natijalari: {listOfMoviesResponse.totalResults}";

                if (page == 1)
                    Console.WriteLine($"\nbetlar: <<( {page} )>> {page + 1}> ..{totalPages}  umumiy qidiruv natijalari: {listOfMoviesResponse.totalResults}");
                else if (page == 2)
                    Console.WriteLine($"\nbetlar: <{page - 1} << ( {page} )>> {page + 1}> ..{totalPages}  umumiy qidiruv natijalari: {listOfMoviesResponse.totalResults}");
                else
                    Console.WriteLine(pageDisplay);

                Console.WriteLine("\nIltimos, qidirish uchun `Backspace` tugmasini bosing.");
            }
            else
            {
                Console.WriteLine("Xato 404: Topilmadi");
                Console.WriteLine("Iltimos, qayta urinib ko'rish uchun `Backspace` tugmasini bosing.");
            }
        }
    }
    public async Task MovieInfo(){
        Console.Clear();

        HttpClient client2 = new HttpClient();
        string url2 = $"http://www.omdbapi.com/?i={id}&plot=full&apikey=ed7b702d";
        var response2 = await client2.GetAsync(url2);
        

        var client2String = response2.Content.ReadAsStringAsync().Result;

        var aboutMovie = JsonSerializer.Deserialize<Root>(client2String);
 
        Console.WriteLine($"About '{aboutMovie.Title}' film");

        Console.WriteLine($"\n  Created in: {aboutMovie.Year} year ");
        Console.WriteLine($"\n  Released: {aboutMovie.Released} ");
        Console.WriteLine($"\n  Film duration: {aboutMovie.Released} ");
        Console.WriteLine($"\n  Genre: {aboutMovie.Genre} ");
        Console.WriteLine($"\n  Film director: {aboutMovie.Director} ");
        Console.WriteLine($"\n  Director: {aboutMovie.Director} ");
        Console.WriteLine($"\n  Writer: {aboutMovie.Writer} ");
        Console.WriteLine($"\n  Syujet: {aboutMovie.Plot} ");
        Console.WriteLine($"\n  Language: {aboutMovie.Language} ");
        Console.WriteLine($"\n  Country: {aboutMovie.Country} ");
        Console.WriteLine($"\n  Awards: {aboutMovie.Awards} ");

        Console.WriteLine("\nYou can press the 'Backspace'");

        ConsoleKey key = Console.ReadKey().Key;
        if (ConsoleKey.Backspace == key)
        {
            ListOfMovies();
        }


    }

    public void ManageConsoleKey()
    {
        while (true)
        {
            Console.Clear();
            ListOfMovies();
            search = true;
            ConsoleKey key = Console.ReadKey().Key;
            if(ConsoleKey.Backspace == key)
            {
                search = false;
                page = 1;
            }else if(ConsoleKey.RightArrow == key)
            {
                page++;
            }else if (ConsoleKey.LeftArrow == key && page!=1)
            {
                page--;
            }else if (ConsoleKey.Escape == key)
            {
                Program.Main();
            }else if (ConsoleKey.UpArrow == key && qator!=1)
            {
                qator--;
            }
            else if (ConsoleKey.DownArrow == key && qator != 10)
            {
                qator++;
            }
            else if (ConsoleKey.Enter == key)
            {
                MovieInfo();
            }
        }
    }
}

public class ResponseSeachClass
{
    public List<SearchRequest> Search { get; set; }
    public string totalResults { get; set; }
    public string Response { get; set; }
}

public class SearchRequest
{
    public string Title { get; set; }
    public string Year { get; set; }
    public string imdbID { get; set; }
    public string Type { get; set; }
    public string Poster { get; set; }
}
