using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Metro.DAL
{
	/// <summary>
	/// Объект, инкапсулирующий все связанное с графом
	/// </summary>
    public class Graph
    {
		#region private fields
		private List<GraphNode> _nodes;
		private List<GraphNode> _sortedNodes;
		private List<GraphNode> _tempNodes;
		private List<GraphNode> _ap;
		private List<int> _marked;
		private Dictionary<int, int> _enter;
		private Dictionary<int, int> _low;
		private Dictionary<int, int> _parent;
		private int _time;
		#endregion

		#region public fields

		public List<GraphNode> Nodes
        {
            get { return _nodes; }
        }

		public List<GraphNode> SortedNodes
		{
			get
			{
				if (!_sortedNodes.Any())
				{
					_sortedNodes = SortNodes();
				}

				return _sortedNodes;
			}
		}

		#endregion

		#region .ctors

		public Graph()
		{
			_nodes = new List<GraphNode>();
			_sortedNodes = new List<GraphNode>();
		}
		#endregion

		#region public methods

		/// <summary>
		/// Добавление ребра графа
		/// </summary>
		/// <param name="from">Вершина - источник</param>
		/// <param name="to">Вершина - назначение</param>
		/// <param name="group">Группа вершин</param>
		public void AddEdge(string from, string to, int group)
        {
            var fromNode = _nodes.FirstOrDefault(n => string.Compare(n.Name,@from) == 0) ??
                           AddNode(@from, @group);

			var toNode = _nodes.FirstOrDefault(n => string.Compare(n.Name, to) == 0) ??
			             AddNode(to, @group);

			AddEdge(fromNode, toNode);
        }

		/// <summary>
		/// Добавление ребра графа
		/// </summary>
		/// <param name="from">Вершина - источник</param>
		/// <param name="to">Вершина - назначение</param>
		public void AddEdge(GraphNode from, GraphNode to)
        {
            from.Edges.Add(to);
            to.Edges.Add(from);
        }

		#endregion

		#region private methods

		/// <summary>
		/// Добавление вершины графа
		/// </summary>
		/// <param name="name">Наименование вершины</param>
		/// <param name="group">Группа</param>
		private GraphNode AddNode(string name, int group)
		{
			var node = new GraphNode
			{
				Name = name,
				Id = _nodes.Count,
				Group = group
			};

			_nodes.Add(node);
			return node;
		}

		/// <summary>
		/// Удаление вершины (вместе со смежными ребрами)
		/// </summary>
		/// <param name="node">Объект вершины</param>
		private void DeleteNode(GraphNode node)
        {
            foreach (var edge in node.Edges)
            {
                edge.Edges.Remove(node);
            }

			_tempNodes.Remove(node);
        }

		/// <summary>
		/// Получение точек сочленения графа
		/// </summary>
		public List<GraphNode> GetArticulationPoints()
        {
            _marked = new List<int>();
            _ap = new List<GraphNode>();
            _enter = new Dictionary<int, int>();
            _low = new Dictionary<int, int>();
            _parent = new Dictionary<int, int>();
            Enumerable.Repeat(-1, _nodes.Count).ToArray();

			Dfs(_nodes.First());
            return _ap.Distinct().ToList();
        }

		/// <summary>
		/// Обход графа в глубину
		/// </summary>
		/// <param name="node"></param>
        private void Dfs(GraphNode node)
        {
            int children = 0;
            _marked.Add(node.Id);
            ++_time;
            _enter.Add(node.Id, _time);
            _low.Add(node.Id, _time);
			
            foreach (var v in node.Edges)
            {
                if (!_marked.Contains(v.Id))
                {
                    children++;
                    _parent.Add(v.Id, node.Id);

                    Dfs(v);
					
                    _low[node.Id] = Math.Min(_low[node.Id], _low[v.Id]);

	                if (!_parent.Keys.Contains(node.Id) && children > 1)
	                {
		                _ap.Add(node);
	                }

	                if (_parent.Keys.Contains(node.Id) && _low[v.Id] >= _enter[node.Id])
	                {
		                _ap.Add(node);
	                }
                }
                else if (!_parent.Keys.Contains(node.Id) || v.Id != _parent[node.Id])
                {
	                _low[node.Id] = Math.Min(_low[node.Id], _enter[v.Id]);
                }
            }
        }

		/// <summary>
		/// Сортировка вершин графа в порядке удаления
		/// </summary>
        private List<GraphNode> SortNodes()
        {
            var res = new List<GraphNode>();
			_tempNodes = new List<GraphNode>(_nodes);

			while (_tempNodes.Any())
            {
                var singleEdgeNodes = _tempNodes.Where(n => n.Edges.Count == 1).ToArray();

                if (singleEdgeNodes.Any())
                {
                    res.AddRange(singleEdgeNodes);

                    foreach (var node in singleEdgeNodes)
                    {
                        DeleteNode(node);
                    }
                }
                else
                {
                    var artList = GetArticulationPoints();
                    var node = _tempNodes.FirstOrDefault(n => !artList.Contains(n));
                   
                    if (node!= null)
                    {
                        res.Add(node);
                        DeleteNode(node);
                    }
                }
            }
            return res;
        }

		#endregion
	}
}
