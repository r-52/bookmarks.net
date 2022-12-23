namespace Beartrail.Application.Common.Base;

public class Result<T>
{
    public bool IsSuccess { get; set; } = false;

    public T? Data { get; set; }
}
