// Ignore Spelling: Json

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
        private static int count = 0;
        private void button1_Click(object sender, EventArgs e)
        {
            string searchTag = "1BSF9OxKHFwP7UyiF4HCJP";

            if(count == 1)
            {
                searchTag = "1BSF9OxKHFwP7UyiF4HCH_";
            }
            else if(count == 2)
            {
                searchTag = "3ClK5vGz14svFocFB_YJ36";
            }
            else if (count == 3)
            {
                searchTag = "0ItlUmZhT6j94x5csoQ7tl";
                count = -1;
            }

            TreeNode resultNode = null;
            foreach (TreeNode rootNode in JsonTreeView.Nodes)
            {
                resultNode = FindNodeByTag(searchTag, rootNode.Nodes);
                if (resultNode != null)
                {
                    break; // Node found, exit the loop
                }
            }

            if (resultNode != null)
            {
                JsonTreeView.SelectedNode = resultNode;
            }
            else
            {
                MessageBox.Show("Node not found in tree", "did not work");
            }
            count++;
















            //////// Example: Searching for a node with a Product object in its Tag and matching Id
            //////TreeNode foundNode = JsonTreeView.Nodes.Cast<TreeNode>()
            //////                         .Where(x => ((x.Tag as string) != null) && (x.Tag as string).ToString() == searchTag)
            //////                         .FirstOrDefault();
            //////if (foundNode != null)
            //////{
            //////    JsonTreeView.SelectedNode = foundNode;
            //////}

            //////var result = JsonTreeView.Nodes.OfType<TreeNode>()
            //////         .FirstOrDefault(node => node.Tag != null && node.Tag.Equals(searchTag));
            //////if (result != null)
            //////{
            //////    JsonTreeView.SelectedNode = foundNode;
            //////}
            ////////JsonTreeView.SelectedNode = JsonTreeView.Nodes.Find("2_hD6cVhHDQAGohyvv6sEg", true).FirstOrDefault();
        }



        public TreeNode FindNodeByTag(object tag, TreeNodeCollection nodes)
        {
            foreach (TreeNode node in nodes)
            {
                if (node.Tag != null && node.Tag.Equals(tag))
                {
                    return node;
                }

                // Recursively search child nodes
                TreeNode foundNode = FindNodeByTag(tag, node.Nodes);
                if (foundNode != null)
                {
                    return foundNode;
                }
            }

            return null; // Node not found
        }
    }
}