using Microsoft.AspNetCore.Http;
using System.IO;

namespace Metro.DAL
{
    public class GraphRepository
    {
        protected GraphRepository(){}

        private sealed class Factory
        {
            private static readonly GraphRepository instance = new GraphRepository();
            public static GraphRepository Instance { get { return instance; } }
        }

        public static GraphRepository Instance
        {
            get { return Factory.Instance; }
        }
        
        private Graph _graph;

	    public Graph Graph
	    {
		    get { return _graph; }
	    }

        public Graph AddGraph(IFormFile file)
        {
            using (var reader = new StreamReader(file.OpenReadStream()))
            {
                var line = reader.ReadLine();
                _graph = new Graph();

                int group = 1;
                while (!reader.EndOfStream)
                {
                    line = reader.ReadLine();

                    if (string.IsNullOrEmpty(line))
                    {
                        group++;
                    }
                    else
                    {
                        var values = line.Split(' ');
                        _graph.AddEdge(values[0], values[1], group);
                    }
                }
            }
            return _graph;
        }
	}
}



