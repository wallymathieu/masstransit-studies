using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MassTransitStudies.Service.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IRepository repository;
        public IndexModel(IRepository repository) => this.repository = repository;
        public ReceivedMessages Messages { get; private set; }
        public void OnGet() => this.Messages = this.repository.List();
    }
}
