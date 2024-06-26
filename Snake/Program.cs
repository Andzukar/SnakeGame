﻿using SnakeGame.Controllers;
using SnakeGame.Controllers.ControlTypes;
using SnakeGame.Controllers.Interfaces;
using System.Diagnostics;

namespace SnakeGame;

/// <summary>
/// Main program.
/// </summary>
internal class Program
{
    /// <summary>
    /// Point of entry.
    /// </summary>
    /// <param name="args">Arguments</param>
    static async Task Main(string[] args)
    {
        while (true)
        {
            Console.CursorVisible = false;

            const int minMapValue = 10;
            const int maxMapValue = 25;

            var snake = new Snake();
            var applaManager = new AppleManager();
            var watch = new Stopwatch();

            const int maxGameSpeed = 300;
            var currentGameSpeed = 5;
            var gameSpeedIterator = 5;
            var startGameSpeed = currentGameSpeed;
            var score = 1;

            snake.GameOver += (string message) =>
            {
                watch.Stop();
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"You lose: {message}");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine();
                Console.WriteLine($"Total score: {score}");
                Console.WriteLine($"Total time: {watch.Elapsed:mm\\:ss\\.ff}");
                Console.WriteLine("Max game speed: ");
                Console.WriteLine();
            };

            Console.Write($"Please enter map size (min value = {minMapValue}, max value = {maxMapValue}): ");

            var mapSize = Console.ReadLine();

            if (int.TryParse(mapSize, out var size) && size >= minMapValue && size <= maxMapValue)
            {
                var map = new Map(size);

                snake.ScoreUp += () =>
                {
                    score++;

                    applaManager.CreateApple(map);

                    currentGameSpeed += gameSpeedIterator;

                    if (currentGameSpeed > maxGameSpeed)
                    {
                        currentGameSpeed = maxGameSpeed;
                    }
                };

                var controller = ChooseControllType(map);

                var (x, y) = (size / 2, size / 2);
                var snakeHeadPosition = new Position(x, y);

                snake.AddPart(snakeHeadPosition);

                applaManager.CreateApple(map);

                watch.Start();

                Console.Clear();
                
                while (true)
                {
                    snake.SnakeHead.PreviousPosition = snakeHeadPosition;

                    controller.MakeStep(ref snakeHeadPosition);

                    if (!snake.Move(snakeHeadPosition, map))
                    {
                        break;
                    }

                    map.UpdateMap();

                    var uiGameSpeed = Math.Round((double)currentGameSpeed / maxGameSpeed * 100, 2);
                    ShowUI(map, score, uiGameSpeed, watch);
                    await Task.Delay(maxGameSpeed - currentGameSpeed);
                }
            }
            else
            {
                Console.Clear();
                Console.WriteLine("You entered invalid data, map size must be an integer!");
            }
        }
    }

    /// <summary>
    /// Show UI.
    /// </summary>
    /// <param name="map">Game map.</param>
    /// <param name="score">Game score.</param>
    /// <param name="speed">Game speed.</param>
    /// <param name="gameTime">Game time.</param>
    private static void ShowUI(Map map, int score, double speed, Stopwatch gameTime)
    {
        Console.SetCursorPosition(0, map.SizeY + 1);
        Console.WriteLine($"Current score: {score}");
        Console.WriteLine($"Current time: {gameTime.Elapsed:mm\\:ss\\.ff}");
        Console.WriteLine($"Current game speed: {speed} %");
    }

    /// <summary>
    /// Choose controll type.
    /// </summary>
    /// <param name="map">Game map.</param>
    /// <returns></returns>
    private static IMove ChooseControllType(Map map)
    {
        Console.WriteLine("Select control type: 1 - manual, 2 - auto");

        var input = Console.ReadLine();

        if (!Enum.TryParse<ControlType>(input, out var controlType))
        {
            return ChooseControllType(map);
        }

        return controlType switch
        {
            ControlType.Manual => new ManualController(),
            ControlType.Auto => new AutoController(map),
            _ => ChooseControllType(map),
        };
    }
}
