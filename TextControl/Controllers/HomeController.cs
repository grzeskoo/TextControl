using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using TextControl.Models;

namespace TextControl.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [HttpGet]
        public IActionResult MergeDocument()
        {

            byte[] bDocument;

            // create a ServerTextControl
            using (TXTextControl.ServerTextControl tx = new TXTextControl.ServerTextControl())
            {

                tx.Create();

                // load the template
                tx.Load("App_Data/template.tx", TXTextControl.StreamType.InternalUnicodeFormat);

                // create the mail merge engine
                using (TXTextControl.DocumentServer.MailMerge mm = new TXTextControl.DocumentServer.MailMerge())
                {
                    // connect to ServerTextControl instance
                    mm.TextComponent = tx;

                    // merge data
                    var jsonData = System.IO.File.ReadAllText("App_Data/data.json");
                    mm.MergeJsonData(jsonData);
                }

                // save in the internal format
                tx.Save(out bDocument, TXTextControl.BinaryStreamType.InternalUnicodeFormat);
            }

            return Ok(bDocument);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}