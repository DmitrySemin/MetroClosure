using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using Metro.DAL;
using Metro.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NLog;

namespace Metro.Controllers
{
	public class HomeController : Controller
	{
		private const string OUTPUT_FILE_NAME = "Output.txt";
		private Logger _logger = LogManager.GetCurrentClassLogger();

		public IActionResult Index()
		{
			return View();
		}

		public IActionResult Error()
		{
			return View();
		}
		/// <summary>
		/// Загрузка исходного файла
		/// </summary>
		/// <param name="file">Файл с данными</param>
		[HttpPost]
		public ActionResult Upload([Required] IFormFile file)
		{
			try
			{
				if (ModelState.IsValid && file.Length > 0)
				{
					GraphRepository.Instance.AddGraph(file);
					return RedirectToAction("Index");
				}
				return View("Error");
			}
			catch (Exception e)
			{
				_logger.Error(e);
				return View("Error");
			}
		}

		/// <summary>
		/// Отрисовка графа
		/// </summary>
		/// <returns></returns>
		[HttpGet]
		public JsonResult Render()
		{
			try
			{
				var graph = GraphRepository.Instance.Graph;

				if(graph == null)
					return Json(null);

				RenderModel renderModel = Converter.ConvertToRenderModel(graph);
				return Json(renderModel);
			}
			catch (Exception e)
			{
				_logger.Error(e);
				return Json(null);
			}
		}
		
		/// <summary>
		/// Выгрузка файла с результатами
		/// </summary>
		/// <returns></returns>
		[HttpGet]
		public FileResult Download()
		{
			var nodes = GraphRepository.Instance.Graph.SortedNodes;

			if (nodes != null && nodes.Any())
			{
				using (var ms = new MemoryStream())
				{
					TextWriter tw = new StreamWriter(ms);

					foreach (var node in nodes)
					{
						tw.WriteLine(node.Name);
					}

					tw.Flush();
					var bytes = ms.ToArray();

					return File(bytes, "application/x-msdownload", OUTPUT_FILE_NAME);
				}
			}

			return null;
		}
	}
}
