using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Threading;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;

namespace PBPemU
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        byte[] rom = File.ReadAllBytes(@"C:\Users\tvirtman.SVA\Downloads\PBPU\examples\mathThing.asm.bin");
        byte[] ram = new byte[256];
        byte X = 0;
        uint Xmath = 0;
        byte Y = 0;
        uint Ymath = 0;
        byte Z = 0;
        uint Zmath = 0;
        byte Loc1 = 0;
        byte Loc2 = 0;
        byte PC1 = 0;
        byte PC2 = 0;
        byte Carry = 0;
        bool useCarry = false;
        int romLocGlobal = 0;
        string currentCode = "";
        int clockSpeed = 200;

        public int makeLoc(byte high, byte low)
        {
            return high * 16 + low;
        }

        public byte getInstruction(byte input)
        {
            return Convert.ToByte(input >> 4);
        }

        public byte getNumber(byte input)
        {
            return Convert.ToByte(input & 15);
        }

        public void UpdateUI()
        {
            this.Dispatcher.BeginInvoke(new Action(() =>
            {
                currentInstruction.Text = "Instruction: " + currentCode;
                PCreg.Text = "PC: " + makeLoc(PC2, PC1).ToString();
                PCregActual.Text = "PC2: " + romLocGlobal.ToString();
                LocReg.Text = "Loc: " + makeLoc(Loc1, Loc2).ToString();
                Xreg.Text = "X: " + X.ToString();
                Yreg.Text = "Y: " + Y.ToString();
                Zreg.Text = "Z: " + Z.ToString();
                ramUI.Text = "RAM:";
                for (int i = 0; i < ram.Length; i++)
                {
                    if (i % 32 == 0)
                    {
                        ramUI.Text += "\n";
                    }
                    ramUI.Text += ram[i].ToString("X") + " ";
                }

                romUI.Text = "ROM:";
                for (int i = 0; i < rom.Length; i++)
                {
                    if (i % 32 == 0)
                    {
                        romUI.Text += "\n";
                    }
                    if (i == romLocGlobal) {
                        romUI.Text += rom[i].ToString("X") + "<";
                    } else {
                        romUI.Text += rom[i].ToString("X") + " ";
                    }
                }
            }));
        }

        public MainWindow()
        {
            InitializeComponent();
        }

        private void runProgram(object sender, RoutedEventArgs e)
        {
            mainCode();
        }

        public void mainCode()
        {
            new Thread(() =>
            {
                Thread.CurrentThread.IsBackground = true;
                /* run your code here */
                for (int romLoc = 0; romLoc < rom.Length; romLoc++)
                {
                    romLocGlobal = romLoc;
                    byte readByte = rom[romLoc];
                    // Instruction decoding and executing
                    switch (getInstruction(readByte))
                    {
                        case 0:
                            // NOP
                            currentCode = "";
                            break;
                        case 1:
                            // ADD
                            Xmath = X;
                            Ymath = Y;
                            // Carry Bit logic
                            if (useCarry)
                            {
                                Zmath = Xmath + Ymath + Carry;
                            }
                            else
                            {
                                Zmath = Xmath + Ymath;
                            }
                            Z = (byte)Zmath;
                            if (Z > 15)
                            {
                                Z -= 15;
                                Carry = 1;
                            }
                            else
                            {
                                Carry = 0;
                            }
                            currentCode = "ADD";
                            break;
                        case 2:
                            // SUB
                            Xmath = X;
                            Ymath = Y;
                            // Borrow Bit logic
                            if (useCarry)
                            {
                                Zmath = Xmath - Ymath - Carry;
                            }
                            else
                            {
                                Zmath = Xmath - Ymath;
                            }
                            Z = (byte)Zmath;
                            if (Z < 0)
                            {
                                Z += 15;
                                Carry = 1;
                            }
                            else
                            {
                                Carry = 0;
                            }
                            currentCode = "SUB";
                            break;
                        case 3:
                            // WT1
                            Loc1 = getNumber(readByte);
                            currentCode = "WT1 " + Loc1;
                            break;
                        case 4:
                            // WT2
                            Loc2 = getNumber(readByte);
                            currentCode = "WT2 " + Loc2;
                            break;
                        case 5:
                            // WTX
                            X = getNumber(readByte);
                            currentCode = "WTX " + X;
                            break;
                        case 6:
                            // WTY
                            Y = getNumber(readByte);
                            currentCode = "WTY " + Y;
                            break;
                        case 7:
                            // WTZ
                            Z = getNumber(readByte);
                            currentCode = "WTZ " + Z;
                            break;
                        case 8:
                            // ZTR
                            ram[makeLoc(Loc1, Loc2)] = Z;
                            currentCode = "ZTR";
                            break;
                        case 9:
                            // RTZ
                            Z = ram[makeLoc(Loc1, Loc2)];
                            currentCode = "RTZ";
                            break;
                        case 10:
                            // PC1
                            PC1 = getNumber(readByte);
                            currentCode = "PC1 " + PC1;
                            break;
                        case 11:
                            // PC2
                            PC2 = getNumber(readByte);
                            currentCode = "PC1 " + PC2;
                            break;
                        case 12:
                            // JMP
                            romLoc = makeLoc(PC2, PC1)-1;
                            currentCode = "JMP";
                            break;
                        case 13:
                            // RTX
                            X = ram[makeLoc(Loc1, Loc2)];
                            currentCode = "RTX";
                            break;
                        case 14:
                            // RTY
                            Y = ram[makeLoc(Loc1, Loc2)];
                            currentCode = "RTY";
                            break;
                        case 15:
                            // USC
                            if (useCarry == true)
                            {
                                useCarry = false;
                            }
                            else
                            {
                                useCarry = true;
                            }
                            currentCode = "USC";
                            break;
                    }
                    UpdateUI();
                    Thread.Sleep(clockSpeed);
                }
            }).Start();
        }

        private void updateClock(object sender, DependencyPropertyChangedEventArgs e)
        {
            clockSpeed = Convert.ToInt32(clockSpeedSetting.Value);
        }
    }
}
