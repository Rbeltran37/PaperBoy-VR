namespace Core.UpdateManager
{
    public interface IUpdateable
    {
        public bool IsValid();
        public void ManagedUpdate();
        public void ManagedFixedUpdate();
        public void ManagedLateUpdate();
        public void SmartUpdate();
    }
}
