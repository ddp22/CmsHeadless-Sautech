using CmsHeadlessApi.Models;

namespace CmsHeadlessApi.Classes.UserRepository
{
    public interface IUserRepository
    {
        UserDTO GetUser(UserModel userMode);
    }
}
