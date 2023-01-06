using Microsoft.VisualBasic;
using System;
namespace SnakeGame.Interface
{
    internal interface IMove
    {
        void MakeStep(ref int x, ref int y);
    }
}
