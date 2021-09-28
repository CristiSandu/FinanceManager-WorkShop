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
            string query = "SELECT IFNULL(a.ACC ,b.ACC) as Name , (IFNULL(a.Balance,0) - IFNULL(b.Balance,0)) as Balance FROM (SELECT Account as ACC ,SUM(Price) as Balance FROM \"Transaction\"  WHERE Type = \"Income\" GROUP BY Account) a,(SELECT Account as ACC ,SUM(Price) as Balance FROM \"Transaction\" WHERE Type = \"Expense\" GROUP BY Account) b GROUP BY Name";
            var trans = await db.QueryAsync<Account>(query);

            var trans_list = trans.ToList();
            return trans_list;
        }
    }
}