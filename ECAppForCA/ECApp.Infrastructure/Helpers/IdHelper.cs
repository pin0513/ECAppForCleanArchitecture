namespace ECApp.Infrastructure.Helpers;

public class IdHelper
{
    public IdHelper()
    {
    }

    public Guid GetId()
    {
        return Guid.NewGuid();
    }
}