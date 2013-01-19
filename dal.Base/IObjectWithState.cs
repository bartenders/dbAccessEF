namespace dal.Core
{
    public interface IObjectWithState
    {
        State State { get; set; }
    }
}
