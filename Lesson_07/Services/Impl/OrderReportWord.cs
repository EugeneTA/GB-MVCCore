using Orders.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TemplateEngine.Docx;

namespace Lesson_07.Services.Impl
{
    public class OrderReportWord : IOrderReport
    {

        #region Tags

        private const string _tagOrderNumber = "OrderNumber";
        private const string _tagOrderDate = "OrderDate";

        private const string _tagSellerContry = "SellerCountry";
        private const string _tagSellerIndex = "SellerIndex";
        private const string _tagSellerRegion = "SellerRegion";
        private const string _tagSellerCity = "SellerCity";
        private const string _tagSellerStreet = "SellerStreet";
        private const string _tagSellerBuilding = "SellerBuilding";
        private const string _tagSellerOffice = "SellerOffice";

        private const string _tagBuyerName = "BuyerName";
        private const string _tagBuyerSecondName = "BuyerSecondName";
        private const string _tagBuyerPhone = "BuyerPhone";
        private const string _tagBuyerAddres = "BuyerAddress";

        private const string _tagProductList = "ProductList";
        private const string _tagProductName = "ProductName";
        private const string _tagProductCount = "ProductCount";
        private const string _tagdProductPrice = "ProductPrice";
        private const string _tagdProductTotal = "ProductTotal";

        private const string _tagOrderTotal = "OrderTotal";

        #endregion

        #region Private Fields

        private readonly FileInfo _templateFile;

        #endregion

        #region Public fields
        public Address SellerAddress { get; set; }
        public Buyer Buyer { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public long OrderNumber { get; set; }
        public DateTime OrderDateTime { get; set; }
        public IEnumerable<(string name, int count, decimal price)> OrderProducts { get; set; }
        #endregion

        #region Constructors

        /// <summary>
        /// 
        /// </summary>
        /// <param name="templateFile">Наименование файла-шаблона</param>
        public OrderReportWord(string templateFile)
        {
            _templateFile = new FileInfo(templateFile);
        }
        #endregion

        public FileInfo Create(string reportFilePath)
        {
            if (!_templateFile.Exists)
                throw new FileNotFoundException();

            var reportFile = new FileInfo(reportFilePath);
            reportFile.Delete();
            _templateFile.CopyTo(reportFile.FullName);

            var productList = OrderProducts.Select(product => new TableRowContent(new List<FieldContent>
            {
                new FieldContent(_tagProductName, product.name),
                new FieldContent(_tagProductCount, product.count.ToString()),
                new FieldContent(_tagdProductPrice, product.price.ToString("c")),
                new FieldContent(_tagdProductTotal, (product.price * product.count).ToString("c"))

            })).ToArray();

            var reportContent = new Content(
                new FieldContent(_tagOrderNumber, OrderNumber.ToString()),
                new FieldContent(_tagOrderDate, OrderDateTime.ToString("dd.MM.yyyy HH:mm:ss")),
                new FieldContent(_tagSellerContry, SellerAddress.Country),
                new FieldContent(_tagSellerIndex, SellerAddress.PostalCode),
                new FieldContent(_tagSellerRegion, SellerAddress.Region),
                new FieldContent(_tagSellerCity, SellerAddress.City),
                new FieldContent(_tagSellerStreet, SellerAddress.Street),
                new FieldContent(_tagSellerBuilding, SellerAddress.Building),
                new FieldContent(_tagSellerOffice, SellerAddress.Office),


                new FieldContent(_tagBuyerName, Buyer.Name),
                new FieldContent(_tagBuyerSecondName, Buyer.LastName),
                new FieldContent(_tagBuyerPhone, Phone),
                new FieldContent(_tagBuyerAddres, Address),

                TableContent.Create(_tagProductList, productList),
                new FieldContent(_tagOrderTotal, OrderProducts.Sum(product => (product.price * product.count)).ToString("c"))
                );

            using (var templateProcessor = new TemplateProcessor(reportFile.FullName).SetRemoveContentControls(true))
            {
                templateProcessor.FillContent(reportContent);
                templateProcessor.SaveChanges();
                reportFile.Refresh();
                return reportFile;
            }
        }
    }
}
