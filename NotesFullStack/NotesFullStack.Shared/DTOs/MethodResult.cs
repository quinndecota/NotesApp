namespace NotesFullStack.Shared.DTOs
{
    public record MethodResult(bool IsSuccess, string? Error)
    {
        public static MethodResult Ok() => new(true ,null);
        public static MethodResult Fail(string error) => new(false, error);
    }

    public record MethodResult<TData>(bool IsSuccess, TData Data, string? Error)
    {
        public static MethodResult<TData> Ok(TData data) => new(true, data, null);
        public static MethodResult<TData> Fail(string error) => new(false, default!, error);
    }
}
