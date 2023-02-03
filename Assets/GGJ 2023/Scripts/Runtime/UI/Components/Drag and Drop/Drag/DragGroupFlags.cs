namespace GGJRuntime
{
    [System.Flags]
    public enum DragGroupFlags
    {
        None    = 0,
        Object  = 1,
        Concept = 2,
        All = ~0
    }
}