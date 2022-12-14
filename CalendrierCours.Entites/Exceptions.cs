namespace CalendrierCours.Entites
{
    public class InvalidDepotException : Exception
    {
        public InvalidDepotException()
        { }
        public InvalidDepotException(string p_message) : base(p_message)
        { }
        public InvalidDepotException(string p_message, Exception p_innerException)
            : base(p_message, p_innerException)
        { }
    }
}
