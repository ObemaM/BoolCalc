using BooleanMinimizator.Models;
using BooleanMinimizerLibrary;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace BooleanMinimizator.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View(new BooleanMinimizatorModel());
        }

        [HttpPost]
        public IActionResult Index(BooleanMinimizatorModel model)
        {
            if (!string.IsNullOrEmpty(model.InputFunction))
            {
                try
                {
                    // Парсим входную формулу
                    var syntaxAnalyzer = new SyntaxAnalyzer();
                    Node rootNode = syntaxAnalyzer.Parse(model.InputFunction);

                    // Строим вектор и таблицу истинности
                    var functionVectorBuilder = new FunctionVectorBuilder();
                    model.VectorOutput = functionVectorBuilder.BuildVector(rootNode);
                    model.TruthTable = functionVectorBuilder.BuildTruthTable(rootNode);

                    // Получаем ПОЛИЗ
                    model.PolizOutput = string.Join(" ", syntaxAnalyzer.GetPOLIZ(rootNode));

                    // Вычисляем МКНФ и МДНФ
                    model.MKNFOutput = BooleanMinimizer.MinimizeMKNF(model.VectorOutput);
                    model.MDNFOutput = BooleanMinimizer.MinimizeMDNF(model.VectorOutput);

                    // Успешное завершение
                    model.ResultMessage = "Функция успешно распознана!";
                }
                catch (Exception ex)
                {
                    // Обработка ошибок
                    model.ResultMessage = $"Ошибка: {ex.Message}";
                }
            }
            return View(model);
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
