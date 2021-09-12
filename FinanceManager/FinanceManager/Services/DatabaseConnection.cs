using System;
using System.Collections.Generic;
using System.IO;
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
            await db.CreateTableAsync<Models.Account>();
        }

        public static async Task AddAccount(string name, float balance)
        {
            await Init();

            var acc = new Account
            {
                Name = name,
                Balance = balance,
            };

            await db.InsertAsync(acc);
        }


        public static async Task<IEnumerable<Account>> GetAccounts()
        {
            await Init();

            return await db.Table<Account>().ToListAsync();
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
            var trans = await db.QueryAsync<Transaction>($"SELECT * FROM Transaction WHERE Type = income");
            return trans;
        }

        public static async Task<IEnumerable<Transaction>> GetExpensesTransactions()
        {
            await Init();
            var trans = await db.QueryAsync<Transaction>($"SELECT * FROM Transaction WHERE Type = expenses");
            return trans;
        }
    }
}
