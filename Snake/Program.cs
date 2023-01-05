﻿using Snake;
using SnakeGame;

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
                        Console.SetCursorPosition(0, newMap.GetLength(0) + 1);
                        Console.WriteLine($"Текущий счет: {score}");
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
    }
}