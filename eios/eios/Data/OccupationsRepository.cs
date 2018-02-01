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
            var ocup = await GetItems();
            var result = new List<Occupation>();
            foreach (Occupation fr in ocup)
            {
                Console.WriteLine("Инфа: id = " + fr.Id + " id_group =" + fr.IdGroup + " id_occup =" + fr.IdOccupation + " Name =" + fr.Name + " Aud =" + fr.Aud);
                if (fr.IdGroup == id)
                    result.Add(fr);
            }
            return result;
        }
        public async Task<int> SaveItem(Occupation item)
        {
            if (item.Id != 0)
            {
                await database.UpdateAsync(item);
                return item.Id;
            }
            else
            {
                return await database.InsertAsync(item);
            }
        }
        public async Task<List<Occupation>> GetItems()
        {
            return await database.Table<Occupation>().ToListAsync();
        }
    }
}
