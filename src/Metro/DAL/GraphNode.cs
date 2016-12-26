using System.Collections.Generic;

namespace Metro.DAL
{
	/// <summary>
	/// Вершина графа
	/// </summary>
	public class GraphNode
	{
		/// <summary>
		/// Идетификатор
		/// </summary>
		public int Id { get; set; }
		/// <summary>
		/// Наименование
		/// </summary>
		public string Name { get; set; }
		/// <summary>
		/// Список смежных вершин
		/// </summary>
		public List<GraphNode> Edges { get; set; }
		/// <summary>
		/// Группа вершины
		/// </summary>
		public int Group { get; set; }

		public GraphNode()
		{
			Edges = new List<GraphNode>();
		}
	}
}
