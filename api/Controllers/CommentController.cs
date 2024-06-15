using System.Linq;
using System.Threading.Tasks;
using api.DTOs.Comment;
using api.Interfaces;
using api.Mappers;
using api.Models;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IStockRepository _stockRepository;

        public CommentController(ICommentRepository commentRepository, IStockRepository stockRepository)
        {
            _commentRepository = commentRepository;
            _stockRepository = stockRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var comments = await _commentRepository.GetAllAsync();
            var commentDtos = comments.Select(c => c.ToCommentDto()).ToList();
            return Ok(commentDtos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync([FromRoute] int id)
        {
            var comment = await _commentRepository.GetByIdAsync(id);
            if (comment == null)
            {
                return NotFound();
            }
            return Ok(comment.ToCommentDto());
        }

        [HttpPost("{stockId}")]
        public async Task<IActionResult> Create([FromRoute] int stockId, [FromBody] CommentCreateDto commentDto)
        {
            if (!await _stockRepository.StockExist(stockId))
            {
                return BadRequest("Stock doesn't exist!");
            }

            var commentModel = commentDto.ToComment(stockId);
            await _commentRepository.CreateAsync(commentModel);

            // Debugging - log the commentModel Id
            Console.WriteLine($"Created comment with ID: {commentModel.Id}");

            return CreatedAtAction(nameof(GetByIdAsync), "Comment", new { id = commentModel.Id }, commentModel.ToCommentDto());
        }

        
    }
}
