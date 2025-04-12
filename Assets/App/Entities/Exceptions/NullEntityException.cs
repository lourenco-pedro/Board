namespace App.Entities.Exceptions
{
    public class NullEntityException : System.Exception
    {
        public NullEntityException(string messsage) : base(messsage){}
    }
}