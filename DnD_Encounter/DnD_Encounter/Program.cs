using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;
using static DnD_Encounter.Program;

namespace DnD_Encounter
{
    internal class Program
    {
        public static bool IsValidTarget(List<Entity> entities, string isAttacked)
        {
            
            for (int i = 0; i < entities.Count; i++)
            {
                if (isAttacked == entities[i].Name)
                {
                    return true;
                }
            }
            Console.WriteLine("Nincs ilyen nevű játékos/monster");
            return false;
        }
        public static void ClearConsole()
        {
            Console.Clear();
            Console.WriteLine("DnD Encounter");
        }
        public static string ValidTarget(List<Entity> entities)
        {
            bool isValid = false;
            string isAttacked = "";

            while (!isValid)
            {
                isAttacked = Console.ReadLine();
                for (int i = 0; i < entities.Count; i++)
                {
                    if (isAttacked == entities[i].Name)
                    {
                        isValid = true;
                    }
                }
                if (!isValid)
                {
                    Console.WriteLine("Nincs ilyen nevű játékos/monster");
                }
            }
            return isAttacked;
        }
        public static int ParseNumber()
        {
            bool isValid = false;
            int input = 0;

            while (!isValid)
            {
                try
                {
                    input = int.Parse(Console.ReadLine());
                    if (input < 0)
                    {
                        Console.WriteLine("Pozitív számot adj meg!");
                    } 
                    else
                    {
                        isValid = true;
                    }
                }
                catch (FormatException)
                {
                    Console.WriteLine("Számot adj meg!");
                }
            }
            return input; 
        }
        public class Entity
        {
            public static void AttackType(List<Entity> entities, int attackerIndex)
            {
                Console.WriteLine("Milyen attack?");
                Console.WriteLine("1: Single target");
                Console.WriteLine("2: Multiattack");
                //Console.WriteLine("3: AOE");

                int attackType = ParseNumber();

                switch (attackType)
                {
                    case 1:
                        Entity.SingleAttack(entities, attackerIndex);
                        break;
                    case 2:
                        Entity.MultiAttack(entities, attackerIndex);
                        break;
                }
                ClearConsole();
            }
            public static void SingleAttack(List<Entity> entities, int attackerIndex)
            {
                Console.WriteLine();
                Console.WriteLine("Célpontok:");
                for (int i = 0; i < entities.Count; i++)
                {
                    if (entities[i].CurrentHealth > 0)
                    {
                        Console.WriteLine(entities[i].Name);
                    }
                }
                Console.WriteLine();

                Console.WriteLine($"Kit támadsz, {entities[attackerIndex].Name}?");
                string isAttacked = ValidTarget(entities);

                Console.WriteLine("Mennyit sebzel?");
                int damage = ParseNumber();

                for (int i = 0; i < entities.Count; i++)
                {
                    if (isAttacked == entities[i].Name)
                    {
                        entities[i].CurrentHealth -= damage;
                        Console.WriteLine($"{entities[attackerIndex].Name} megtámadta {entities[i].Name}-t");
                        Console.WriteLine();
                        break;
                    }
                }

            }
            public static void MultiAttack (List<Entity> entities, int attackerIndex)
            {
                ClearConsole();
                Console.WriteLine($"Hányszor támadsz, {entities[attackerIndex].Name}?");
                int attackTimes = ParseNumber();
                for (int i = 0; i < attackTimes; i++)
                {
                    Console.WriteLine($"{i + 1}. támadás:");
                    Entity.SingleAttack(entities, attackerIndex);
                }
            }
            public static void Heal (List<Entity> entities, int healerIndex)
            {
                Console.WriteLine();
                Console.WriteLine("Célpontok:");
                for (int i = 0; i < entities.Count; i++)
                {
                    Console.WriteLine(entities[i].Name);
                }

                Console.WriteLine($"Kit healelsz, {entities[healerIndex].Name}?");
                string isHealed = ValidTarget(entities);

                Console.WriteLine("Mennyit healelsz?");
                int heal = ParseNumber();

                for (int i = 0; i < entities.Count; i++)
                {
                    if (isHealed == entities[i].Name)
                    {
                        entities[i].CurrentHealth += heal;
                        if (entities[i].CurrentHealth > entities[i].MaxHealth)
                        {
                            entities[i].CurrentHealth = entities[i].MaxHealth;
                        }
                        
                        Console.WriteLine($"{entities[healerIndex].Name} healelte {entities[i].Name}-t");
                        break;
                    }
                }
            }
            public string Name;
            public int MaxHealth;
            public int CurrentHealth;
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
                ClearConsole();
                Console.WriteLine($"{i + 1}. játékos neve?");
                string name = Console.ReadLine();
                  
