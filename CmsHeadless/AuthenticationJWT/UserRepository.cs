using CmsHeadless.Models;

namespace CmsHeadless.AuthenticationJWT
{
    public class UserRepository : IUserRepository
    {
        private readonly List<UserDTO> users = new List<UserDTO>();
        private readonly CmsHeadlessDbContext _contextDb;

        public UserRepository(CmsHeadlessDbContext contextDb)
        {
            _contextDb = contextDb;
            IQueryable<CmsUser> query = from CmsUser in _contextDb.CmsUser select CmsUser;
            for (int i = 0; i < query.Count(); i++)
            {
                users.Add(new UserDTO
                {
                    UserName = query.Select(x => x.UserName).ToList()[i],
                    Password = query.Select(x => x.PasswordHash).ToList()[i],
                    Role = "User"
                });
            }
        }
        public UserDTO GetUser(UserModel userModel)
        {
            return users.Where(x => x.UserName.ToLower() == userModel.UserName.ToLower() && x.Password == userModel.Password).FirstOrDefault();
        }
    }

}
