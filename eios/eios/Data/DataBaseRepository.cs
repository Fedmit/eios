using System.Collections.Generic;
using System.Threading.Tasks;
using SQLite;
using Xamarin.Forms;
using eios.Model;
using System;
using System.Diagnostics;

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

        public async Task DropTable<T>() where T : new()
        {
            await database.DropTableAsync<T>();
        }

        public async Task CreateTable<T>() where T : new()
        {
            await database.CreateTableAsync<T>();
        }

        public async Task<List<Occupation>> GetOccupations(int idGroup)
        {
            try
            {
                return await database.QueryAsync<Occupation>(
                    "SELECT * FROM Occupations WHERE id_group = ? ORDER BY id_occup",
                    idGroup
                );
            }
            catch (SQLiteException ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public async Task<List<StudentAttendance>> GetAttendance(int idOccupation, int idGroup)
        {
            try
            {
                var students = await database.QueryAsync<Student>(
                    "SELECT id_student, fullname FROM Students WHERE id_group = ? ORDER BY id_student",
                    idGroup
                );
                var attendance = await database.QueryAsync<StudentAbsent>(
                    "SELECT id_student FROM Attendance WHERE id_group = ? AND id_occup = ? ORDER BY id_student",
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
                            result.Add(new StudentAttendance
                            {
                                Id = students[i].Id,
                                FullName = students[i].FullName,
                                IsAbsent = true
                            });
                        }
                        else
                        {
                            result.Add(new StudentAttendance
                            {
                                Id = students[i].Id,
                                FullName = students[i].FullName,
                                IsAbsent = false
                            });
                        }
                    }
                    else
                    {
                        result.Add(new StudentAttendance
                        {
                            Id = students[i].Id,
                            FullName = students[i].FullName,
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

        public async Task<List<Occupation>> GetMarks(int idGroup)
        {
            try
            {
                var marks = await database.QueryAsync<Occupation>(
                    "SELECT id_occup, is_checked, is_blocked FROM Occupations WHERE id_group = ? ORDER BY id_occup",
                    idGroup
                );
                return marks.Count != 0 ? marks : null;
            }
            catch (SQLiteException ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public async Task<List<Occupation>> GetUnblockedOccupations(int idGroup)
        {
            try
            {
                var occupations = await database.QueryAsync<Occupation>(
                    "SELECT id_occup FROM Occupations WHERE (is_checked = 0 OR is_blocked = 0) AND is_sync = 0 AND id_group = ?",
                    idGroup
                );
                return occupations.Count != 0 ? occupations : null;
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

        public async Task SetMarks(List<Mark> marks, int idGroup)
        {
            try
            {
                var cache = await database.QueryAsync<Occupation>(
                    "SELECT * FROM Occupations WHERE id_group = ? ORDER BY id_occup",
                    idGroup
                );

                for (int i = 0; i < cache.Count; i++)
                {
                    cache[i].IsChecked = marks[i].IsChecked;
                    cache[i].IsBlocked = marks[i].IsBlocked;
                }
                await database.UpdateAllAsync(cache);
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

        public async Task SetSyncFlag(int idOccupation, int idGroup)
        {
            try
            {
                var list = await database.QueryAsync<Occupation>(
                    "SELECT * FROM Occupations WHERE id_occup = ? AND id_group = ?",
                    idOccupation, idGroup
                );
                list[0].IsSync = true;
                await database.UpdateAllAsync(list);
            }
            catch (SQLiteException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public async Task<List<Occupation>> GetUnsyncOccupations()
        {
            try
            {
                var unsyncOccups = await database.QueryAsync<Occupation>(
                    "SELECT * FROM Occupations WHERE is_sync = 0"
                );

                return unsyncOccups.Count != 0 ? unsyncOccups : null;
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
                return await database.QueryAsync<StudentAbsent>(
                    "SELECT id_student FROM Attendance WHERE id_occup = ? AND id_group = ?",
                    idOccupation,
                    idGroup
                );
            }
            catch (SQLiteException ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public async Task SetAttendence(List<StudentSelect> refreshList, int idOccupation, int idGroup)
        {
            try
            {
                var selectedStudents = refreshList.FindAll(s => s.IsSelected.Equals(true));

                var absentStudents = new List<StudentAbsent>();
                foreach (var student in selectedStudents)
                {
                    absentStudents.Add(new StudentAbsent()
                    {
                        Id = student.Id,
                        IdOccupation = idOccupation,
                        IdGroup = idGroup
                    });
                }

                await database.QueryAsync<StudentAbsent>(
                    "DELETE FROM Attendance WHERE id_occup = ? AND id_group = ?",
                    idOccupation,
                    idGroup
                );
                await database.InsertAllAsync(absentStudents);

                var occupationsList = await database.QueryAsync<Occupation>(
                    "SELECT * FROM Occupations WHERE id_group = ? ORDER BY id_occup",
                    idGroup
                );

                foreach (var occup in occupationsList)
                {
                    if (occup.IdOccupation == idOccupation)
                    {
                        occup.IsChecked = true;
                        occup.IsSync = false;
                    }
                }

                await database.UpdateAllAsync(occupationsList);
            }
            catch (SQLiteException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public async Task SetAttendence(List<StudentAbsent> absentStudents, int idOccupation, int idGroup)
        {
            try
            {
                await database.QueryAsync<StudentAbsent>(
                    "DELETE FROM Attendance WHERE id_occup = ? AND id_group = ?",
                    idOccupation,
                    idGroup
                );

                if (absentStudents != null)
                {
                    await database.InsertAllAsync(absentStudents);
                }
            }
            catch (SQLiteException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public async Task DeleteAttendance(int idOccupation, int idGroup)
        {
            try
            {
                await database.QueryAsync<StudentAbsent>(
                    "DELETE FROM Attendance WHERE id_occup = ? AND id_group = ?",
                    idOccupation,
                    idGroup
                );

                var occupationsList = await database.QueryAsync<Occupation>(
                    "SELECT * FROM Occupations WHERE id_occup = ? AND id_group = ?",
                    idOccupation,
                    idGroup
                );
                occupationsList[0].IsChecked = false;

                await database.QueryAsync<Occupation>(
                    "DELETE FROM Occupations WHERE id_occup = ? AND id_group = ?",
                    idOccupation,
                    idGroup
                );
                await database.InsertAllAsync(occupationsList);
            }
            catch (SQLiteException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public async Task<List<StudentSelect>> GetStudents(int idGroup)
        {
            try
            {
                return await database.QueryAsync<StudentSelect>("SELECT id_student, fullname FROM Students WHERE id_group =?", idGroup);
            }
            catch (SQLiteException ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }
    }
}
