using System.Collections.Generic;

namespace Metro.Models
{
	/// <summary>
	/// Модель для рендера графа на фронте
	/// </summary>
    public class RenderModel
    {
		/// <summary>
		/// Список вершин
		/// </summary>
        public List<Node> Nodes { get; set; }
		/// <summary>
		/// Список ребер
		/// </summary>
		public List<Link> Links { get; set; }
    }
}
