using Snake;
using SnakeGame;
using System.Diagnostics;
using static System.Formats.Asn1.AsnWriter;

namespace SnakeGame
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.CursorVisible = false;
            var apple = new Apple();
            var map = new Map();
            var snake = new Snake(apple);
            var score = 1;
            var watch = new Stopwatch();

            snake.ScoreUp += () =>
            {
                score++;
            };

            while (true)
            {
                Console.Write("Пожалуйста, введите размер карты: ");
                var mapSize = Console.ReadLine();
                if (int.TryParse(mapSize, out var size))
                {
                    ConsoleKey? key = null;
                    Task.Run(() =>
                    {
                        while (true)
                        {
                            key = Console.ReadKey().Key;
                        }
                    });

                    Console.BackgroundColor = ConsoleColor.DarkYellow;
                    var newMap = map.GenerateMap(size);
                    watch.Start();
                    var (x, y) = (size / 2, size / 2);
                    snake.AddNewSnakePart(x, y);
                    apple.CreateApple(newMap);
                    while (true)
                    {
                        Console.Clear();
                        snake.MakeStep(ref x, ref y, key);
                        if (!snake.CheckPositionAndConfirmStep(x, y, newMap))
                        {
                            break;
                        }
                        map.DrawMap(newMap);
                        ShowUI(newMap, score, watch);
                        Thread.Sleep(200);
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
            Console.WriteLine($"Current time: {watch.Elapsed.ToString("mm\\:ss\\.ff")}");

        }
    }
}