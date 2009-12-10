namespace JelloScrum.Login.Services
{
    using Castle.ActiveRecord;
    using Model;
    using QueryObject;
    using Wachtwoord;

    public class LoginService<T> : ILoginService<T> where T : class, IUser
    {

        #region ILoginService<T> Members

        public bool CheckPassWord(string userName, string passWord)
        {
            return GetUser(userName, passWord) != null;
        }

        public bool IsAllowedToLogin(string userName, string passWord)
        {
            T user = GetUser(userName, passWord);

            return user != null && user.IsActive;
        }

        public T GetUser(long id)
        {
            return ActiveRecordMediator<T>.FindByPrimaryKey(id, false);
        }

        public T GetUser(string userName, string passWord)
        {
            T user = ActiveRecordMediator<T>.FindOne(new UsersWithUserNameQuery<T>().GetQuery(userName));

            if (user == null)
                return null;
            //is the password equal to the encrypted version of the password?
            return user.PassWord == PassWordHelper.EncryptPassWord(passWord, user.Salt) ? user : null;
        }

        public bool IsUserNameValid(string userName, T user)
        {
            T foundUser = ActiveRecordMediator<T>.FindOne(new UsersWithUserNameQuery<T>().GetQuery(userName));
            return foundUser == null || foundUser.Equals(user); 
        }

        #endregion
    }
}