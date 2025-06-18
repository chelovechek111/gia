using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DemoEmelyanov.Models;

namespace DemoEmelyanov.Models
{
    public class User
    {
        public int Id { get; set; }               // Уникальный ID
        public string Login { get; set; }         // Логин
        public string Password { get; set; }      // Пароль
        public string Role { get; set; }          // Роль (например: "Администратор", "Пользователь")
        public bool IsBlocked { get; set; }       // Заблокирован ли пользователь
        public int FailedAttempts { get; set; } = 3;  // Кол-во неверных попыток входа
        public DateTime LastLogin { get; set; } = DateTime.MinValue;  // Последний вход
        public bool RequirePasswordChange { get; set; } // Требуется ли смена пароля

    }
}
public class AppUser
{
    public string Login { get; set; }
    public string Password { get; set; }
    public string Role { get; set; } // "admin" или "user"

    public int FailedAttempts { get; set; } = 0;
    public bool IsBlocked { get; set; } = false;
    public DateTime LastLogin { get; set; } = DateTime.MinValue;
}


public static class UserDatabase
{
    private static List<AppUser> users = new List<AppUser>
    {
        new AppUser { Login = "admin", Password = "1234", Role = "admin" },
        new AppUser { Login = "anton", Password = "12345", Role = "user", IsBlocked = true},
        new AppUser { Login = "guest", Password = "0000", Role = "user" },
        new AppUser { Login = "admin2", Password = "999", Role = "admin" }
    };

    public static List<AppUser> GetAllUsers() => users;

    public static AppUser FindUser(string login) => users.FirstOrDefault(u => u.Login == login);
}
