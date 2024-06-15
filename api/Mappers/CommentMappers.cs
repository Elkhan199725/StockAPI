using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using api.DTOs.Comment;
using api.Models;

namespace api.Mappers
{
public static class CommentMappers
    {
        public static CommentDTO ToCommentDto(this Comment comment)
        {
            return new CommentDTO
            {
                Id = comment.Id,
                Title = comment.Title,
                Content = comment.Content,
                CreatedOn = comment.CreatedOn,
                StockId = comment.StockId
            };
        }

        public static Comment ToComment(this CommentCreateDto commentCreateDto, int stockId)
        {
            return new Comment
            {
                Title = commentCreateDto.Title,
                Content = commentCreateDto.Content,
                StockId = stockId
            };
        }

        public static void UpdateComment(this CommentUpdateDto commentUpdateDto, Comment comment)
        {
            comment.Title = commentUpdateDto.Title;
            comment.Content = commentUpdateDto.Content;
            comment.StockId = commentUpdateDto.StockId;
        }
    }
}