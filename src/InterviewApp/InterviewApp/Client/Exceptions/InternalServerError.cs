
namespace InterviewApp.Client.Exceptions;
public class InternalServerError : Exception
{
    public InternalServerError() : base() { }

    public InternalServerError(string msg) : base(msg) { }
}