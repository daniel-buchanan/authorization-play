namespace authorization_play.Core
{
    public abstract class EqualityBase<T>
        where T: class
    {
        public abstract override int GetHashCode();

        public override bool Equals(object obj) => this.Equals(obj as T);

        public abstract bool Equals(T resource);
    }
}
