using System.Collections.Generic;
using System.Linq;
using Metro.DAL;

namespace Metro.Models
{
	/// <summary>
	/// Хелпер для формирования viewmodel для рендера графа
	/// </summary>
    public static class Converter
    {
        public static RenderModel ConvertToRenderModel(Graph graph)
        {
            var sortedNodes = graph.SortedNodes;

            var response = new RenderModel()
            {
                Nodes = sortedNodes.Select(n => new Node()
                {
                    Id = n.Name,
                    Group = n.Group
                }).ToList(),
				Links = new List<Link>()
            };

            foreach (var node in sortedNodes)
            {
                response.Links.AddRange(node.Edges.Select(e => new Link()
                {
                    Source = node.Name,
                    Target = e.Name
                }));
            }
           
            return response;
        }
    }
}
