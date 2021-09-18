using ChromeHtmlToPdfLib;
using ChromeHtmlToPdfLib.Settings;
//using iText.Html2pdf;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Report2Pdf.Controllers
{
    [ApiController]
    [Route("api/v0.0.0.1/[controller]/{token}")]
    public class Html2PdfController : ControllerBase
    {
        private readonly ILogger<Html2PdfController> _logger;

        public Html2PdfController(ILogger<Html2PdfController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<string> Get()
        {
            //using (FileStream htmlSource = System.IO.File.Open(@"d:\arhive\projects\2021\buh-telegram-bot\2021-09-18-generation-report-pdf\index.html", FileMode.Open))
            //using (FileStream pdfDest = System.IO.File.Open("output.pdf", FileMode.OpenOrCreate))
            //{
            //    var converterProperties = new ConverterProperties();
            //    HtmlConverter.ConvertToPdf(htmlSource, pdfDest);
            //}

            using (var converter = new Converter())
            {
                var properties = new PageSettings();
                properties.PreferCSSPageSize = true;
                properties.PrintBackground = true;
                converter.ConvertToPdf(new ConvertUri(@"d:/arhive/projects/2021/buh-telegram-bot/2021-09-18-generation-report-pdf/index.html"), @"output.pdf", properties);
            }

            return default;
        }
    }
}
