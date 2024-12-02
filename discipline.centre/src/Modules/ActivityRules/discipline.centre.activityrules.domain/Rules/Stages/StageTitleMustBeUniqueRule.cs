using discipline.centre.activityrules.domain.Specifications;
using discipline.centre.shared.abstractions.SharedKernel;
using discipline.centre.shared.abstractions.SharedKernel.Exceptions;

namespace discipline.centre.activityrules.domain.Rules.Stages;

internal sealed class StageTitleMustBeUniqueRule(List<Stage>? stages, StageSpecification specification) : IBusinessRule
{
    public Exception Exception => new DomainException("ActivityRule.Stages.StageTitleMustBeUnique",
        "Stages must be unique.");
    public bool IsBroken()
        => stages is not null && stages.Exists(x => x.Title == specification.Title);
}