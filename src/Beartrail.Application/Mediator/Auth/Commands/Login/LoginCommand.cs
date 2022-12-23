namespace Beartrail.Application.Mediator.Auth.Commands.Login;

public class LoginCommand : IRequest<Result<LoginResponseDataTransferObject>>
{
    public string Email { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;
}

public class LoginCommandHandler : IRequestHandler<LoginCommand, Result<LoginResponseDataTransferObject>>
{
    private readonly IJwtTokenGeneratorService _jwtTokenGeneratorService;
    private readonly IUserSignInManager _userSignInManager;
    private readonly ILogger<LoginCommandHandler> _logger;

    public LoginCommandHandler(IJwtTokenGeneratorService jwtTokenGeneratorService, IUserSignInManager userSignInManager, ILogger<LoginCommandHandler> logger)
    {
        _userSignInManager = userSignInManager;
        _logger = logger;
        _jwtTokenGeneratorService = jwtTokenGeneratorService;
    }

    public async Task<Result<LoginResponseDataTransferObject>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"[{this.ToString()}] Trying to sign in....");
        var result = await _userSignInManager.SignInAsync(new LogInUserDataTransferObject
        {
            Email = request.Email,
            Password = request.Password,
        });
        if (result.IsSuccess is false)
        {
            _logger.LogInformation($"[{this.ToString()} Could not sign in for email {request.Email}]");
            return new()
            {
                Data = null,
                IsSuccess = false,
            };
        }

        _logger.LogInformation($"[{this.ToString()}] sign in was successful");
        string? token = await _jwtTokenGeneratorService.GenerateTokenAsync(request.Email);
        return new()
        {
            Data = new()
            {
                Token = token,
            },
            IsSuccess = token?.Length > 0
        };
    }
}
