namespace AlchemyLub.ToolBox.Exceptions;

/// <summary>
/// Корневая модель для всех пользовательских исключений.
/// </summary>
public class GeneralException : Exception
{
    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="GeneralException"/> с указанным сообщением об ошибке
    /// </summary>
    /// <param name="message">Сообщение, описывающее ошибку</param>
    protected GeneralException(string message) : base(message)
    {
    }

    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="GeneralException"/> с указанным сообщением об ошибке
    /// и ссылкой на внутреннее исключение, которое является причиной текущего
    /// </summary>
    /// <param name="message">Сообщение, описывающее ошибку</param>
    /// <param name="innerException">Исключение, которое является причиной текущего исключения</param>
    protected GeneralException(string message, Exception innerException) : base(message, innerException)
    {
    }

    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="GeneralException"/>
    /// </summary>
    protected GeneralException()
    {
    }
}
