namespace prescriptionSystemApi.model
{
    public class UserConstants
    {
        public static List<User> Users = new List<User>()
        {
            new User() { Id = 1,Name = "şükrü",Username = "doctor1",Password = "pass", Role="doctor"},
            new User() { Id = 2,Name = "x",Username = "doctor2",Password = "pass", Role="doctor"},
            new User() { Id = 3,Name = "hande",Username = "doctor3",Password = "pass", Role="doctor"},

            new User() { Id = 4,Name = "DURU Eczane",Username = "phar1",Password = "pass",Email="handehkeskin@hotmail.com", Role="pharmacy"},
            new User() { Id = 5,Name = "p2",Username = "phar2",Password = "pass",Email="nomail@hotmail.com", Role="pharmacy"},
            new User() { Id = 6,Name = "p3",Username = "phar3",Password = "pass", Email="nomail2@hotmail.com",Role="pharmacy"}
        };
    }
}
