using System;

namespace Mikaboshi.Locapos
{
    /// <summary>
    /// Locapos の認証に必要なトークンが見つからなかったときの例外を示します。
    /// </summary>
    [Serializable]
    public class TokenNotFoundException : Exception
    {
        public TokenNotFoundException() { }
        public TokenNotFoundException(string message) : base(message) { }
        public TokenNotFoundException(string message, Exception inner) : base(message, inner) { }
    }
}
