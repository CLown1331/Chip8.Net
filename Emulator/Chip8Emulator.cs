using System;
using System.Collections.Generic;
using System.Text;

namespace Emulator
{
    public class Chip8Emulator
    {
        const int               StackSize = 16;
        const int               MemorySize = 4096;
        const int               GfsRow = 32;
        const int               GfxCol = 64;
        const int               KeySize = 16;
        const int               MaxGameSize = 0xE00;

        private UInt16          opcode;                                 // opcode, 16bit
        private UInt16          I;                                      // 16bit register (For memory address)
        private UInt16          PC;                                     // program counter
        private UInt16[]        stack = new UInt16[StackSize];          // stack
        private UInt16          SP;                                     // stack pointer
        private UInt16          nnn;                                    // address

        Byte                    x;                                      // 4bit register
        Byte                    y;                                      // 4bit register
        Byte                    n;                                      // 4bit constant
        Byte                    nn;                                     // 8bit constant
        Byte[] memory =         new Byte[MemorySize];                   // virtual machine memory
        Byte[] V =              new Byte[16];
        Byte[,] gfx =           new Byte[GfsRow, GfxCol];               // gfx memory
        Byte                    delayTimer;
        Byte                    soundTimer;
        Byte[] key =            new Byte[KeySize];                  

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
        
        void OpcodeX0()
        {
            throw new NotImplementedException();
        }

        void OpcodeX1()
        {
            throw new NotImplementedException();
        }

        void OpcodeX2()
        {
            throw new NotImplementedException();
        }

        void OpcodeX3()
        {
            throw new NotImplementedException();
        }

        void OpcodeX4()
        {
            throw new NotImplementedException();
        }

        void OpcodeX5()
        {
            throw new NotImplementedException();
        }

        void OpcodeX6()
        {
            throw new NotImplementedException();
        }

        void OpcodeX7()
        {
            throw new NotImplementedException();
        }

        void OpcodeX8()
        {
            throw new NotImplementedException();
        }

        void OpcodeX9()
        {
            throw new NotImplementedException();
        }

        void OpcodeXA()
        {
            throw new NotImplementedException();
        }

        void OpcodeXB()
        {
            throw new NotImplementedException();
        }

        void OpcodeXC()
        {
            throw new NotImplementedException();
        }

        void OpcodeXD()
        {
            throw new NotImplementedException();
        }

        void OpcodeXE()
        {
            throw new NotImplementedException();
        }

        void OpcodeXF()
        {
            throw new NotImplementedException();
        }

        private delegate void OpCodeResolve();

        OpCodeResolve[] JumpTable;

        public Chip8Emulator()
        {
            PC          = 0x200;
            opcode      = 0;
            I           = 0;
            SP          = 0;

            JumpTable = new OpCodeResolve[16]{
                new OpCodeResolve(OpcodeX0), new OpCodeResolve(OpcodeX1), new OpCodeResolve(OpcodeX2), new OpCodeResolve(OpcodeX3),
                new OpCodeResolve(OpcodeX4), new OpCodeResolve(OpcodeX5), new OpCodeResolve(OpcodeX6), new OpCodeResolve(OpcodeX7),
                new OpCodeResolve(OpcodeX8), new OpCodeResolve(OpcodeX9), new OpCodeResolve(OpcodeXA), new OpCodeResolve(OpcodeXB),
                new OpCodeResolve(OpcodeXC), new OpCodeResolve(OpcodeXD), new OpCodeResolve(OpcodeXE), new OpCodeResolve(OpcodeXF)
            };
        }
    }
}
