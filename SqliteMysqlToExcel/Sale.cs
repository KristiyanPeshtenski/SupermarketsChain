namespace SupermarketChain
{
    public class SaleForExcelReport
    {
        public SaleForExcelReport(string name, decimal income, decimal expense, decimal tax)
        {
            this.Name = name;
            this.Income = income;
            this.Expense = expense;
            this.Tax = tax;
        }

        public string Name { get; set; }

        public decimal Income { get; set; }

        public decimal Expense { get; set; }

        public decimal Tax { get; set; }
    }
}
