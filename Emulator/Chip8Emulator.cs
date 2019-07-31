using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;

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

        private Byte x;                                                 // 4bit register
        private Byte y;                                                 // 4bit register
        private Byte n;                                                 // 4bit constant
        private Byte nn;                                                // 8bit constant
        private Byte[] memory =         new Byte[MemorySize];           // virtual machine memory
        private Byte[] V =              new Byte[16];
        private Byte[,] gfx =           new Byte[GfsRow, GfxCol];       // gfx memory
        private Byte delayTimer;
        private Byte soundTimer;
        private Byte[] key =            new Byte[KeySize];

        private Boolean drawFlag;

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
            switch (nn)
            {
                case 0xE0:
                    Console.WriteLine("clearing the screen");
                    Array.Clear(gfx, 0, gfx.Length);
                    drawFlag = true;
                    PC += 2;
                    break;
                case 0xEE:
                    Console.WriteLine("subroutine return");
                    PC = stack[--SP];
                    break;
                default:
                    throw new NotImplementedException();
                    break;
            }
        }

        void OpcodeX1()
        {
            Console.WriteLine("jumping to nnn: 0x{0:X}", nnn);
            PC = nnn;
        }

        void OpcodeX2()
        {
            Console.WriteLine("call subroutine in nnn: 0x{0:X}", nnn);
            stack[SP++] = (UInt16)(PC + 2);
            PC = nnn;
        }

        void OpcodeX3()
        {
            Console.WriteLine("skip next instruction if 0x{0:X} == 0x{1:X}\n", V[x], nn);
            PC += (UInt16)((V[x] == nn) ? 4 : 2);
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
            Console.WriteLine("set 0x{0:x} = 0x{1:x}", x, nn);
            V[x] = nn;
            PC += 2;
        }

        void OpcodeX7()
        {
            Console.WriteLine("set 0x{0:x} += 0x{1:x}", x, nn);
            V[x] += nn;
            PC += 2;
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
            I = nnn;
            PC += 2;
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
            DrawSprite(V[x], V[y], n);
            drawFlag = true;
            PC += 2;
        }

        void OpcodeXE()
        {
            throw new NotImplementedException();
        }

        void OpcodeXF()
        {
            switch (nn)
            {
                case 0x07:
                    V[x] = delayTimer;
                    PC += 2;
                    break;
                case 0x0A:
                    {
                        bool gotKeyPress = false;
                        while (!gotKeyPress)
                        {
                            for (int i = 0; i < KeySize; i++)
                            {
                                if (key[i] > 0)
                                {
                                    V[x] = (Byte)(i);
                                    gotKeyPress = true;
                                    break;
                                }
                            }
                        }
                        PC += 2;
                    }
                    break;
                case 0x15:
                    delayTimer = V[x];
                    PC += 2;
                    break;
                case 0x18:
                    soundTimer = V[x];
                    PC += 2;
                    break;
                case 0x1E:
                    V[0xF] = (Byte)((I + V[x]) > 0xFFF ? 1 : 0);
                    I += V[x];
                    PC += 2;
                    break;
                case 0x29:
                    I =(Byte)(5 * V[x]);
                    PC += 2;
                    break;
                case 0x33:
                    memory[I + 0] = (Byte)((V[x] % 1000) / 100);
                    memory[I + 1] = (Byte)((V[x] % 100) / 10);
                    memory[I + 2] = (Byte)(V[x] % 10);
                    PC += 2;
                    break;
                case 0x55:
                    for (int i = 0; i <= x; i++)
                    {
                        memory[I + i] = V[i];
                    }
                    I += (Byte)(x + 1);
                    PC += 2;
                    break;
                case 0x65:
                    for (int i = 0; i <= x; i++)
                    {
                        V[i] = memory[I + i];
                    }
                    I += (Byte)(x + 1);
                    PC += 2;
                    break;
                default:
                    throw new NotImplementedException();
                    break;
            }
        }

        private void DrawSprite(byte b, byte b1, byte b2)
        {
            var row = y;
            var col = x;

            V[0xF] = 0;

            for (var byteIndex = 0; byteIndex < n; byteIndex++)
            {

                var hand = memory[I + byteIndex];

                for (var bitIndex = 0; bitIndex < 8; bitIndex++)
                {
                    var bit = (hand >> bitIndex) & 0x1;

                    var rowIndex = (row + byteIndex) % GfsRow;

                    var colIndex = (col + (7 - bitIndex)) % GfxCol;

                    if (bit == 1 && gfx[rowIndex, colIndex] == 1)
                    {
                        V[0xF] = 1;
                    }

                    gfx[rowIndex, colIndex] ^= 1;
                }
            }
        }


        private delegate void OpCodeResolve();

        OpCodeResolve[] JumpTable;

        public Chip8Emulator()
        {
            Initialize();
        }

        private void Initialize()
        {
            PC = 0x200;
            opcode = 0;
            I = 0;
            SP = 0;

            JumpTable = new OpCodeResolve[16]{
                new OpCodeResolve(OpcodeX0), new OpCodeResolve(OpcodeX1), new OpCodeResolve(OpcodeX2), new OpCodeResolve(OpcodeX3),
                new OpCodeResolve(OpcodeX4), new OpCodeResolve(OpcodeX5), new OpCodeResolve(OpcodeX6), new OpCodeResolve(OpcodeX7),
                new OpCodeResolve(OpcodeX8), new OpCodeResolve(OpcodeX9), new OpCodeResolve(OpcodeXA), new OpCodeResolve(OpcodeXB),
                new OpCodeResolve(OpcodeXC), new OpCodeResolve(OpcodeXD), new OpCodeResolve(OpcodeXE), new OpCodeResolve(OpcodeXF)
            };

            Array.Clear(memory, 0, memory.Length);
            Array.Clear(gfx, 0, gfx.Length);
            Array.Clear(stack, 0, stack.Length);
            Array.Clear(key, 0, key.Length);

            for (int i = 0; i < 80; i++)
            {
                memory[i] = chip8Fontset[i];
            }

            delayTimer = 0;
            soundTimer = 0;
            drawFlag = true;
        }

        private void Cycle()
        {
            opcode = (UInt16)((memory[PC] << 8) | memory[PC + 1]);

            Console.WriteLine("Ox{0:X}\n", opcode);

            n = (Byte)(opcode & 0x000F);
            nn = (Byte)(opcode & 0x00FF);
            nnn = (UInt16)(opcode & 0x0FFF);
            x = (Byte)((opcode & 0x0F00) >> 8);
            y = (Byte)((opcode & 0x00F0) >> 4);

            JumpTable[(opcode & 0xF000) >> 12]();
        }

        public void LoadRom(string filePath)
        {
            using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                fs.Read(memory, 0x200, MaxGameSize);
            }
        }

        private void PrintState()
        {

            Console.WriteLine("------------------------------------------------------------------\n");

            Console.WriteLine("V0: 0x{0:X} V4: 0x{1:X} V8: 0x{2:X}  VC: 0x{3:X}", V[0], V[4], V[8], V[12]);
            Console.WriteLine("V1: 0x{0:X} V5: 0x{1:X} V9: 0x{2:X}  VD: 0x{3:X}", V[1], V[5], V[9], V[13]);
            Console.WriteLine("V2: 0x{0:X} V6: 0x{1:X} VA: 0x{2:X}  VE: 0x{3:X}", V[2], V[6], V[10], V[14]);
            Console.WriteLine("V3: 0x{0:X} V7: 0x{1:X} VB: 0x{2:X}  VF: 0x{3:X}", V[3], V[7], V[11], V[15]);

            Console.WriteLine("PC: 0x{0:X}\n", PC);
            Console.WriteLine("------------------------------------------------------------------");

        }

        public void Run()
        {
            PrintState();
            while (true)
            {
                Cycle();
                PrintState();
                if (drawFlag)
                {
                    DrawFrame();
                }
                Thread.Sleep(1000);
            }
        }

        private void DrawFrame()
        {
                Console.Clear();
                for (int x = 0; x < 32; x++)
                {
                    for (int y = 0; y < 64; y++)
                    {
                        Console.Write((gfx[x, y] == 1 ? '#' : '.'));
                    }
                    Console.Write('\n');
                }

                drawFlag = false;
        }
    }
}
