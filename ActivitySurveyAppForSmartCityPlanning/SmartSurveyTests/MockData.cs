using System;
namespace SmartSurveyTests
{
    public class MockData
    {
        //public static List<Users> MockData()
        //{

        //    return new List<Users> {

        //            new Users (username:"someusername1",password:"password")
        //        };
        //}
    }
}

public class Users
{

    private string username { get; set; }
    private string password { get; set; }

    public Users() { }

    public Users(string username, string password)
    {
        this.username = username;
        this.password = password;
    }

}

