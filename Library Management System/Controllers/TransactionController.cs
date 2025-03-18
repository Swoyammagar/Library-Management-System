using Library_Management_System.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Library_Management_System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        public static List<Book> BookList = new List<Book> 
        { 
            new Book { Author = "ABC", Title = "AaBbCc", IsAvailable = true, ISBN = "123-122-3123-123" }, 
            new Book { Author = "CDE", Title = "CcDdEe", IsAvailable = true, ISBN = "534-435-5434-543" }, 
            new Book { Author = "AAA", Title = "AaAaAa", IsAvailable = true, ISBN = "543-654-8765-768" }, 
            new Book { Author = "XYZ", Title = "XxYyZz", IsAvailable = true, ISBN = "122-343-4234-424" } 
        };

        public static List<Patron> PatronList = new List<Patron>();

        public static List<Transaction> TransactionList = new List<Transaction>();

        [HttpGet("GetBooks")]
        public IActionResult GetBooks()
        {            
            return Ok(BookList);
        }

        [HttpPost("CreateCustomer")]
        public IActionResult CreateCustomer(Patron customer)
        {
            if (customer != null)
            {
                PatronList.Add(customer);
                return Ok("Customer created.");
            }
            return BadRequest("Unable to create customer. Please try again.");
        }

        [HttpGet("GetCustomerList")]
        public IActionResult GetCustomerList() 
        {
            return Ok(PatronList);          
        }

        [HttpPost("BorrowBook")]
        public IActionResult BorrowBook(Transaction requestBody)
        {
            if (!CheckBook(requestBody.BorrowedBook))
            {
                return BadRequest("Book is not available.");
            }
            if (!CheckPatron(requestBody.Patron))
            {
                return BadRequest("Customer invalid.");
            }
            TransactionList.Add(requestBody);
            BookList.Where(x => x.ISBN == requestBody.BorrowedBook).FirstOrDefault().IsAvailable = false;
            PatronList.Where(x => x.Name == requestBody.Patron).FirstOrDefault().NumOfBooksBorrowed += 1;
            return Ok("Success");
        }

        [HttpPost("ReturnBook")]
        public IActionResult ReturnBook(Transaction requestBody)
        {
            if (!CheckPatron(requestBody.Patron))
            {
                return BadRequest("Customer invalid.");
            }
            TransactionList.Add(requestBody);
            BookList.Where(x => x.ISBN == requestBody.BorrowedBook).FirstOrDefault().IsAvailable = true;
            PatronList.Where(x => x.Name == requestBody.Patron).FirstOrDefault().NumOfBooksBorrowed -= 1;
            return Ok("Success");
        }

        private bool CheckBook(string ISBN)
        {
            return BookList.Where(x => x.ISBN == ISBN && x.IsAvailable == true).Count() > 0;            
        }

        private bool CheckPatron(string PatronName)
        {
            return PatronList.Where(x => x.Name == PatronName).Count() > 0;
        }
    }
}
