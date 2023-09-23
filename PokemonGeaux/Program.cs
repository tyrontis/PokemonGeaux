using Newtonsoft.Json;
using System.Media;
using PokemonGeaux.Models;

namespace PokemonGeaux
{
    class Program
    {
        static async Task Main(string[] args)
        {
            ShowTitle();

            while (true)
            {
                Console.WriteLine("\nPlease enter a Pokemon's Name or ID. Type 'exit' to quit.");
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
                        var response = await client.GetAsync(apiUrl);

                        if (response.IsSuccessStatusCode)
                        {
                            string content = await response.Content.ReadAsStringAsync();
                            var thisPokemon = JsonConvert.DeserializeObject<PokemonPayload>(content);

                            Console.WriteLine($"\nPokemon Name: {char.ToUpper(thisPokemon.Name[0]) + thisPokemon.Name.Substring(1)}");
                            Console.WriteLine($"Pokemon ID: {thisPokemon.Id}");
                            Console.WriteLine("\nTypes:");
                            foreach (var type in thisPokemon.Types)
                            {
                                Console.WriteLine($"- {char.ToUpper(type.Type.Name[0]) + type.Type.Name.Substring(1)}");
                            }

                            // Type
                            foreach (var type in thisPokemon.Types)
                            {
                                string typeUrl = $"https://pokeapi.co/api/v2/type/{type.Type.Name.ToLower()}";
                                var typeResponse = await client.GetAsync(typeUrl);

                                if (typeResponse.IsSuccessStatusCode)
                                {
                                    string typeContent = await typeResponse.Content.ReadAsStringAsync();
                                    var typeData = JsonConvert.DeserializeObject<PokemonPayload>(typeContent);

                                    // Strengths
                                    Console.WriteLine("\nStrengths:");
                                    foreach (var strength in typeData.DamageRelations.DoubleDamageTo)
                                    {
                                        string strengthName = strength.Name.ToString();
                                        Console.WriteLine($"- {char.ToUpper(strengthName[0]) + strengthName.Substring(1)}");
                                    }

                                    // Weaknesses
                                    Console.WriteLine("\nWeaknesses:");
                                    foreach (var weakness in typeData.DamageRelations.DoubleDamageFrom)
                                    {
                                        string weaknessName = weakness.Name.ToString();
                                        Console.WriteLine($"- {char.ToUpper(weaknessName[0]) + weaknessName.Substring(1)}");
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

            SoundPlayer start = new SoundPlayer();
            start.SoundLocation = AppDomain.CurrentDomain.BaseDirectory + @"Resources\Polyphia8bit.wav";
            start.Play();

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
