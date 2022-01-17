using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SeriLogDemo.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
            _logger.LogInformation("Index page is being loaded by {DevName} at {LogTime}", "Pula", DateTime.Now);
            _logger.LogError("Something went wrong here  by {DevName} at {LogTime}", "Pula", DateTime.Now);

            try
            {
                throw new Exception("Exception thrown at Index file.");
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "Something bad has happended. ");
            }
        }
    }
}
