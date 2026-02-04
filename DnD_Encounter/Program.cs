using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using static DnD_Encounter.Program;

namespace DnD_Encounter
{
    internal class Program
    {
        public class PlayerNotFoundException : Exception
        {
            public PlayerNotFoundException() : base("Nincs ilyen nevű játékos/monster")
            {

            }
        }

        public class Entity
        {
            public static void Attack (List <Entity> entities, int attackerIndex)
            {  
                bool tryAgain = true;
                while (tryAgain)
                {
                    for (int i = 0; i < entities.Count; i++)
                    {
                        Console.WriteLine(entities[i].Name);
                    }
                    try
                    {
                        Console.WriteLine("Kit támadsz?");
                        string isAttacked = Console.ReadLine();
                        Console.WriteLine("Mennyit sebzel?");
                        int damage = int.Parse(Console.ReadLine());

                        bool playerFound = false;
                        for (int i = 0; i < entities.Count; i++)
                        {
                            if (isAttacked == entities[i].Name)
                            {
                                entities[i].TempHealth -= damage;
                                Console.WriteLine($"{entities[attackerIndex].Name} megtámadta {entities[i].Name}-t");
                                playerFound = true;
                                break;
                            }                            
                        }
                        if (!playerFound)
                        {
                            throw new PlayerNotFoundException();
                        }
                        tryAgain = false;
                    } catch (FormatException)
                    {
                        Console.Clear();
                        Console.WriteLine("Számot adj meg damage-nek");

                    } catch (PlayerNotFoundException ex)
                    {
                        Console.Clear();
                        Console.WriteLine(ex.Message);
                    }
                }
                
                
            }
            public static void Heal (List<Entity> entities, int healerIndex)
            {
                bool tryAgain = true;
                while (tryAgain)
                {
                    for (int i = 0; i < entities.Count; i++)
                    {
                        Console.WriteLine(entities[i].Name);
                    }
                    try
                    {
                        Console.WriteLine("Kit healelsz?");
                        string isHealed = Console.ReadLine();
                        Console.WriteLine("Mennyit healelsz?");
                        int heal = int.Parse(Console.ReadLine());

                        bool playerFound = false;
                        for (int i = 0; i < entities.Count; i++)
                        {
                            if (isHealed == entities[i].Name)
                            {
                                entities[i].TempHealth += heal;
                                Console.WriteLine($"{entities[healerIndex].Name} healelte {entities[i].Name}-t");
                                playerFound = true;
                                break;
                            }
                        }
                        if (!playerFound)
                        {
                            throw new PlayerNotFoundException();
                        }
                        tryAgain = false;
                    }
                    catch (FormatException)
                    {
                        Console.Clear();
                        Console.WriteLine("Számot adj meg damage-nek");

                    }
                    catch (PlayerNotFoundException ex)
                    {
                        Console.Clear();
                        Console.WriteLine(ex.Message);
                    }
                }
            }
            public string Name;
            public int Health;
            public int TempHealth;
            public int Initiative;
        }
        public class Player : Entity
        {
           
        }

        public class Monster : Entity
        {
            
        }

        public static List<Player> InitialisePlayers(int playerCount)
        {
            List<Player> players = new List<Player>();
            for (int i = 0; i < playerCount; i++)
            {
                bool tryAgain = true;
                Console.WriteLine($"{i + 1}. játékos neve?");
                string name = Console.ReadLine();

                while (tryAgain)
                {
                    try
                    {
                        Console.WriteLine($"Mennyi {name} HP-ja?");
                        int health = int.Parse(Console.ReadLine());
                        int tempHealth = health;
                        Console.WriteLine($"Mennyi {name} initiative-je?");
                        int initiative = int.Parse(Console.ReadLine());

                        players.Add(new Player { Name = name, Health = health, TempHealth = tempHealth, Initiative = initiative });
                        tryAgain = false;
                        Console.Clear();
                    }
                    catch (FormatException)
                    {
                        Console.WriteLine("Számot adj meg HP/initiative-nek!");
                    }

                }
            }
            return players;
        }

