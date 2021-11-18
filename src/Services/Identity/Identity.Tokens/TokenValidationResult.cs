namespace Identity.Tokens
{
    public enum TokenValidationResult
    {
        Success,
        TokenExpired,
        EncryptionKeyNotFound,
        InvalidSignature,
        Unknown,
    }
}
