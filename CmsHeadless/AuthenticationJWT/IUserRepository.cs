namespace CmsHeadless.AuthenticationJWT
{
    public interface IUserRepository
    {
        UserDTO GetUser(UserModel userModel);
    }

}
