using System;
using System.Windows.Forms;
using WinFormsConsoleReadonly;


namespace HeepWare.Windows.Forms.IFC.Prototype
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
             Application.Run(new IheepIFCPrototypeForm());
           //Application.Run(new Form1());
        }
    }
}