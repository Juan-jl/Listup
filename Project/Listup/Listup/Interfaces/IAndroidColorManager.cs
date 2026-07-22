namespace Listup.Interfaces
{
    public interface IAndroidColorManager
    {
        void SetStatusBarColor(string hexColor, bool blackIcons);
        void SetNavigationBarColor(string hexNavigationBarColor, bool blackIcons);
    }
}
