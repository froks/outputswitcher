using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace OutputSwitcher
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            WinMM.switchOuputDevice();
        }
    }
}