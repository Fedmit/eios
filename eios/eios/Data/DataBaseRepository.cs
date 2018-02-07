using System.Collections.Generic;
using System.Threading.Tasks;
using SQLite;
using Xamarin.Forms;
using eios.Model;
using System;

namespace eios.Data
{
    public class DataBaseRepository
    {
        private SQLiteAsyncConnection database;
        public DataBaseRepository(string filename)
        {
            string databasePath = DependencyService.Get<ISQlite>().GetDatabasePath(filename);
            database = new SQLiteAsyncConnection(databasePath);
        }

        public async Task CreateTables()
        {
            await database.CreateTableAsync<Attendance>();
            await database.CreateTableAsync<Student>();
            await database.CreateTableAsync<Occupation>();
            await database.CreateTableAsync<Group>();
        }

        public async Task deleteThisShits()
        {
            await database.DropTableAsync<Attendance>();
            await database.DropTableAsync<Student>();
            await database.DropTableAsync<Occupation>();
            await database.DropTableAsync<Group>();
        }

        public async Task<List<Occupation>> GetMeOccupation(int id_group)
        {
            try
            {
                return await database.QueryAsync<Occupation>("SELECT id_ocup, lesson_name, lesson_id, aud FROM Occupations WHERE id_group = ?", id_group);
            }
            catch (SQLiteException ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public async Task <List<AttendStudent>> GetMeAttendence(int id_ocup, int id_group)
        {
            try
            {
                List<Student> students = await database.QueryAsync<Student>("SELECT id_student, fio FROM Students WHERE id_group = ? ORDER BY id_student", id_group);
                List<Attendance> attendance = await database.QueryAsync<Attendance>("SELECT id_student FROM Attendance WHERE id_group = ? AND id_ocup = ? ORDER BY id_student", id_group, id_ocup);
                List<AttendStudent> result = new List<AttendStudent>();
                int j = 0;
                for (int i = 0; i < students.Count; i++)
                {
                    if (j != attendance.Count)
                    {
                        if (students[i].Id == attendance[j].id_student)
                        {
                            j++;
                            result.Add(new AttendStudent { id_student = students[i].Id, fio = students[i].FullName, is_absent = true });
                        }
                        else
                        {
                            result.Add(new AttendStudent { id_student = students[i].Id, fio = students[i].FullName, is_absent = false });
                        }
                    }
                    else
                    {
                        result.Add(new AttendStudent { id_student = students[i].Id, fio = students[i].FullName, is_absent = false });
                    }
                }
                return result;
            }
            catch (SQLiteException ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }

        }

        public async Task <List<Occupation>> GetMeMarks(int id_group)
        {
            try
            {
                return await database.QueryAsync<Occupation>("SELECT id_ocup, is_check, is_block FROM Occupations WHERE id_group = ?", id_group);
            }
            catch (SQLiteException ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public async Task SetYouStudents(List<Student> students)
        {
            try
            {
                await database.InsertAllAsync(students);
            }
            catch (SQLiteException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public async Task SetYouGroup(List<Group> groups)
        {
            try
            {
                await database.InsertAllAsync(groups);
            }
            catch (SQLiteException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public async Task SetYouMarks(List<Occupation> marks)
        {
            try
            {
                List<Occupation> cache = await database.Table<Occupation>().ToListAsync();
                await database.DropTableAsync<Occupation>();
                for (int i = 0; i < cache.Count; i++)
                {
                    marks[i].IdGroup = cache[i].IdGroup;
                    marks[i].Name = cache[i].Name;
                    marks[i].IdLesson = cache[i].IdLesson;
                    marks[i].Aud = cache[i].Aud;
                }
                SetYouOccupation(marks);
            }
            catch (SQLiteException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public async Task SetYouOccupation(List<Occupation> list) 
        {
            try
            {
                await database.InsertAllAsync(list);
            }
            catch (SQLiteException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public async Task<List<Group>> GetMeGroups()
        {
            try
            {
                return await database.Table<Group>().ToListAsync();
            }
            catch (SQLiteException ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }
        
        public async Task SetSentFlag(int id_ocup, int id_group)
        {
            try
            {
                List<Occupation> list = await database.QueryAsync<Occupation>("SELECT * FROM Occupations WHERE id_ocup = ? AND id_group = ?", id_ocup, id_group);
                list[0].is_sent = true;
                await database.UpdateAllAsync(list);
            }
            catch (SQLiteException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public async Task<List<Occupation>> SentAttendence()
        {
            try
            {
                return await database.QueryAsync<Occupation>("SELECT id_ocup, id_group FROM Occupations WHERE is_sent = 0");
            } catch (SQLiteException ex)
            {
                Console.WriteLine(ex.Message);
                return null;    
            }
        }

        public async Task<List<Attendance>> SentNotSynhronized(int id_ocup, int id_group)
        {
            try
            {
                return await database.QueryAsync<Attendance>("SELECT id_student FROM Attendance WHERE id_ocup = ? AND id_group = ?", id_ocup, id_group);
            }
            catch (SQLiteException ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public async Task UpdateAttendence(List<Attendance> refreshList, int id_ocup, int id_group)
        {
            try
            {
                await database.QueryAsync<Attendance>("DELETE FROM Attendance WHERE id_ocup = ? AND id_group = ?", id_ocup, id_group);
                await database.InsertAllAsync(refreshList);
            }
            catch (SQLiteException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
