namespace Identity.Domain
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
