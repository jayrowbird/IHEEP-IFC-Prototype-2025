using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;

namespace JsonTreeView
{
	public static class JsonTreeViewLoader
	{
		public static void LoadJsonToTreeView(this TreeView treeView, string json)
		{
			if (string.IsNullOrWhiteSpace(json))
			{
				return;
			}

			var @object = JObject.Parse(json);
			AddObjectNodes(@object, "JSON", treeView.Nodes);
		}

		public static void AddObjectNodes(JObject @object, string name, TreeNodeCollection parent)
		{
			var node = new TreeNode(name);
            node.NodeFont = new Font("Arial", 10, FontStyle.Bold);

			// if this is a child label set the back ground color
			if(name.Contains("[") == true) node.BackColor = Color.Aqua;

            parent.Add(node);

			foreach (var property in @object.Properties())
			{
				AddTokenNodes(property.Value, property.Name, node.Nodes);
			}
		}

		private static void AddArrayNodes(JArray array, string name, TreeNodeCollection parent)
		{
			var node = new TreeNode(name);
			node.NodeFont = new Font("Arial", 10,FontStyle.Bold);
            parent.Add(node);

			for (var i = 0; i < array.Count; i++)
			{
				AddTokenNodes(array[i], string.Format("[{0}] {1}", i, array[i] ["ifcName"]), node.Nodes);
			}
		}

		private static void AddTokenNodes(JToken token, string name, TreeNodeCollection parent)
		{
			if (token is JValue)
			{
				TreeNode tnode = new TreeNode(string.Format("{0}: {1}", name, ((JValue)token).Value));
				if(name.ToLower().Contains("propertysets") == true)
				{
                    tnode.BackColor = Color.LightGreen;
                }
                if (name.ToLower().Contains("quantitysets") == true)
                {
                    tnode.BackColor = Color.Yellow;
                }
                parent.Add(tnode);
			}
			else if (token is JArray)
			{
				AddArrayNodes((JArray)token, name, parent);
			}
			else if (token is JObject)
			{
				AddObjectNodes((JObject)token, name, parent);
			}
		}
	}
}