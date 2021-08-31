using InterviewApp.Shared;
using InterviewApp.Shared.Models;
using Microsoft.AspNetCore.Mvc;

namespace InterviewApp.Server.Controllers;

[Route("[controller]")]
[ApiController]
public class InterviewController : ControllerBase
{
    private readonly JsonDataHandler _dataHandler;
    private readonly ILogger<InterviewController> _logger;

    public InterviewController(ILogger<InterviewController> logger)
    {
        _dataHandler = new JsonDataHandler();
        _logger = logger;
    }

    [HttpGet]
    public List<InterviewQuestion> Get()
    {
        var data = _dataHandler.GetDataFromFile();

        if (data == null)
        {
            throw new ArgumentNullException(nameof(data));
        }

        return data;
    }
}
