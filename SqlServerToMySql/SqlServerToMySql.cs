namespace SupermarketChain
{
    using System.Data.Entity;
    using MySql.Data.MySqlClient;

    public class SqlServerToMySql
    {
        private const string MySqlDataSource = "server=localhost;Database=supermarkets_chain;uid=root;pwd=;";
        private const string ExportToMySQLSuccess = "Export data to MySql server: Done!";

        public string ExportDataIntoMySql()
        {
            var context = new SupermarketsChainSqlServerEntities();
            var measures = context.Measures;
            var supermarkets = context.Supermarkets;
            var vendors = context.Vendors;
            var expenses = context.Expenses;
            var products = context.Products;
            var sales = context.Sales;

            var connection = new MySqlConnection(MySqlDataSource);
            connection.Open();
            using (connection)
            {
                InsertMeasuresIntoDb(measures, connection);

                InsertSupermarketsIntoDb(supermarkets, connection);

                InsertVendorsIntoDb(vendors, connection);

                InsertExpensesIntoDb(expenses, connection);

                InsertProductsIntoDb(products, connection);

                InsertSalesIntoDb(sales, connection);

                InsertSupermarketsProductsIntoDb(supermarkets, connection);

                InsertVendorsExpensesIntoDb(vendors, connection);
            }

            return ExportToMySQLSuccess;
        }

        private static void InsertVendorsExpensesIntoDb(DbSet<Vendors> vendors, MySqlConnection connection)
        {
            foreach (var vendor in vendors)
            {
                foreach (var expense in vendor.Expenses)
                {
                    var insertQuery =
                        "insert into vendors_has_expenses (vendor_id, expense_id) values (@vendor_id, @expense_id)";
                    var cmd = new MySqlCommand(insertQuery, connection);
                    cmd.Parameters.AddWithValue("@vendor_id", vendor.Id);
                    cmd.Parameters.AddWithValue("@expense_id", expense.Id);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        private static void InsertSupermarketsProductsIntoDb(DbSet<Supermarkets> supermarkets, MySqlConnection connection)
        {
            foreach (var supermarket in supermarkets)
            {
                foreach (var product in supermarket.Products)
                {
                    var insertQuery =
                        "insert into supermarkets_has_products (supermarket_id, product_id) values (@supermarket_id, @product_id)";
                    var cmd = new MySqlCommand(insertQuery, connection);
                    cmd.Parameters.AddWithValue("@supermarket_id", supermarket.Id);
                    cmd.Parameters.AddWithValue("@product_id", product.Id);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        private static void InsertSalesIntoDb(DbSet<Sales> sales, MySqlConnection connection)
        {
            foreach (var sale in sales)
            {
                var insertQuery =
                    "insert into sales (id, ordered_on, supermarket_id, product_id) values (null, @ordered_on, @supermarket_id, @product_id)";
                var cmd = new MySqlCommand(insertQuery, connection);
                cmd.Parameters.AddWithValue("@ordered_on", sale.OrderedOn);
                cmd.Parameters.AddWithValue("@supermarket_id", sale.Supermarkets.Id);
                cmd.Parameters.AddWithValue("@product_id", sale.Products.Id);
                cmd.ExecuteNonQuery();
            }
        }

        private static void InsertProductsIntoDb(DbSet<Products> products, MySqlConnection connection)
        {
            foreach (var product in products)
            {
                var insertQuery =
                    "insert into products (id, name, price, measure_id, vendor_id) values (null, @name, @price, @measure_id, @vendor_id)";
                var cmd = new MySqlCommand(insertQuery, connection);
                cmd.Parameters.AddWithValue("@name", product.Name);
                cmd.Parameters.AddWithValue("@price", product.Price);
                cmd.Parameters.AddWithValue("@measure_id", product.Measures.Id);
                cmd.Parameters.AddWithValue("@vendor_id", product.Vendors.Id);
                cmd.ExecuteNonQuery();
            }
        }

        private static void InsertExpensesIntoDb(DbSet<Expenses> expenses, MySqlConnection connection)
        {
            foreach (var expense in expenses)
            {
                var insertQuery = "insert into expenses (id, month, expense) values (null, @month, @expense)";
                var cmd = new MySqlCommand(insertQuery, connection);
                cmd.Parameters.AddWithValue("@month", expense.Month);
                cmd.Parameters.AddWithValue("@expense", expense.Expense);
                cmd.ExecuteNonQuery();
            }
        }

        private static void InsertVendorsIntoDb(DbSet<Vendors> vendors, MySqlConnection connection)
        {
            foreach (var vendor in vendors)
            {
                var insertQuery = "insert into vendors (id, name) values (null, @vendor)";
                var cmd = new MySqlCommand(insertQuery, connection);
                cmd.Parameters.AddWithValue("@vendor", vendor.Name);
                cmd.ExecuteNonQuery();
            }
        }

        private static void InsertSupermarketsIntoDb(DbSet<Supermarkets> supermarkets, MySqlConnection connection)
        {
            foreach (var supermarket in supermarkets)
            {
                var insertQuery = "insert into supermarkets (id, name) values (null, @supermarket)";
                var cmd = new MySqlCommand(insertQuery, connection);
                cmd.Parameters.AddWithValue("@supermarket", supermarket.Name);
                cmd.ExecuteNonQuery();
            }
        }

        private static void InsertMeasuresIntoDb(DbSet<Measures> measures, MySqlConnection connection)
        {
            foreach (var measure in measures)
            {
                var insertQuery = "insert into measures (id, name) values (null, @name)";
                var cmd = new MySqlCommand(insertQuery, connection);
                cmd.Parameters.AddWithValue("@name", measure.Name);
                cmd.ExecuteNonQuery();
            }
        }
    }
}