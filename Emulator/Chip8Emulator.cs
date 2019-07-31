using System;
using System.Collections.Generic;
using System.Text;

namespace Emulator
{
    public class Chip8Emulator
    {
        const int StackSize = 16;
        const int MemorySize = 4096;
        const int GfsRow = 32;
        const int GfxCol = 64;
        const int KeySize = 16;
        const int MaxGameSize = 0xE00;

        private UInt16 opcode;                              // opcode, 16bit
        private UInt16 I;                                   // 16bit register (For memory address)
        private UInt16 PC;                                  // program counter
        private UInt16[] stack = new UInt16[StackSize];     // stack
        private UInt16 SP;                                  // stack pointer
        private UInt16 nnn;                                 // address

        Byte x;
        Byte y;
        Byte n;
        Byte nn;
        Byte[] memory = new Byte[MemorySize];
        Byte[] V = new Byte[16];
        Byte[,] gfx = new Byte[GfsRow, GfxCol];
        Byte delayTimer;
        Byte soundTimer;
        Byte[] key = new Byte[KeySize];

        private Byte[] chip8Fontset = new Byte[80] {
                0xF0, 0x90, 0x90, 0x90, 0xF0, // 0
                0x20, 0x60, 0x20, 0x20, 0x70, // 1
                0xF0, 0x10, 0xF0, 0x80, 0xF0, // 2
                0xF0, 0x10, 0xF0, 0x10, 0xF0, // 3
                0x90, 0x90, 0xF0, 0x10, 0x10, // 4
                0xF0, 0x80, 0xF0, 0x10, 0xF0, // 5
                0xF0, 0x80, 0xF0, 0x90, 0xF0, // 6
                0xF0, 0x10, 0x20, 0x40, 0x40, // 7
                0xF0, 0x90, 0xF0, 0x90, 0xF0, // 8
                0xF0, 0x90, 0xF0, 0x10, 0xF0, // 9
                0xF0, 0x90, 0xF0, 0x90, 0x90, // A
                0xE0, 0x90, 0xE0, 0x90, 0xE0, // B
                0xF0, 0x80, 0x80, 0x80, 0xF0, // C
                0xE0, 0x90, 0x90, 0x90, 0xE0, // D
                0xF0, 0x80, 0xF0, 0x80, 0xF0, // E
                0xF0, 0x80, 0xF0, 0x80, 0x80  // F
        };

        public Chip8Emulator()
        {

        }
    }
}
