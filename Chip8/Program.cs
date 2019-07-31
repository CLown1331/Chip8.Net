using System;
using Emulator;

namespace Chip8
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            Chip8Emulator emulator = new Chip8Emulator();

            emulator.LoadRom("C:\\Users\\User\\source\\repos\\Chip8.Net\\pong2.c8");

            emulator.Run();
        }
    }
}
