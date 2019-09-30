namespace CustomerApi.Controllers
{
    public class ErrorDetails
    {
        public string errorDescription { get; set; }

        public ErrorDetails()
        {
        }

        public ErrorDetails(string errorDescription)
        {
            this.errorDescription = errorDescription;
        }
    }
}
