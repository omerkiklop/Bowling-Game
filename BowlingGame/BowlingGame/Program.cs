using System;
using BusinessComponents.BusinessComponents;
using BusinessComponents.Factory;
using BusinessComponents.Mappers;
using BusinessEntityServices.Entities;
using Log;
using Console = System.Console;

namespace BowlingGame
{
    public static class Program
    {
        private static void Main()
        {
            ConsoleKeyInfo key;
            do
            {
                var score = PlayGame();

                Console.WriteLine("Press \"N\" to play again, or any other key to exit.");

                key = Console.ReadKey();

            } while (key.Key == ConsoleKey.N);
        }

        private static int PlayGame()
        {
            Console.WriteLine("Lets Play Bowling");

            Console.WriteLine();

            Logger.Info("PlayGame :: Start to Play");

            string line;

            var players = new Players();

            var scoreEntity = new ScoreEntity();

            var calculator = new BowlingGameCalculator(scoreEntity);

            var messageEntity = new MessagesEntity();

            var messageFactory = new MessageFactory(messageEntity);

            var gameRules = new BowlingGameRules(messageFactory, new ConfigurationEntity());

            var playerFactory = new PlayerFactory(new PlayerEntity(), gameRules,new FrameEntity(),new FrameMapper()); 

            var gameEntity = new GameEntity();

            Console.WriteLine("Enter Players Names Following Enter Key and Then Enter Start ");

            Console.WriteLine("To Exit Press 'Exit'");

            Console.WriteLine("To Get Top 5 Scores Enter TOP");

            var game = new BusinessComponents.BusinessComponents.BowlingGame(players, calculator, gameRules, gameEntity, messageFactory, playerFactory);

            game.WriteExternal -= GameOnWriteExternal;

            game.WriteExternal += GameOnWriteExternal;

            while ((line = Console.ReadLine()) != string.Format("start").ToLower())
            {
                var name = line;

                if (string.IsNullOrWhiteSpace(name))
                {
                    Console.WriteLine("Name Is Empty, Please fill in a Correct Name");
                    continue;
                }

                if (name.ToLower().Equals("exit"))
                    break;

                if (name.ToUpper().Equals("TOP"))
                    break;

                Logger.Info("PlayGame :: CreatePlayer");

                game.CreatePlayer(name);
            }

            Logger.Info("PlayGame :: Done Adding Players");

            if (line.ToLower().Equals("exit"))
                return 0;

            if (line.ToUpper().Equals("TOP"))
            {
                game.GetTop5Scores();
                return 0;
            }

            Logger.Info("PlayGame :: BowlingGame Created and start playing ");

            while (!game.IsGameOver())
            {
                game.PlayGame();
            }

            Logger.Info("PlayGame :: Game Over ");

            Console.WriteLine("Game Over");

            Console.WriteLine();

            return 0;
        }

        private static void MessageEntityOnWriteExternal(string obj)
        {
           GameOnWriteExternal(obj);
        }

        private static void GameOnWriteExternal(string obj)
        {
            Console.WriteLine(obj);

            Console.WriteLine();
        }
    }
}