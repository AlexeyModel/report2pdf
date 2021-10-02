using AbsLibraryCore.DateTimeAtlas.Transform;
using ChromeHtmlToPdfLib;
using ChromeHtmlToPdfLib.Settings;
using CsvHelper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Report2Pdf.Classs;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

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
        public IEnumerable<string> Get(string token)
        {
            if (!token.Equals("E18337CD-74AB-4BAF-9284-DFF640BF11E6"))
                return default;

            var transactionPath = "d:\\Downloads\\Mall_Customers.csv";
            period = TransformDateTimeAtlas.GetFirstLastDayMonth(DateTime.Now.Year, DateTime.Now.Month);
            var folderName = DateTime.Now.Ticks;
            var sourcePath = @"d:/arhive/projects/2021/buh-telegram-bot/2021-09-18-generation-report-pdf/index.html";
            var sourceContent = System.IO.File.ReadAllText(sourcePath);
            Directory.CreateDirectory($"{ AppDomain.CurrentDomain.BaseDirectory}{folderName}\\");
            var stylesPath = @"d:/arhive/projects/2021/buh-telegram-bot/2021-09-18-generation-report-pdf/style.css";
            var outputHtml = $"{AppDomain.CurrentDomain.BaseDirectory}{folderName}\\output.html";
            var outputPdf =  $"{AppDomain.CurrentDomain.BaseDirectory}{folderName}\\output.pdf";
            System.IO.File.Copy(stylesPath, $"{AppDomain.CurrentDomain.BaseDirectory}{folderName}\\style.css");

            using (var reader = new StreamReader(transactionPath))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                records = csv.GetRecords<Price>().ToList();
            }

            var table = new StringBuilder();
            int i = 0;

            var note1 = "Spending";
            var note2 = "Receipt";

            foreach (var row in records)
            {
                table.Append("<tr>");
                table.Append("<td>");
                table.Append(++i);
                table.Append("</td>");
                table.Append("<td>");
                table.Append(row.Genre);
                table.Append("</td>");
                table.Append("<td>");
                table.Append(row.Age);
                table.Append($" ({(row.Age.Contains('-') ? note1 : note2)})");
                table.Append("</td>");
                table.Append("<td>");
                table.Append(DateTime.Now);
                table.Append("</td>");
                table.Append("</tr>");
            }

            System.IO.File.WriteAllText(
                outputHtml,
                sourceContent
                    .Replace("#report_start#", period["First"].ToString("yyyy-MM-dd"))
                    .Replace("#report_end#", period["Last"].ToString("yyyy-MM-dd"))
                    .Replace("#top_tags#", records.Select(x => x.Genre).Distinct().Aggregate((x, y) => x + ", " + y))
                    .Replace("#transaction_count#", records.Count.ToString())
                    .Replace("#tags_count#", records.Select(x => x.Genre).Distinct().ToList().Count.ToString())
                    .Replace("#current_year#", DateTime.Now.Year.ToString())
                    .Replace("#table_reports#", table.ToString())
            );

            using (var converter = new Converter())
            {
                var properties = new PageSettings();
                properties.PreferCSSPageSize = true;
                properties.PrintBackground = true;
                converter.ConvertToPdf(new ConvertUri(outputHtml), outputPdf, properties);
            }

            return default;
        }

        [HttpPost]
        public string Post(string token)
        {
            if (!token.Equals("46DDA4EC-79B2-44D8-BF22-A2E664DFBEC6"))
                return default;

            var req = HttpContext.Request;

            using (var reader = new StreamReader(req.Body))
            {
                string content = reader.ReadToEndAsync().Result;
                reports = JsonConvert.DeserializeObject<Report>(content);
            }

            period = TransformDateTimeAtlas.GetFirstLastDayMonth(
                reports
                    .Records
                    .OrderBy(r => r.DateStart).Select(r => r.DateStart).FirstOrDefault().Year,
                reports
                    .Records
                    .OrderBy(r => r.DateStart).Select(r => r.DateStart).FirstOrDefault().Month
            );

            var folderName = DateTime.Now.Ticks;
            var folderToken = reports.Token;
            var sourcePath = $"{AppDomain.CurrentDomain.BaseDirectory}resource\\index.light.html";
            var sourceContent = System.IO.File.ReadAllText(sourcePath);
            Directory.CreateDirectory($"{ AppDomain.CurrentDomain.BaseDirectory}{folderToken}\\{folderName}\\");
            var stylesPath = $"{AppDomain.CurrentDomain.BaseDirectory}resource\\style.css";
            var outputHtml = $"{AppDomain.CurrentDomain.BaseDirectory}{folderToken}\\{folderName}\\output.html";
            var outputPdf = $"{AppDomain.CurrentDomain.BaseDirectory}{folderToken}\\{folderName}\\output-{folderName}.pdf";
            System.IO.File.Copy(stylesPath, $"{AppDomain.CurrentDomain.BaseDirectory}{folderToken}\\{folderName}\\style.css");
            var totalNote = "Total Result";

            var table = new StringBuilder();
            int i = 0;

            var note1 = "Spending";
            var note2 = "Receipt";

            foreach (var row in reports.Records)
            {
                table.Append("<tr>");
                table.Append("<td>");
                table.Append(++i);
                table.Append("</td>");
                table.Append("<td>");
                table.Append(row.Notes);
                table.Append("</td>");
                table.Append("<td>");
                table.Append(row.Amount);
                table.Append($" ({(row.Amount < 0 ? note1 : note2)})");
                table.Append("</td>");
                table.Append("<td>");
                table.Append(row.DateStart);
                table.Append("</td>");
                table.Append("</tr>");
            }

            var total = new StringBuilder();

            var totalValue = reports.Records.Where(r => r.Amount >= 0).Select(r => r.Amount).Sum() + reports.Records.Where(r => r.Amount < 0).Select(r => r.Amount).Sum();

            total.Append("<tr>");
            total.Append("<td></td>");
            total.Append("<td>");
            total.Append(reports.Records.Where(r => r.Amount >= 0).Select(r => r.Amount).Sum());
            total.Append("</td>");
            total.Append("<td>");
            total.Append(note2);
            total.Append("</td>");
            total.Append("</tr>");

            total.Append("<tr>");
            total.Append("<td></td>");
            total.Append("<td>");
            total.Append(Math.Abs(reports.Records.Where(r => r.Amount < 0).Select(r => r.Amount).Sum()));
            total.Append("</td>");
            total.Append("<td>");
            total.Append(note1);
            total.Append("</td>");
            total.Append("</tr>");

            total.Append("<tr>");
            total.Append("<td></td>");
            total.Append("<td>");
            total.Append($"<b>{Math.Abs(totalValue)}</b>");
            total.Append("</td>");
            total.Append("<td>");
            total.Append($"<b>{totalNote} {(totalValue > 0 ? note2: note1)}</b>");
            total.Append("</td>");
            total.Append("</tr>");

            System.IO.File.WriteAllText(
                outputHtml,
                sourceContent
                    .Replace("#report_start#", period["First"].ToString("yyyy-MM-dd"))
                    .Replace("#report_end#", period["Last"].ToString("yyyy-MM-dd"))
                    .Replace("#top_tags#", reports.Records.Select(x => x.Notes).Distinct().Aggregate((x, y) => x + ", " + y))
                    .Replace("#transaction_count#", reports.Records.Count.ToString())
                    .Replace("#tags_count#", reports.Records.Select(x => x.Notes).Distinct().ToList().Count.ToString())
                    .Replace("#current_year#", DateTime.Now.Year.ToString())
                    .Replace("#table_reports#", table.ToString())
                    .Replace("#table_total#", total.ToString())
            );

            using (var converter = new Converter())
            {
                var properties = new PageSettings();
                properties.PreferCSSPageSize = true;
                properties.PrintBackground = true;
                converter.ConvertToPdf(new ConvertUri(outputHtml), outputPdf, properties);
            }

            return $"https://";
        }

        private static Report reports;
        private static List<Price> records;
        private static Dictionary<string, DateTime> period;
    }
}
