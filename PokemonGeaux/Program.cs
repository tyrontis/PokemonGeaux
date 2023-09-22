using Newtonsoft.Json.Linq;
using System.Media;

namespace PokemonGeaux
{
    class Program
    {
        static async Task Main(string[] args)
        {
            ShowTitle();

            while (true)
            {
                Console.WriteLine("Please enter a Pokemon's Name or ID. Type 'exit' to quit.");
                string pokemonName = Console.ReadLine();

                if (pokemonName.ToLower() == "exit")
                {
                    break;
                }

                if (string.IsNullOrWhiteSpace(pokemonName))
                {
                    Console.WriteLine($"Please enter a valid Pokemon name.");
                    continue;
                }

                try
                {
                    using (HttpClient client = new HttpClient())
                    {
                        string apiUrl = $"https://pokeapi.co/api/v2/pokemon/{pokemonName.ToLower()}";
                        HttpResponseMessage response = await client.GetAsync(apiUrl);

                        if (response.IsSuccessStatusCode)
                        {
                            string content = await response.Content.ReadAsStringAsync();
                            JObject pokemonData = JObject.Parse(content);

                            JArray types = pokemonData["types"] as JArray;
                            if (types != null)
                            {
                                foreach (var type in types)
                                {
                                    string typeName = type["type"]["name"].ToString();
                                    Console.WriteLine($"\nType: {char.ToUpper(typeName[0]) + typeName.Substring(1)}");

                                    // Type
                                    string typeUrl = $"https://pokeapi.co/api/v2/type/{typeName}";
                                    HttpResponseMessage typeResponse = await client.GetAsync(typeUrl);

                                    if (typeResponse.IsSuccessStatusCode)
                                    {
                                        string typeContent = await typeResponse.Content.ReadAsStringAsync();
                                        JObject typeData = JObject.Parse(typeContent);

                                        // Strength/Weakness
                                        var damageRelations = typeData["damage_relations"];
                                        Console.WriteLine($"\nStrengths:");

                                        foreach (var strength in damageRelations["double_damage_to"])
                                        {
                                            string strengthName = strength["name"].ToString();
                                            Console.WriteLine($"- {char.ToUpper(strengthName[0]) + strengthName.Substring(1)}");
                                        }

                                        Console.WriteLine($"\nWeaknesses:");
                                        foreach (var weakness in damageRelations["double_damage_from"])
                                        {
                                            string weaknessName = weakness["name"].ToString();
                                            Console.WriteLine($"- {char.ToUpper(weaknessName[0]) + weaknessName.Substring(1)}");
                                        }

                                        Console.WriteLine("");
                                    }
                                }
                            }
                        }
                        else
                        {
                            Console.WriteLine("\nPokemon not found. Please enter a valid Pokemon name. Let's try again!");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred: {ex.Message}");
                }
            }
        }

        static void ShowTitle()
        {
            Console.Title = "PokemonGeaux";

            Console.WriteLine("\t\t\t                                  ,'\\");
            Console.WriteLine("\t\t\t    _.----.        ____         ,'  _\\   ___    ___     ____");
            Console.WriteLine("\t\t\t_,-'       `.     |    |  /`.   \\,-'    |   \\  /   |   |    \\  |`.");
            Console.WriteLine("\t\t\t\\      __    \\    '-.  | /   `.  ___    |    \\/    |   '-.   \\ |  |");
            Console.WriteLine("\t\t\t \\.    \\ \\   |  __  |  |/    ,','_  `.  |          | __  |    \\|  |");
            Console.WriteLine("\t\t\t   \\    \\/   /,' _`.|      ,' / / / /   |          ,' _`.|     |  |");
            Console.WriteLine("\t\t\t    \\     ,-'/  /   \\    ,'   | \\/ / ,`.|         /  /   \\  |     |");
            Console.WriteLine("\t\t\t     \\    \\ |   \\_/  |   `-.  \\    `'  /|  |    ||   \\_/  | |\\    |");
            Console.WriteLine("\t\t\t      \\    \\ \\      /       `-.`.___,-' |  |\\  /| \\      /  | |   |");
            Console.WriteLine("\t\t\t       \\    \\ `.__,'|  |`-._    `|      |__| \\/ |  `.__,'|  | |   |");
            Console.WriteLine("\t\t\t        \\_.-'       |__|    `-._ |              '-.|     '-.| |   |");
            Console.WriteLine("\t\t\t                                `'                            '-._|");
            Console.WriteLine("\n\n");
        }
    }
}
