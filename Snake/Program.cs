using Snake;
using SnakeGame.Controller;
using SnakeGame.Interface;
using System.Diagnostics;

namespace SnakeGame
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.CursorVisible = false;
            var apple = new Apple();
            var snake = new Snake(apple);
            var score = 1;
            var watch = new Stopwatch();

            snake.ScoreUp += () =>
            {
                score++;
            };

            snake.GameOver += (string message) =>
            {
                Console.Clear();
                Console.WriteLine("You lose!");
                Console.WriteLine(message);
                Console.WriteLine($"Total score: {score}");
                Console.WriteLine($"Total time: {watch.Elapsed:mm\\:ss\\.ff}");
            };

            while (true)
            {
                Console.Write("Пожалуйста, введите размер карты: ");
                var mapSize = Console.ReadLine();
                if (int.TryParse(mapSize, out var size))
                {
                    Console.BackgroundColor = ConsoleColor.DarkYellow;
                    var newMap = Map.GenerateMap(size);
                    watch.Start();
                    var (x, y) = (size / 2, size / 2);
                    snake.AddNewSnakePart(x, y);
                    apple.CreateApple(newMap);
                    var controller = ChooseControllType();
                    while (true)
                    {
                        Console.Clear();
                        snake.SnakeHead.PreviousPosition = new Position(x, y);
                        controller.MakeStep(ref x, ref y);

                        if (!snake.CheckPositionAndConfirmStep(x, y, newMap))
                        {
                            break;
                        }
                        Map.GetCurrentMap[y, x] = Constant.SnakeDesignation;
                        Map.DrawMap(newMap);
                        ShowUI(newMap, score, watch);
                        Thread.Sleep(1);
                    }
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("Вы ввели недопустимые данные, размер карты должен быть целочисленным числом");
                }
            }
        }


        private static void ShowUI(char[,] map, int score, Stopwatch watch)
        {
            Console.SetCursorPosition(0, map.GetLength(0) + 1);
            Console.WriteLine($"Current score: {score}");
            Console.WriteLine($"Current time: {watch.Elapsed:mm\\:ss\\.ff}");
        }

        private static IMove ChooseControllType()
        {
            Console.WriteLine("Выберите тип управления: 1 - ручной, 2 - авто");
            return Console.ReadLine() switch
            {
                "1" => new ManualController(),
                "2" => new AStartController(),
                _ => throw new NotImplementedException(),
            };
        }
    }
}