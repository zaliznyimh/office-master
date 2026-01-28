namespace OfficeMaster.ViewModels.Shared;

public class ApiResult
{
    public bool IsSuccess { get; }
    public string ErrorMessage { get; }

    private ApiResult(bool isSuccess, string errorMessage)
    {
        IsSuccess = isSuccess;
        ErrorMessage = errorMessage;
    }

    public static ApiResult Success()
        => new ApiResult(true, null!);

    public static ApiResult Failure(string errorMessage)
        => new ApiResult(false, errorMessage);
}