namespace discipline.centre.shared.infrastructure.Modules.Types;

internal sealed record ModuleRequestRegistration(Type RequestType, Type ResponseType,
    Func<object, Task<object?>> Action);