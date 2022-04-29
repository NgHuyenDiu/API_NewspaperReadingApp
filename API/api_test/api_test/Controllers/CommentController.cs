using api_test.DAO;
using api_test.EF;
using api_test.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api_test.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentController : ControllerBase
    {

        private NewspaperReadingAppContext _db;
        public CommentController(NewspaperReadingAppContext db)
        {
            _db = db;
        }

        [HttpGet]
        public IActionResult getAll(int id_articles)
        {
            var data = CommentDAO.getAll(id_articles);
            List<Comment> list = new List<Comment>();
            for (int i = 0; i < data.Rows.Count; i++)
            {
                Comment cmt = new Comment();
                cmt.IdComment = Int32.Parse(data.Rows[i]["id_comment"].ToString());
                cmt.IdUser = Int32.Parse(data.Rows[i]["id_user"].ToString());
                cmt.IdArticles = Int32.Parse(data.Rows[i]["id_articles"].ToString());
                cmt.ContentComment = data.Rows[i]["content_comment"].ToString();
                list.Add(cmt);
                
            }
            return Ok(new { result = true, data = list });
        }

        [HttpGet("get/{id}")]
        public IActionResult getByID(int id)
        {
            try
            {
                // linkQ[Object] query
                var dt = _db.Comments.SingleOrDefault(dt => dt.IdComment == id);
                if (dt == null)
                {
                    return Ok(new { result = false, message = "Không tìm thấy bình luận" });
                }
                return Ok(new { result = true, data = dt });
            }
            catch
            {
                return Ok(new { result = false, message = "Truy xuất thông tin bình luận theo id thất bại" });
            }

        }

        [HttpPost]
        [Route("create")]
        public IActionResult Create(CommentModel model)
        {
            Comment cat = new Comment();
            cat.IdComment = CommentDAO.taoMa();
            cat.IdUser = model.IdUser;
            cat.IdArticles = model.IdArticles;
            cat.ContentComment = model.ContentComment;
            _db.Comments.Add(cat);
            _db.SaveChanges();
            return Ok(new { result = true, message = "Thêm bình luận thành công" });
        }

        [HttpDelete("delete")]
        public IActionResult delete(int id_cmt)
        {
            try
            {
                // linkQ[Object] query
                var cate = _db.Comments.SingleOrDefault(cat => cat.IdComment == id_cmt);
                if (cate == null)
                {
                    return Ok(new { result = false, message = "Không tìm thấy thông tin bình luận" });
                }

                // delete

                _db.Comments.Remove(cate);
                _db.SaveChanges();
                return Ok(new { result = true, message = "Xoá bình luận thành công" });
            }
            catch
            {
                return Ok(new { result = false, message = "Xoá bình luận thất bại" });
            }

        }

        [HttpPut("Edit/{id}")]
        public IActionResult edit(int id, CommentModelEdit model)
        {
            try
            {
                // linkQ[Object] query
                var cate = _db.Comments.SingleOrDefault(cat => cat.IdComment == id);
                if (cate == null)
                {
                    return Ok(new { result = false, message = "Không tìm thấy thông tin bình luận" });
                }
                if (id != cate.IdComment)
                {
                    return Ok(new { result = false, message = "Id comment không thể sửa đổi" });
                }

                // update
                cate.ContentComment = model.ContentComment;

                _db.SaveChanges();
                return Ok(new { result = true, message = "Chỉnh sửa bình luận thành công" });
            }
            catch
            {
                return Ok(new { result = false, message = "Chỉnh sửa thất bại" });
            }
        }
    }
}