                Console.WriteLine($"Mennyi {name} HP-ja?");
                int maxHealth = ParseNumber();
                int currentHealth = maxHealth;

                Console.WriteLine($"Mennyi {name} initiative-je?");
                int initiative = ParseNumber();

                players.Add(new Player { Name = name, MaxHealth = maxHealth, CurrentHealth = currentHealth, Initiative = initiative });
                ClearConsole(); 
            }
            return players;
        }
        public static List<Monster> InitialiseMonster (int monsterCount)
        {
            List<Monster> monsters = new List<Monster>();

            for (int i = 0; i < monsterCount; i++)
            {
                ClearConsole();
                Console.WriteLine($"{i + 1}. monster neve?");
                string name = Console.ReadLine();

                Console.WriteLine($"Mennyi {name} HP-ja?");
                int maxHealth = ParseNumber();
                int currentHealth = maxHealth;

                Console.WriteLine($"Mennyi {name} initiative-je?");
                int initiative = ParseNumber();

                ClearConsole();
                monsters.Add(new Monster { Name = name, MaxHealth = maxHealth, CurrentHealth = currentHealth, Initiative = initiative });
            }
            return monsters;
        }
        public static void DisplayPlayers(int playerCount, List<Player> players)
        {
            for (int i = 0; i < playerCount; i++)
            {
                Console.WriteLine($"Játokes: {i+1}");
                Console.WriteLine(players[i].Name);
                Console.WriteLine(players[i].CurrentHealth);
                Console.WriteLine(players[i].Initiative);
                Console.WriteLine();
            }
        }
        public static void DisplayMonsters(int monsterCount, List<Monster> monsters)
        {
            for (int i = 0; i < monsterCount; i++)
            {
   
                Console.WriteLine($"Monster: {i+1}");
                Console.WriteLine(monsters[i].Name);
                Console.WriteLine(monsters[i].CurrentHealth);
                Console.WriteLine(monsters[i].Initiative);
                Console.WriteLine();
            }
        }
        public static void DisplayInitiativeOrder(List<Entity> entities)
        {
            Console.WriteLine("Initiative sorrend:");
            for (int i = 0; i < entities.Count; i++)
            {
                Console.WriteLine(entities[i].Name);
            }
            Console.WriteLine();
        }
        static void Main(string[] args)
        {
            Console.WriteLine("DnD Encounter");
            
            int playerCount = 0;
            List<Player> players = new List<Player>();
            Console.WriteLine("Mennyi játekos?");
            playerCount = ParseNumber();
            players = InitialisePlayers(playerCount);

            int monsterCount = 0;
            List<Monster> monsters = new List<Monster>();
            Console.WriteLine("Mennyi monster?");
            monsterCount = ParseNumber();
            monsters = InitialiseMonster(monsterCount);

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
                int  roundCount = 1;
                Console.WriteLine($"{roundCount}. kör");
                
                for (int i = 0; i < entities.Count; i++)
                {
                    for (int j = 0; j < monsters.Count; j++)
                    {
                        encounterActive = false;
                        if (monsters[j].CurrentHealth > 0)
                        {
                            encounterActive = true;
                            break;
                        }
                    }
                    if (entities[i].CurrentHealth < 0)
                    {
                        continue;
                    }
                    Console.WriteLine($"Mit szeretnél csinálni, {entities[i].Name}?");
                    Console.WriteLine("1 : attack");
                    Console.WriteLine("2 : heal");
                    //Console.WriteLine("3 : buff");
                    //Console.WriteLine("4 : deboff");
                    int input = int.Parse(Console.ReadLine());
                    ClearConsole();

                    switch (input)
                    {
                        case 1:
                            Entity.AttackType(entities, i);
                            break;
                        case 2: 
                            Entity.Heal(entities, i);
                            break;

                    }
                    DisplayPlayers(playerCount, players);
                    DisplayMonsters(monsterCount, monsters);
                }
                roundCount++;
            }
            ClearConsole ();
            DisplayPlayers(playerCount, players);
            DisplayMonsters(monsterCount, monsters);
            Console.WriteLine("Nyertetek");
        }
    }
}
