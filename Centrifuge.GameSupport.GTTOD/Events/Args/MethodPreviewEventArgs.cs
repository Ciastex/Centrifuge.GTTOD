namespace Centrifuge.GTTOD.Events.Args
{
    public class MethodPreviewEventArgs<T> : TypeInstanceEventArgs<T>
    {
        public bool Cancel { get; set; }
        public MethodPreviewEventArgs(T instance) : base(instance) { }
    }
}
