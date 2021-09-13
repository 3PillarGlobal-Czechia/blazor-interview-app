
using InterviewApp.Shared.Models;
using Microsoft.AspNetCore.Components;

namespace InterviewApp.Client.Components.ExpansionList.ExpansionListItem.ExpansionListItemContent;

public partial class AppExpansionListItemContent
{
    [Parameter]
    public InterviewQuestion? Question { get; set; }

    [Parameter]
    public EventCallback OnRatingChanged { get; set; }

    public async Task RatingChanged(int rating)
    {
        if (Question is null)
        {
            throw new InvalidOperationException(nameof(Question));
        }

        Question.Rating = rating;

        await OnRatingChanged.InvokeAsync();
    }
}
