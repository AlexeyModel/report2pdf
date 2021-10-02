using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Report2Pdf.Classs
{
    public class Report
    {
        [JsonProperty("token")]
        public string Token { get; set; }

        [JsonProperty("report")]
        public List<ReportRecord> Records { get; set; }
    }

    public class ReportRecord
    {
        [JsonProperty("amount")]
        public float Amount { get; set; }
        [JsonProperty("notes")]
        public string Notes { get; set; }
        [JsonProperty("date")]
        public DateTime DateStart { get; set; }
    }
}
