using System;
using System.Collections.Generic;
using System.Linq;
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
        public class PlayerNotFoundException : Exception
        {
            public PlayerNotFoundException() : base("Nincs ilyen nevű játékos/monster")
            {

            }
        }

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

        public static bool IsValidNumber(string damageStr)
        {
            int damage = 0;
            try
            {
                damage = int.Parse(damageStr);
            }
            catch (FormatException)
            {
                Console.WriteLine("Számot adj meg damage-nek!");
                return false;
            }
            if (damage < 0)
            {
                Console.WriteLine("Pozitív számot adj meg!");
                return false;
            } else {
                return true;    
            }
        }

        public class Entity
        {
            public static void Attack(List<Entity> entities, int attackerIndex)
            {
                bool isValid = false;
                int damage = 0;
                string isAttacked = "";

                while (!isValid)
                {
                    for (int i = 0; i < entities.Count; i++)
                    {
                        Console.WriteLine(entities[i].Name);
                    }
                    Console.WriteLine("Kit támadsz?");
                    isAttacked = Console.ReadLine();
                    isValid = IsValidTarget(entities, isAttacked);
                }

                isValid = false;
                while (!isValid)
                {
                    Console.WriteLine("Mennyit sebzel?");
                    string damageStr = Console.ReadLine();
                    if (IsValidNumber(damageStr))
                    {
                        damage = int.Parse(damageStr);
                        isValid = true;
                    }
                }

                for (int i = 0; i < entities.Count; i++)
                {
                    if (isAttacked == entities[i].Name)
                    {
                        entities[i].TempHealth -= damage;
                        Console.WriteLine($"{entities[attackerIndex].Name} megtámadta {entities[i].Name}-t");
                        break;
                    }
                }

            }            
            public static void Heal (List<Entity> entities, int healerIndex)
            {
                bool isValid = false;
                int damage = 0;
                string isAttacked = "";

                while (!isValid)
                {
                    for (int i = 0; i < entities.Count; i++)
                    {
                        Console.WriteLine(entities[i].Name);
                    }
                    Console.WriteLine("Kit healelsz?");
                    isAttacked = Console.ReadLine();
                    isValid = IsValidTarget(entities, isAttacked);
                }

                isValid = false;
                while (!isValid)
                {
                    Console.WriteLine("Mennyit healelsz?");
                    string damageStr = Console.ReadLine();
                    if (IsValidNumber(damageStr))
                    {
                        damage = int.Parse(damageStr);
                        isValid = true;
                    }
                }

                for (int i = 0; i < entities.Count; i++)
                {
                    if (isAttacked == entities[i].Name)
                    {
                        entities[i].TempHealth += damage;
                        if (entities[i].TempHealth > entities[i].Health)
                        {
                            entities[i].TempHealth = entities[i].Health;
                        }
                        
                        Console.WriteLine($"{entities[healerIndex].Name} healelte {entities[i].Name}-t");
                        break;
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

                Console.Clear();
                Console.WriteLine($"{i + 1}. játékos neve?");
                string name = Console.ReadLine();

                int health = 0;
                int initiative = 0;
                int tempHealth = 0;

                bool isValid = false;
                while (!isValid)
                {
                    Console.WriteLine($"Mennyi {name} HP-ja?");
                    string healthStr = Console.ReadLine();
                    if (IsValidNumber(healthStr))
                    {
                        health = int.Parse(healthStr);
                        tempHealth = health;
                        isValid = true;
                    }
                }
                isValid = false;
                while (!isValid)
                {
                    Console.WriteLine($"Mennyi {name} initiative-je?");
                    string initiativeStr = Console.ReadLine();
                    if (IsValidNumber(initiativeStr))
                    {
                        initiative = int.Parse(initiativeStr);
                        isValid = true;
                    }
                }

                players.Add(new Player { Name = name, Health = health, TempHealth = tempHealth, Initiative = initiative });
                Console.Clear(); 
            }
            return players;
        }

        public static List<Monster> InitialiseMonster (int monsterCount)
        {
            List<Monster> monsters = new List<Monster>();

            for (int i = 0; i < monsterCount; i++)
            {
                Console.Clear();
                Console.WriteLine($"{i + 1}. monster neve?");
                string name = Console.ReadLine();

                int health = 0;
                int initiative = 0;
                int tempHealth = 0;

                bool isValid = false;
                while (!isValid)
                {
                    Console.WriteLine($"Mennyi {name} HP-ja?");
                    string healthStr = Console.ReadLine();
                    if (IsValidNumber(healthStr))
                    {
                        health = int.Parse(healthStr);
                        tempHealth = health;
                        isValid = true;
                    }
                }
                isValid = false;
                while (!isValid)
                {
                    Console.WriteLine($"Mennyi {name} initiative-je?");
                    string initiativeStr = Console.ReadLine();
                    if (IsValidNumber(initiativeStr))
                    {
                        initiative = int.Parse(initiativeStr);
                        isValid = true;
                    }
                }

                Console.Clear();
                monsters.Add(new Monster { Name = name, Health = health, TempHealth = tempHealth, Initiative = initiative });
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

            bool isValid = false;
            while (!isValid)
            {
                Console.WriteLine("Mennyi játekos?");
                string playerCountStr = Console.ReadLine();
                if (IsValidNumber(playerCountStr))
                {
                    playerCount  = int.Parse(playerCountStr);
                    isValid = true;
                }
            }

            players = InitialisePlayers(playerCount);

            int monsterCount = 0;
            List<Monster> monsters = new List<Monster>();

            isValid = false;
            while (!isValid)
            {
                Console.WriteLine("Mennyi monster?");
                string monsterCountStr = Console.ReadLine();
                if (IsValidNumber(monsterCountStr))
                {
                    monsterCount = int.Parse(monsterCountStr);
                    isValid = true;
                }
                
            }
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
                Console.WriteLine();
                int  roundCount = 1;
                Console.WriteLine($"{roundCount}. kör");
                for (int i = 0; i < monsters.Count; i++)
                {
                    encounterActive = false;
                    if (monsters[i].TempHealth > 0)
                    {
                        encounterActive = true;
                        break;
                    }
                }
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

            }
            Console.Clear ();
            Console.WriteLine("Nyertetek");
        }
    }
}
