using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Recommender_System_FT.Models;
using Microsoft.ML;
using Microsoft.ML.Data;
using Microsoft.ML.Trainers;
using System;
using System.IO;
using System.Linq;

namespace Recommender_System_FT.Controllers
{
    public class HomeController : Controller
    {
        private static string BaseDataSetRelativePath = @"../../../Data";  // Thư mục chứa dữ liệu
        private static string TrainingDataRelativePath = $"{BaseDataSetRelativePath}/Amazon0302.txt"; // Khai báo đường dẫn tương đối
        private static string TrainingDataLocation = GetAbsolutePath(TrainingDataRelativePath); // Lấy đường dẫn tuyệt đối từ đường dẫn tương đối
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult Predict(int id)
        {
            MLContext mlContext = new MLContext();
            var traindata = mlContext.Data.LoadFromTextFile(path: TrainingDataLocation,
                                                      columns: new[]
                                                                {
                                                                    new TextLoader.Column("Label", DataKind.Single, 0),
                                                                    new TextLoader.Column(name:nameof(ProductEntry.ProductID), dataKind:DataKind.UInt32, source: new [] { new TextLoader.Range(0) }, keyCount: new KeyCount(262111)),
                                                                    new TextLoader.Column(name:nameof(ProductEntry.CoPurchaseProductID), dataKind:DataKind.UInt32, source: new [] { new TextLoader.Range(1) }, keyCount: new KeyCount(262111))
                                                                },
                                                      hasHeader: true,
                                                      separatorChar: '\t');
            MatrixFactorizationTrainer.Options options = new MatrixFactorizationTrainer.Options();
            options.MatrixColumnIndexColumnName = nameof(ProductEntry.ProductID);
            options.MatrixRowIndexColumnName = nameof(ProductEntry.CoPurchaseProductID);
            options.LabelColumnName = "Label";
            options.LossFunction = MatrixFactorizationTrainer.LossFunctionType.SquareLossOneClass;
            options.Alpha = 0.01;
            options.Lambda = 0.025;

            var est = mlContext.Recommendation().Trainers.MatrixFactorization(options);

            ITransformer model = est.Fit(traindata);
            var predictionengine = mlContext.Model.CreatePredictionEngine<ProductEntry, Copurchase_prediction>(model);
            var top10 = (from m in Enumerable.Range(1, 262111)
                         let p = predictionengine.Predict(
                            new ProductEntry()
                            {
                                ProductID = (uint)id,
                                CoPurchaseProductID = (uint)m
                            })
                         orderby p.Score descending
                         select (ProductID: m, Score: p.Score)).Take(10);


            List<ResultModel> lst = new List<ResultModel>();
            foreach (var t in top10)
                lst.Add(new ResultModel
                {
                    Score = t.Score,
                    Product =  t.ProductID
                });
          return Json(lst);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public static string GetAbsolutePath(string relativeDatasetPath)
        {
            FileInfo _dataRoot = new FileInfo(typeof(Program).Assembly.Location);
            string assemblyFolderPath = _dataRoot.Directory.FullName;

            string fullPath = Path.Combine(assemblyFolderPath, relativeDatasetPath);

            return fullPath;
        }

        public class Copurchase_prediction
        {
            public float Score { get; set; }
        }

        public class ProductEntry
        {
            [KeyType(count: 262111)]
            public uint ProductID { get; set; }

            [KeyType(count: 262111)]
            public uint CoPurchaseProductID { get; set; }
        }

        public class ResultModel
        {
            public float Score { get; set; }
            public int Product { get; set; }
        }

    }

}
