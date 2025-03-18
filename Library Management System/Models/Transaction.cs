namespace Library_Management_System.Models
{
    public class Transaction
    {
        public int TransactionId { get; set; }
        public string BorrowedBook { get; set; }
        public string Patron { get; set; }
        public DateTime TransactionDate { get; set; }
    }
}
