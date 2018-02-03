using eios.Model;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace eios.Data
{
    public class OccupationsRepository
    {
        SQLiteAsyncConnection database;
        /*В конструкторе класса происходит создание подключения и базы данных (если она отсутствует)
         *Поскольку на конкретных платформах логика создания будет отличаться, то здесь используется метод DependencyService.Get<ISQLite>(),
         *позволяющий в зависимости от платформы применить определенную реализацию интерфейса ISQLite.
         */
        public OccupationsRepository(string filename)
        {
            string databasePath = DependencyService.Get<ISQLite>().GetDatabasePath(filename);
            database = new SQLiteAsyncConnection(databasePath);
        }
        public async Task CreateTable()
        {
            await database.CreateTableAsync<Occupation>();
        }
        public async Task<List<Occupation>> GetItemsAsync(int id)
        {
            var ocup = await GetItemsAsync();
            var result = new List<Occupation>();
            foreach (Occupation fr in ocup)
            {
                Console.WriteLine("Инфа: id = " + fr.Id + " id_group =" + fr.IdGroup + " Name =" + fr.Name + " Aud =" + fr.Aud);
                if (fr.IdGroup == id)
                    result.Add(fr);
            }
            return result;
        }
        public async Task<int> SaveItem(Occupation item)
        {
            var row = await database.UpdateAsync(item);
            if (row == 0)
            {
                await database.InsertAsync(item);
            }
            return row;
        }
        public async Task<List<Occupation>> GetItemsAsync()
        {
            return await database.Table<Occupation>().ToListAsync();
        }
    }
}
