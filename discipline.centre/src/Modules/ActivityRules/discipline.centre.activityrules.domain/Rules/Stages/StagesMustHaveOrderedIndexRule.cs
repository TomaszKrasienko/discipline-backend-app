using discipline.centre.activityrules.domain.Specifications;
using discipline.centre.shared.abstractions.SharedKernel;
using discipline.centre.shared.abstractions.SharedKernel.Exceptions;

namespace discipline.centre.activityrules.domain.Rules.Stages;

internal sealed class StagesMustHaveOrderedIndexRule(List<Stage>? stages, StageSpecification specification) : IBusinessRule
{
    public Exception Exception => new DomainException("ActivityRule.Stages.MustHaveOrderedIndex",
        "Provided stages have invalid indexes");

    public bool IsBroken()
    {
        if (stages is null || stages.Count < 1)
        {
            return specification.Index != 1;
        }
        return (stages!.Max(x => x.Index)! + 1) != specification.Index;
    }
}