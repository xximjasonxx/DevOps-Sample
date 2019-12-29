namespace UserApi.Framework.Binding
{
    public class UserTokenResult
    {
        public TokenState TokenState { get; set; }
    
        public string Username { get; set; }
    }

    public enum TokenState
    {
        Valid,
        Empty,
        Invalid
    }
}