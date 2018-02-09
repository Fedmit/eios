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
            await database.CreateTableAsync<StudentAbsent>();
            await database.CreateTableAsync<Student>();
            await database.CreateTableAsync<Occupation>();
            await database.CreateTableAsync<Group>();
        }

        public async Task DeleteThisShits()
        {
            await database.DropTableAsync<StudentAbsent>();
            await database.DropTableAsync<Student>();
            await database.DropTableAsync<Occupation>();
            await database.DropTableAsync<Group>();
        }

        public async Task<List<Occupation>> GetOccupations(int idGroup)
        {
            try
            {
                return await database.QueryAsync<Occupation>(
                    "SELECT id_ocup, lesson_name, lesson_id, aud FROM Occupations WHERE id_group = ?",
                    idGroup
                );
            }
            catch (SQLiteException ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public async Task <List<StudentAttendance>> GetAttendance(int idOccupation, int idGroup)
        {
            try
            {
                var students = await database.QueryAsync<Student>(
                    "SELECT id_student, firstname, lastname, secondname FROM Students WHERE id_group = ? ORDER BY id_student",
                    idGroup
                );
                var attendance = await database.QueryAsync<StudentAbsent>(
                    "SELECT id_student FROM Attendance WHERE id_group = ? AND id_ocup = ? ORDER BY id_student",
                    idGroup, idOccupation
                );
                var result = new List<StudentAttendance>();
                int j = 0;
                for (int i = 0; i < students.Count; i++)
                {
                    if (j != attendance.Count)
                    {
                        if (students[i].Id == attendance[j].Id)
                        {
                            j++;
                            result.Add(new StudentAttendance {
                                Id = students[i].Id,
                                FirstName = students[i].FirstName,
                                LastName = students[i].LastName,
                                SecondName = students[i].SecondName,
                                IsAbsent = true });
                        }
                        else
                        {
                            result.Add(new StudentAttendance
                            {
                                Id = students[i].Id,
                                FirstName = students[i].FirstName,
                                LastName = students[i].LastName,
                                SecondName = students[i].SecondName,
                                IsAbsent = false
                            });
                        }
                    }
                    else
                    {
                        result.Add(new StudentAttendance
                        {
                            Id = students[i].Id,
                            FirstName = students[i].FirstName,
                            LastName = students[i].LastName,
                            SecondName = students[i].SecondName,
                            IsAbsent = false
                        });
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

        public async Task <List<Occupation>> GetMarks(int idGroup)
        {
            try
            {
                return await database.QueryAsync<Occupation>("SELECT id_ocup, is_check, is_block FROM Occupations WHERE id_group = ?", idGroup);
            }
            catch (SQLiteException ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public async Task SetStudents(List<Student> students)
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

        public async Task SetGroup(List<Group> groups)
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

        public async Task SetMarks(List<Occupation> marks)
        {
            try
            {
                var cache = await database.Table<Occupation>().ToListAsync();
                await database.DropTableAsync<Occupation>();
                for (int i = 0; i < cache.Count; i++)
                {
                    marks[i].IdGroup = cache[i].IdGroup;
                }
                await SetOccupations(marks);
            }
            catch (SQLiteException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public async Task SetOccupations(List<Occupation> list) 
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

        public async Task<List<Group>> GetGroups()
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
        
        public async Task SetSentFlag(int idOccupation, int idGroup)
        {
            try
            {
                var list = await database.QueryAsync<Occupation>(
                    "SELECT * FROM Occupations WHERE id_ocup = ? AND id_group = ?",
                    idOccupation, idGroup
                );
                list[0].is_sent = true;
                await database.UpdateAllAsync(list);
            }
            catch (SQLiteException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public async Task<List<Occupation>> GetUnsentOccupations()
        {
            try
            {
                return await database.QueryAsync<Occupation>("SELECT id_ocup, id_group FROM Occupations WHERE is_sent = 0");
            }
            catch (SQLiteException ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public async Task<List<StudentAbsent>> GetAbsentStudents(int idOccupation, int idGroup)
        {
            try
            {
                return await database.QueryAsync<StudentAbsent>("SELECT id_student FROM Attendance WHERE id_ocup = ? AND id_group = ?", idOccupation, idGroup);
            }
            catch (SQLiteException ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public async Task SetAttendence(List<StudentAbsent> refreshList, int idOccupation, int idGroup)
        {
            try
            {
                await database.QueryAsync<StudentAbsent>("DELETE FROM Attendance WHERE id_ocup = ? AND id_group = ?", idOccupation, idGroup);
                await database.InsertAllAsync(refreshList);
            }  catch (SQLiteException ex)
            {
                Console.WriteLine(ex.Message);  
            }
        }

        public async Task <List<StudentSelect>> GetStudents(int idGroup)
        {
            try
            {
                return await database.QueryAsync<StudentSelect>("SELECT id_student, firstname, lastname, secondname FROM Students WHERE id_group =?", idGroup);
            }
            catch (SQLiteException ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }
    }
}