        public static List<Monster> InitialiseMonster (int monsterCount)
        {
            List<Monster> monsters = new List<Monster>();

            for (int i = 0; i < monsterCount; i++)
            {
                bool tryAgain = true;
                Console.WriteLine($"{i + 1}. monster neve?");
                string name = Console.ReadLine();
                while (tryAgain)
                {
                    try
                    {
                        Console.WriteLine($"Mennyi a {name} HP-ja?");
                        int health = int.Parse(Console.ReadLine());
                        int tempHealth = health;
                        Console.WriteLine($"Mennyi a {name} initiative-je?");
                        int initiative = int.Parse(Console.ReadLine());
                        Console.Clear();
                        monsters.Add(new Monster { Name = name, Health = health, TempHealth = tempHealth, Initiative = initiative });
                        tryAgain = false;

                    }
                    catch (FormatException)
                    {
                        Console.WriteLine("Számot adj meg HP/initiative-nek!");
                    }
                }
            }
            return monsters;
        }
        public static void DisplayPlayers(int playerCount, List<Player> players)
        {
            for (int i = 0; i < playerCount; i++)
            {
                Console.WriteLine();
                Console.WriteLine($"Játokes: {i}");
                Console.WriteLine(players[i].Name);
                Console.WriteLine(players[i].TempHealth);
                Console.WriteLine(players[i].Initiative);
            }
        }

        public static void DisplayMonsters(int monsterCount, List<Monster> monsters)
        {
            for (int i = 0; i < monsterCount; i++)
            {
                Console.WriteLine();
                Console.WriteLine($"Monster: {i}");
                Console.WriteLine(monsters[i].Name);
                Console.WriteLine(monsters[i].TempHealth);
                Console.WriteLine(monsters[i].Initiative);
            }
        }

        public static void DisplayInitiativeOrder(List<Entity> entities)
        {
            Console.WriteLine();
            Console.WriteLine("Initiative sorrend");
            for (int i = 0; i < entities.Count; i++)
            {
                Console.WriteLine(entities[i].Name);
            }
        }

        static void Main(string[] args)
        {
            Console.WriteLine("DnD Encounter");
            int playerCount = 0;
            List<Player> players = new List<Player>();

            bool tryAgain = true;
            while (tryAgain)
            {
                try
                {
                    Console.WriteLine("Mennyi játékos?");
                    playerCount = int.Parse(Console.ReadLine());
                    players = InitialisePlayers(playerCount);
                    tryAgain = false;
                }
                catch (FormatException)
                {
                    Console.WriteLine("Számot adj meg!");
                }
            }

            int monsterCount = 0;
            List<Monster> monsters = new List<Monster>();

            tryAgain = true;
            while (tryAgain)
            {
                try
                {
                    Console.WriteLine("Mennyi monster?");
                    monsterCount = int.Parse(Console.ReadLine());
                    monsters = InitialiseMonster(monsterCount);
                    tryAgain = false;
                }

                catch (FormatException)
                {
                    Console.WriteLine("Számot adj meg!");
                }
            }

            DisplayPlayers(playerCount, players);
            DisplayMonsters(monsterCount, monsters);


            List<Entity> entities = new List<Entity>(players.Count + monsters.Count);
            entities.AddRange(players);
            entities.AddRange(monsters);
            entities.Sort((a, b) => b.Initiative.CompareTo(a.Initiative));
            DisplayInitiativeOrder(entities);

            bool encounterActive = true;
            while (encounterActive)
            {
                Console.WriteLine();
                int  roundCount = 1;
                Console.WriteLine($"{roundCount}. kör");
                for (int i = 0; i < entities.Count; i++)
                {
                    if (entities[i].TempHealth < 0)
                    {
                        continue;
                    }
                    Console.WriteLine($"Mit szeretnél csinálni, {entities[i].Name}?");
                    Console.WriteLine("1 : attack");
                    Console.WriteLine("2 : heal");
                    //Console.WriteLine("3 : buff");
                    //Console.WriteLine("4 : deboff");
                    int input = int.Parse(Console.ReadLine());
                    Console.Clear();

                    switch (input)
                    {
                        case 1:
                            Entity.Attack(entities, i);
                            break;
                        case 2: 
                            Entity.Heal(entities, i);
                            break;

                    }
                    DisplayPlayers(playerCount, players);
                    DisplayMonsters(monsterCount, monsters);
                }
                for (int i = 0;i < monsters.Count; i++)
                {
                    encounterActive = false;
                    if (monsters[i].TempHealth > 0)
                    {
                        encounterActive = true;
                        break;
                    }
                }

            }
            Console.Clear ();
            Console.WriteLine("Nyertetek");
        }
    }
}
