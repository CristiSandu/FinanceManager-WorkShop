using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FinanceManager.Models;
using SQLite;
using Xamarin.Essentials;

namespace FinanceManager.Services
{
    public static class DatabaseConnection
    {
        static SQLiteAsyncConnection db;
        static async Task Init()
        {
            if (db != null)
                return;
            var databasePath = Path.Combine(FileSystem.AppDataDirectory, "expencesDb.db");
            db = new SQLiteAsyncConnection(databasePath);

            await db.CreateTableAsync<Models.Transaction>();
        }
      
        public static async Task AddTransaction(Transaction transaction)
        {
            await Init();
            await db.InsertAsync(transaction);
        }

        public static async Task<IEnumerable<Transaction>> GetGlobalTransactions()
        {
            await Init();
            return await db.Table<Transaction>().ToListAsync();
        }

        public static async Task<IEnumerable<Transaction>> GetIncomeTransactions()
        {
            await Init();
            var trans = await db.QueryAsync<Transaction>($"SELECT * FROM \"Transaction\" WHERE Type = \"Income\"");
            return trans;
        }

        public static async Task<IEnumerable<Transaction>> GetExpensesTransactions()
        {
            await Init();
            string query = "SELECT * FROM \"Transaction\" WHERE Type = \"Expense\"";
            var trans = await db.QueryAsync<Transaction>(query);
            var trans_list = trans.ToList();
            return trans;
        }

        public static async Task<float> GetFunctionResult(string query)
        {
            await Init();
            // "SELECT SUM(Price) FROM \"Transaction\" WHERE Type = \"Income\"" get sum from Income
            // "SELECT SUM(Price) FROM \"Transaction\" WHERE Type = \"Expense\"" get sum from Expense

            var incomeSum = await db.ExecuteScalarAsync<float>(query);
            return incomeSum;
        }

        public static async Task<List<Account>> GetAccounts()
        {
            await Init();
            string query = "SELECT Account as Name ,SUM(Price) as Balance FROM \"Transaction\"  WHERE Type = \"Income\" GROUP BY Account";
            var trans = await db.QueryAsync<Account>(query);

            string query2 = "SELECT Account as Name ,SUM(Price) as Balance FROM \"Transaction\" WHERE Type = \"Expense\" GROUP BY Account ";
            var trans2 = await db.QueryAsync<Account>(query2);



            foreach (Account income in trans)
            {
                foreach (Account outcome in trans2)
                {
                    if (income.Name == outcome.Name)
                    {
                        income.Balance -= outcome.Balance;
                    }
                }
            }

            var trans_list = trans.ToList();
            return trans_list;
        }
    }
}
