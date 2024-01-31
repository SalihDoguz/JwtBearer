namespace JwtSecurity.Models
{
    public class UsersApi
    {
        public static List<UserApi> Users = new()
        {
            new UserApi {Id = 1, UserName ="Salih", Password="123456",Role="Yönetici"},
            new UserApi {Id = 2, UserName ="Çağan", Password="123456",Role="Çırak"},
            new UserApi {Id = 3, UserName = "Furkan",Password = "123456",Role = "İzolasyoncu" }
        };
    }
}
