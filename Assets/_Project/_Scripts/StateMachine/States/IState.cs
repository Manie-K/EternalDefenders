namespace EternalDefenders
{
    public interface IState
    {
        /// <summary>
        /// It is used for debug/editor purposes.
        /// </summary>
        string Name { get; }
        
        /// <summary>
        /// It is used for hash etc.
        /// </summary>
        string Id { get; }
        
        void OnEnter();
        void OnUpdate();
        void OnFixedUpdate();
        void OnExit();
    }
}
