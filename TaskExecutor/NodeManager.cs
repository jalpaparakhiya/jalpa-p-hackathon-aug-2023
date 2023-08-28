using System.Xml.Linq;
using TaskExecutor.Models;

namespace TaskExecutor
{
	public class NodeManager
	{
		private List<Node> _nodes = new List<Node>();

		public void RegisterNode(Node node)
		{
			var existNode = _nodes.FirstOrDefault(n => n.Name == node.Name);
			if(existNode == null) {
				_nodes.Add(node);
			}
		}

		public async Task UnregisterNode(string name)
		{
			var node = _nodes.FirstOrDefault(n => n.Name == name);
			if (node != null)
				_nodes.Remove(node);
		}

		public Node GetAvailableNode()
		{
			return _nodes.FirstOrDefault(n => n.Status == NodeStatus.Available);
		}
	}
}
