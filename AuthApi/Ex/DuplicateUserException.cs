using System;

namespace AuthApi.Ex
{
    public class DuplicateUserException : Exception
    {
        public DuplicateUserException(string reason) : base(reason)
        {
            
        }   
    }
}