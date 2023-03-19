namespace Domain.Constants;

public static class ValidationMessages
{
    public const string ValidationFailure = "Произошла одна или несколько ошибок валидации.";
    public const string DefaultValue = "Задано значение по умолчанию.";
    public const string InvalidId = "Некоректный идентификатор";
    public const string DepartureGreaterOrEqualsThanArrival = "Время вылета не может быть больше или равным времени прилёта.";
    public const string OriginEqualsDestination = "Место отбытия не может совпадать с местом прибытия.";
    public const string DepartureOrArrivalWhenDelay = "Для изменения статуса на задержку, требуется указать смещение времени для отбытия или прибытия.";
}

public static class AuthenticationMessages
{
    public const string PasswordOrUserIsInvalid = "Некоректный пользователь или пароль.";
}
