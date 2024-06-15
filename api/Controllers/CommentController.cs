using System.Linq;
using System.Threading.Tasks;
using api.DTOs.Comment;
using api.Interfaces;
using api.Mappers;
using api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IStockRepository _stockRepository;
        private readonly ILogger<CommentController> _logger;

        public CommentController(ICommentRepository commentRepository, IStockRepository stockRepository, ILogger<CommentController> logger)
        {
            _commentRepository = commentRepository;
            _stockRepository = stockRepository;
            _logger = logger;
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAllAsync()
        {
            var comments = await _commentRepository.GetAllAsync();
            var commentDtos = comments.Select(c => c.ToCommentDto()).ToList();
            return Ok(commentDtos);
        }

        [HttpGet("GetById/{id}")]
        public async Task<IActionResult> GetByIdAsync([FromRoute] int id)
        {
            var comment = await _commentRepository.GetByIdAsync(id);
            if (comment == null)
            {
                return NotFound();
            }
            return Ok(comment.ToCommentDto());
        }

        [HttpPost("Create/{stockId}")]
        public async Task<IActionResult> Create([FromRoute] int stockId, [FromBody] CommentCreateDto commentDto)
        {
            if (!await _stockRepository.StockExist(stockId))
            {
                return BadRequest("Stock doesn't exist!");
            }

            var commentModel = commentDto.ToComment(stockId);
            await _commentRepository.CreateAsync(commentModel);

            // Logging for debugging
            _logger.LogInformation($"Comment created with ID: {commentModel.Id}");
            _logger.LogInformation($"CreatedAtAction - Action: GetByIdAsync, Controller: Comment, ID: {commentModel.Id}");

            // Explicitly specify route template
            return CreatedAtAction(nameof(GetByIdAsync), new { id = commentModel.Id }, commentModel.ToCommentDto());
        }
    }
}
