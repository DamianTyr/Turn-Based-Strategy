namespace Saving
{
    public interface ISaveable
    {
        void CaptureState(string guid);
        
        void RestoreState(string guid);
    }
}