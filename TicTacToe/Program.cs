using System;

namespace TicTacToe
{
    public static class Program
    {
        static void Main(string[] args)
        {
            TicTacToe.Run(Console.Write, Console.ReadLine, Console.Clear);

            Console.ReadKey();
        }
    }
}