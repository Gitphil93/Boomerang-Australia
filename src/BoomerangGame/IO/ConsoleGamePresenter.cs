using System;
using System.Collections.Generic;
using System.Linq;
using Boomerang.Domain;
using Boomerang.Game;

namespace Boomerang.IO
{
    public class ConsoleGamePresenter : IGamePresenter
    {

        public void ShowGameStart(int playerCount)
        {
            Console.WriteLine("Starting Boomerang: Australia!");
            Console.WriteLine($"Players: {playerCount}");
            Console.WriteLine();
        }

        public void ShowRoundStart(int roundIndex)
        {
            Console.WriteLine();
            Console.WriteLine($"----- ROUND {roundIndex} -----");
        }

        public void ShowFinalResults(List<Player> players, List<Player> winners)
        {
            Console.WriteLine("\n===== FINAL SCORES =====");

            foreach (var p in players)
            {
                var s = p.ScoreSheet;
                Console.WriteLine(
                    $"{p.Name}: Total={s.TotalScore} " +
                    $"(Throw/Catch={s.ThrowCatchScore}, Regions={s.RegionScore}, Collections={s.CollectionScore}, Animals={s.AnimalScore}, Activities={s.ActivityScore})"
                );
            }

            Console.WriteLine();

            if (winners.Count == 1)
                Console.WriteLine($"Winner: {winners[0].Name}!");
            else
                Console.WriteLine("Tie between: " + string.Join(", ", winners.Select(w => w.Name)));
        }
    }
}
