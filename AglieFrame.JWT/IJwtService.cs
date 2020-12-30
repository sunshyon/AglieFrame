using System;

namespace AglieFrame.JWT
{
    public interface IJwtService
    {
        string GetToken(string userName);
    }
}
