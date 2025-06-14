namespace pizzeria.Utils 
{
    public static class Validator
    {
        public static bool IsValidLogin(string login)
        {
            return
                login.Length >= 3 &&
                login.All(char.IsLetter);
        }

        public static bool IsValidPassword(string password)
        {
            return
                password.Length >= 6 &&
                !password.Any(char.IsWhiteSpace) &&
                password.Any(char.IsUpper) &&
                password.Any(char.IsLower) &&
                password.Any(char.IsDigit) &&
                password.Any("!@#$%^&*()_+-=[]{}|;':\",.<>?`~".Contains);
        }
        
        public static bool IsValidName(string name)
        {
            return name.Length <= 20;
        }
    }
}