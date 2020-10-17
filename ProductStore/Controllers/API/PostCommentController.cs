using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ProductStore.Data;
using ProductStore.Models;

namespace ProductStore.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostCommentController : ControllerBase
    {
        private readonly ILogger<HomeController> _logger;
        private AuthDbContext _context;

        public PostCommentController(AuthDbContext context, ILogger<HomeController> logger)
        {
            _logger = logger;
            _context = context;
        }

        [HttpGet]
        [Route("GetAll")]
        public IEnumerable<Comment> GetAllComments()
        {
            return  _context.Comment.Include(m => m.CommentProduct).Include(m => m.CommentUser).ToList();
        }

    }
}
