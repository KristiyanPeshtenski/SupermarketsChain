namespace SupermarketsChain
{
    using System;
    using MySql.Data.MySqlClient;

    class SQLServerToMySQLDB
    {
        static void Main()
        {
            var context = new SupermarketsChainEntities();
            var measures = context.Measures;
            var supermarkets = context.Supermarkets;
            var vendors = context.Vendors;
            var expenses = context.Expenses;
            var products = context.Products;
            var sales = context.Sales;

            String myConn = "server=localhost;Database=supermarkets_chain;uid=root;pwd=;";
            MySqlConnection conn = new MySqlConnection(myConn);
            conn.Open();

            //Insert measures
            foreach (var measure in measures)
            {
                String insertQuery = "insert into measures (id, name) values (null, @name)";
                MySqlCommand cmd = new MySqlCommand(insertQuery, conn);
                cmd.Parameters.AddWithValue("@name", measure.Name);
                cmd.ExecuteNonQuery();
            }

            //Insert supermarkets
            foreach (var supermarket in supermarkets)
            {
                String insertQuery = "insert into supermarkets (id, name) values (null, @supermarket)";
                MySqlCommand cmd = new MySqlCommand(insertQuery, conn);
                cmd.Parameters.AddWithValue("@supermarket", supermarket.Name);
                cmd.ExecuteNonQuery();
            }

            //Insert vendors
            foreach (var vendor in vendors)
            {
                String insertQuery = "insert into vendors (id, name) values (null, @vendor)";
                MySqlCommand cmd = new MySqlCommand(insertQuery, conn);
                cmd.Parameters.AddWithValue("@vendor", vendor.Name);
                cmd.ExecuteNonQuery();
            }

            //Insert expenses
            foreach (var expense in expenses)
            {
                String insertQuery = "insert into expenses (id, month, expense) values (null, @month, @expense)";
                MySqlCommand cmd = new MySqlCommand(insertQuery, conn);
                cmd.Parameters.AddWithValue("@month", expense.Month);
                cmd.Parameters.AddWithValue("@expense", expense.Expense);
                cmd.ExecuteNonQuery();
            }

            //Insert products
            foreach (var product in products)
            {
                String insertQuery = "insert into products (id, name, price, measure_id, vendor_id) values (null, @name, @price, @measure_id, @vendor_id)";
                MySqlCommand cmd = new MySqlCommand(insertQuery, conn);
                cmd.Parameters.AddWithValue("@name", product.Name);
                cmd.Parameters.AddWithValue("@price", product.Price);
                cmd.Parameters.AddWithValue("@measure_id", product.Measure.Id);
                cmd.Parameters.AddWithValue("@vendor_id", product.Vendor.Id);
                cmd.ExecuteNonQuery();
            }

            //Insert sales
            foreach (var sale in sales)
            {
                String insertQuery = "insert into sales (id, ordered_on, supermarket_id, product_id) values (null, @ordered_on, @supermarket_id, @product_id)";
                MySqlCommand cmd = new MySqlCommand(insertQuery, conn);
                cmd.Parameters.AddWithValue("@ordered_on", sale.OrderedOn);
                cmd.Parameters.AddWithValue("@supermarket_id", sale.Supermarket.Id);
                cmd.Parameters.AddWithValue("@product_id", sale.Product.Id);
                cmd.ExecuteNonQuery();
            }

            //Insert SupermarketProducts
            foreach (var supermarket in supermarkets)
            {
                foreach (var product in supermarket.Products)
                {
                    String insertQuery = "insert into supermarkets_has_products (supermarket_id, product_id) values (@supermarket_id, @product_id)";
                    MySqlCommand cmd = new MySqlCommand(insertQuery, conn);
                    cmd.Parameters.AddWithValue("@supermarket_id", supermarket.Id);
                    cmd.Parameters.AddWithValue("@product_id", product.Id);
                    cmd.ExecuteNonQuery();
                }
            }

            //Insert VendorsExpenses
            foreach (var vendor in vendors)
            {
                foreach (var expense in vendor.Expenses)
                {
                    String insertQuery = "insert into vendors_has_expenses (vendor_id, expense_id) values (@vendor_id, @expense_id)";
                    MySqlCommand cmd = new MySqlCommand(insertQuery, conn);
                    cmd.Parameters.AddWithValue("@vendor_id", vendor.Id);
                    cmd.Parameters.AddWithValue("@expense_id", expense.Id);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}