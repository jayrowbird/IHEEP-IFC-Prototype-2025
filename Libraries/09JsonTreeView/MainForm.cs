using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace JsonTreeView
{
	public partial class MainForm : Form
	{
		[STAThread]
		static void Main()
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new MainForm());
		}

		public MainForm()
		{
			InitializeComponent();
 
			string jsonString = File.ReadAllText("TestData\\ExampleIFCJson.json");
			JsonTreeView.LoadJsonToTreeView(jsonString);

			Process process = new Process();

            nint appWin;
            try
            {
                // Start the process 
                process.StartInfo.FileName = "C:\\Windows\\System32\\excel.exe";
                //process.StartInfo.WindowStyle = ProcessWindowStyle.Normal;
                process.Start();

                // Wait for process to be created and enter idle condition 
                process.WaitForInputIdle();

                // Get the main handle
                appWin = process.MainWindowHandle;

                // Put it into this form
                ApplicationControl.SetParent(appWin, this.Handle);

                // Remove border and whatnot
                ApplicationControl.SetWindowLong(appWin, ApplicationControl.GWL_STYLE, ApplicationControl.WS_VISIBLE);

                // Move the window to overlay it on this window
                ApplicationControl.MoveWindow(appWin, 0, 0, this.Width, this.Height, true);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Error");
            }


            if (false)
			{
				JsonTreeView.LoadJsonToTreeView(@"{
	'id': '0001',
	'type': 'donut',
	'name': 'Cake',
	'ppu': 0.55,
	'batters':
		{
			'batter':
				[
					{ 'id': '1001', 'type': 'Regular' },
					{ 'id': '1002', 'type': 'Chocolate' },
					{ 'id': '1003', 'type': 'Blueberry' },
					{ 'id': '1004', 'type': 'Devil\'s Food' }
				]
		},
	'topping':
		[
			{ 'id': '5001', 'type': 'None' },
			{ 'id': '5002', 'type': 'Glazed' },
			{ 'id': '5005', 'type': 'Sugar' },
			{ 'id': '5007', 'type': 'Powdered Sugar' },
			{ 'id': '5006', 'type': 'Chocolate with Sprinkles' },
			{ 'id': '5003', 'type': 'Chocolate' },
			{ 'id': '5004', 'type': 'Maple' }
		]
}");
			}
			JsonTreeView.ExpandAll();
		}


	}
}