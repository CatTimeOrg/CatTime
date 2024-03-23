namespace CatTime.Backend.Exceptions
{
    public class SomethingFishyException : Exception
    {
        public SomethingFishyException() 
            : base("Meow??? Something fishy is going on!")
        {
        }
    }
}
