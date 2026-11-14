namespace ZenithSky_Proyecto.Common;

/// Excepciones de negocio: la capa Services las lanza y el GlobalExceptionHandler las traduce a HTTP.
public abstract class AppException(string message) : Exception(message);

/// HTTP 404
public class NotFoundException(string message) : AppException(message);

/// HTTP 409
public class ConflictException(string message) : AppException(message);

/// HTTP 400
public class BadRequestException(string message) : AppException(message);
