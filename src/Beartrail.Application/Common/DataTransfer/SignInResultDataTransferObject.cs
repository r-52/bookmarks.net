namespace Beartrail.Application.Common.DataTransfer;

public class SignInResultDataTransferObject
{
    public string? Email { get; set; }

    public bool IsSuccess { get; set; } = false;
}
