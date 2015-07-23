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

            //Insert expense
            foreach (var expense in expenses)
            {
                String insertQuery = "insert into expenses (id, month, expense) values (null, @month, @expense)";
                MySqlCommand cmd = new MySqlCommand(insertQuery, conn);
                cmd.Parameters.AddWithValue("@month", expense.Month);
                cmd.Parameters.AddWithValue("@expense", expense.Expense);
                cmd.ExecuteNonQuery();
            }
        }
    }
}
