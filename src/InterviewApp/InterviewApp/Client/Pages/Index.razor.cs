
using InterviewApp.Shared.Models;

namespace InterviewApp.Client.Pages;

public partial class Index
{
    public string? SearchValue
    {
        get => _searchValue;
        set
        {
            _searchValue = value;
            StateHasChanged();
            FilterList();
        }
    }

    private Random random = new Random();

    private List<InterviewQuestion>? _all;
    private List<InterviewQuestion>? _current;
    private List<InterviewQuestion>? _currentFiltered;
    private List<int>? _currIndexList;
    private List<int>? _prevIndexList;

    private string? _searchValue;

    protected override async Task OnInitializedAsync()
    {
        _all = await InterviewService.GetInterviewQuestions();

        Randomize();
    }

    protected void Randomize()
    {
        if (_all == null)
        {
            throw new ArgumentNullException(nameof(_all));
        }

        _current = new List<InterviewQuestion>();
        _currentFiltered = new List<InterviewQuestion>();
        _currIndexList = new List<int>();

        for (int i = 0; i < 10; i++)
        {
            var index = random.Next(0, _all.Count);

            if (_currIndexList.Contains(index) || (_prevIndexList != null && _prevIndexList.Contains(index)))
            {
                i--;
            }
            else
            {
                _currIndexList.Add(index);
                _current.Add(_all[index]);
            }
        }

        _currentFiltered = _current;
    }

    protected void Reset()
    {
        _prevIndexList = _currIndexList;

        Randomize();
    }

    protected void FilterList()
    {
        if (_current ==  null)
        {
            throw new ArgumentNullException(nameof(_current));
        }

        var value = _searchValue?.Trim();

        if (string.IsNullOrEmpty(value))
        {
            _currentFiltered = _current;
        }
        else
        {
            _currentFiltered = _current.Where(x => (x.Content != null && x.Content.ToLower().Contains(value.ToLower())) ||
                                                   (x.Category != null && x.Category.ToLower().Contains(value.ToLower()))).ToList();
        }
    }
}
