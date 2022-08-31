using AM.OneTimeMessages.Core;
using AM.OneTimeMessages.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace AM.OneTimeMessages.Web.Controllers
{
    public class MessageController : Controller
    {
        public const string Name = "Message";
        public const string SaveAction = nameof(Save);
        public const string LoadAction = nameof(Load);

        private readonly IMessageStorage _storage;
        private readonly ILogger<MessageController> _logger;

        public MessageController(IMessageStorage storage, ILogger<MessageController> logger)
        {
            _storage = storage;
            _logger = logger;
        }

        [HttpGet("{id?}")]
        public IActionResult Index([FromRoute] string id)
        {
            return View(model: id);
        }

        [HttpPost("message/save")]
        public async Task<IActionResult> Save([FromForm] MessageModel model)
        {
            if (string.IsNullOrWhiteSpace(model.Id) || string.IsNullOrWhiteSpace(model.Message))
            {
                return BadRequest();
            }

            await _storage.PushAsync(model.Id, model.Message, model.TimeStamp);

            return Ok();
        }

        [HttpGet("message/load/{id}")]
        public async Task<IActionResult> Load([FromRoute] string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return BadRequest();
            }

            var message = await _storage.PullAsync(id);
            if (string.IsNullOrEmpty(message))
            {
                return NotFound();
            }

            return Json(new MessageModel { Id = id, Message = message });
        }

        [HttpGet("message/error")]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}