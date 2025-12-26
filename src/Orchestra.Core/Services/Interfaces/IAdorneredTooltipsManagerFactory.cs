namespace Orchestra
{
    using System.Windows.Documents;

    public interface IAdorneredTooltipsManagerFactory
    {
        IAdorneredTooltipsManager Create(AdornerLayer adornerLayer);
    }
}
