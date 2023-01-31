using RoyaleArena;
using System;

namespace TestingPlayground
{
    public class Program
    {
        public static void Main(string[] args)
        {
            IArena RA = new Arena();
            BattleCard cd1 = new BattleCard(5, CardType.SPELL, "joro", 10, 5);
            BattleCard cd2 = new BattleCard(6, CardType.SPELL, "joro", 10, 5);
            BattleCard cd3 = new BattleCard(7, CardType.SPELL, "joro", 10, 5);

            RA.Add(cd1);
            RA.Add(cd2);
            RA.Add(cd3);
            RA.RemoveById(5);

            var card = RA.GetById(5);

            Console.WriteLine(card.Id);
        }
    }
}
