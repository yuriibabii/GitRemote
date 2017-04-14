namespace GitRemote.DI
{
    public interface IMetricsHelper
    {
        double GetLabelWidth(string text, double maxWidth, double fontSize = 14);

        double GetLabelHeight(string text, double maxWidth, double fontSize = 14);
    }
}
