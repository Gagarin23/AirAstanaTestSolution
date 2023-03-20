namespace Application.Common.Interfaces;

/// <summary>
/// Контракт для получения аутентификационных данных пользователя.
/// </summary>
public interface IUserContext
{
    string Username { get; }
}
