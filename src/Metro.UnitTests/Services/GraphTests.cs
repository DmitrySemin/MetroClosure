using System.Linq;
using Metro.DAL;
using Xunit;

namespace Metro.UnitTests.Services
{
    public class GraphTests
    {
        private Graph _graph;

		public GraphTests()
        {
			_graph = new Graph();
		}

        [Theory]
        [InlineData("Node1", "Node2")]
        public void Addedge(string nodeFrom, string nodeTo)
        {
			_graph.AddEdge(nodeFrom, nodeTo, 1);

	        var existNodes = _graph.Nodes.Select(node => node.Name);
	        Assert.Contains( nodeFrom, existNodes);
	        Assert.Contains( nodeTo, existNodes);
        }
    }
}
