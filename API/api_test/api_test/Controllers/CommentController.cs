using api_test.DAO;
using api_test.EF;
using api_test.Model;
using Microsoft.AspNetCore.Authorization;
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
            List<UserView> listUserView = new List<UserView>();
            for (int i = 0; i < data.Rows.Count; i++)
            {
                Comment cmt = new Comment();
                cmt.IdComment = Int32.Parse(data.Rows[i]["id_comment"].ToString());
                int id = Int32.Parse(data.Rows[i]["id_user"].ToString());
                cmt.IdUser = id;
                cmt.IdArticles = Int32.Parse(data.Rows[i]["id_articles"].ToString());
                cmt.ContentComment = data.Rows[i]["content_comment"].ToString();
                list.Add(cmt);

                var user = _db.Users.SingleOrDefault(user => user.IdUser == id);
                UserView userView = new UserView();
                userView.IdUser = user.IdUser;
                userView.Name = user.Name;
                userView.Phone = user.Phone;
                userView.Email = user.Email;
                userView.Avata = user.Avata;
                userView.Gender = user.Gender;
                userView.Username = user.Username;
                userView.Role = user.Role;
                listUserView.Add(userView);
            }
            return Ok(new { result = true, data = list , listuser = listUserView });
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
        [Authorize]
        public IActionResult Create(CommentModel model)
        {
            Comment com = new Comment();
            com.IdComment = CommentDAO.taoMa();
            com.IdUser = model.IdUser;
            com.IdArticles = model.IdArticles;
            com.ContentComment = model.ContentComment;
            _db.Comments.Add(com);
            _db.SaveChanges();
            return Ok(new { result = true, message = "Thêm bình luận thành công", comment = com });
        }

       
        [HttpDelete("delete")]
        [Authorize]
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
        [Authorize]
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
