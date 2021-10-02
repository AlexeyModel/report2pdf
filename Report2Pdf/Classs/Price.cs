using CsvHelper.Configuration.Attributes;

namespace Report2Pdf.Classs
{
    public class Price
    {
        [Name("CustomerID")]
        public string CustomerID { get; set; }

        [Name("Genre")]
        public string Genre { get; set; }

        [Name("Age")]
        public string Age { get; set; }

        [Name("Annual Income (k$)")]
        public string Annual { get; set; }

        [Name("Spending Score (1-100)")]
        public string Spending { get; set; }
    }
}
