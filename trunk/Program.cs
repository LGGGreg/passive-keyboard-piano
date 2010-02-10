/*
Copyright (c) 2009, Gregory Hendrickson (LordGregGreg Back)
All rights reserved.

Redistribution and use in source and binary forms, with or without modification, are permitted provided that the following conditions are met:

    * Redistributions of source code must retain the above copyright notice, this list of conditions and the following disclaimer.
    * Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following disclaimer in the documentation and/or other materials provided with the distribution.
    * Neither the name of the  Gregory Hendrickson nor the names of its contributors may be used to endorse or promote products derived from this software without specific prior written permission.

THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
*/

using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Diagnostics;
using System.Threading;
using System.Runtime.InteropServices;

namespace KeyboardPiano
{
    public class GWorker
    {
        private int freq;
        private int dur;
        private int volume;
        private Random r;
        private int chanel;
        private Form1 theForm;
        public GWorker(int ifreq,int idur,int invol,Form1 inform)
        {
            freq = ifreq;
            dur = idur;
            theForm = inform;
            volume = invol;
            chanel = 0;// r.Next(0, 10);
        }
        // This method will be called when the thread is started.
        public void DoWork()
        {
            
            theForm.sendMsg(true, freq,volume,chanel);
            System.Threading.Thread.Sleep(TimeSpan.FromSeconds(dur));
            theForm.sendMsg(false, freq,volume,chanel);
            
        }
    }
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        /// 
        private const int WH_KEYBOARD_LL = 13;
        private const int WM_KEYDOWN = 0x0100;
        private static LowLevelKeyboardProc _proc = HookCallback;
        private static IntPtr _hookID = IntPtr.Zero;
        private static Form1 myForm;
        private static Random random;
        private static Keys lastK;
        [STAThread]
        static void Main()
        {
            try
            {
                _hookID = SetHook(_proc);
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                myForm = new Form1();
                random = new Random();

                Application.Run(myForm);

                UnhookWindowsHookEx(_hookID);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private static IntPtr SetHook(LowLevelKeyboardProc proc)
        {
            using (Process curProcess = Process.GetCurrentProcess())
            using (ProcessModule curModule = curProcess.MainModule)
            {
                return SetWindowsHookEx(WH_KEYBOARD_LL, proc,
                    GetModuleHandle(curModule.ModuleName), 0);
            }
        }

        private delegate IntPtr LowLevelKeyboardProc(
            int nCode, IntPtr wParam, IntPtr lParam);

        private static IntPtr HookCallback(
            int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0 && wParam == (IntPtr)WM_KEYDOWN)
            {
                int vkCode = Marshal.ReadInt32(lParam);
                Keys newKey = (Keys)vkCode;
                if (newKey != lastK)
                {
                    lastK = newKey;
                    //myForm.textBox1.AppendText(((Keys)vkCode).ToString());
                    string s = (newKey).ToString().ToLower();
                    myForm.textBox1.Text = s;
                    int duration = 1;
                    double x = 0.0;
                    if (s == "oemtilde") x = 31.5;
                    if (s == "tab") x = 51.5;
                    if (s == "d1") x = 89.5;
                    if (s == "d2") x = 138.5;
                    if (s == "d3") x = 192.5;
                    if (s == "d4") x = 247.5;
                    if (s == "d5") x = 301.5;
                    if (s == "d6") x = 354;
                    if (s == "d7") x = 407.5;
                    if (s == "d8") x = 462.5;
                    if (s == "d9") x = 517.5;
                    if (s == "d0") x = 572.5;
                    if (s == "oemminus") x = 627.5;
                    if (s == "oemplus") x = 680;
                    if (s == "back") x = 760;
                    if (s == "q") x = 112.5;
                    if (s == "w") x = 166.5;
                    if (s == "e") x = 220;
                    if (s == "r") x = 273.5;
                    if (s == "t") x = 327.5;
                    if (s == "y") x = 382;
                    if (s == "u") x = 436;
                    if (s == "i") x = 490;
                    if (s == "o") x = 544;
                    if (s == "p") x = 598;
                    if (s == "oem6") x = 706.5;
                    if (s == "oemopenbrackets") x = 652.5;
                    if (s == "oem5") x = 766;
                    if (s == "capital") x = 58.5;
                    if (s == "a") x = 140;
                    if (s == "s") x = 194;
                    if (s == "d") x = 248;
                    if (s == "f") x = 302.5;
                    if (s == "g") x = 356.5;
                    if (s == "h") x = 410;
                    if (s == "j") x = 464;
                    if (s == "k") x = 518;
                    if (s == "l") x = 572;
                    if (s == "oem1") x = 626;
                    if (s == "oem7") x = 679.5;
                    if (s == "return") x = 760.5;
                    if (s == "lshiftkey") x = 72;
                    if (s == "rshiftkey") x = 680;
                    if (s == "z") x = 167;
                    if (s == "x") x = 221;
                    if (s == "c") x = 275;
                    if (s == "v") x = 329;
                    if (s == "b") x = 383;
                    if (s == "n") x = 437;
                    if (s == "m") x = 491;
                    if (s == "oemcomma") x = 545;
                    if (s == "oemperiod") x = 600;
                    if (s == "oemquestion") x = 653;
                    if (s == "space")
                    {
                        x = 9000;
                        duration = 1;
                    }
                    x *= 1 / 766.1;
                    //127 is full
                    double range = myForm.trackBar2.Value * 127.0 / 100.0;
                    double ofset = (127 - range) / 2.0;
                    x = ofset + (x * range);

                    if (x - .1 < ofset)
                    {
                        x = (random.NextDouble()*range)+ofset;
                    }
                    myForm.textBox1.Text = s + "=>" + x.ToString();
                    

                    GWorker workerObject = new GWorker((int)x, duration,myForm.trackBar1.Value, myForm);
                    Thread workerThread = new Thread(workerObject.DoWork);
                    workerThread.Start();
                    //myForm.pianoControl1.PressPianoKey(Keys.A);//random.Next(1, 10));
                    //myForm.sendMsg(true, random.Next(1, 70));
                }
                
            }
            return CallNextHookEx(_hookID, nCode, wParam, lParam);
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook,
            LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode,
            IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string lpModuleName);
    
    }
}