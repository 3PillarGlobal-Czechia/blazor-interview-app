
using InterviewApp.Shared.Models;
using Microsoft.AspNetCore.Components;

namespace InterviewApp.Client.Components.ExpansionList.ExpansionListItem.ExpansionListItemContent;

public partial class AppExpansionListItemContent
{
    [Parameter]
    public InterviewQuestion? Question { get; set; }
}
