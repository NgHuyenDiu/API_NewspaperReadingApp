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
    public class ArticlesController : ControllerBase
    {
        private NewspaperReadingAppContext _db;
        public ArticlesController(NewspaperReadingAppContext db)
        {
            _db = db;
        }

        [HttpGet]
        public IActionResult getAll()
        {
            var data = ArticlesDAO.getAll();
            List<Article> list = new List<Article>();
            for (int i = 0; i < data.Rows.Count; i++)
            {
                Article art = new Article();
                art.IdArticles = Int32.Parse(data.Rows[i]["id_articles"].ToString());
                art.Title = data.Rows[i]["title"].ToString();
                art.ContentArticles = data.Rows[i]["content_articles"].ToString();
                art.IdUser = Int32.Parse(data.Rows[i]["id_user"].ToString());
                art.Status= data.Rows[i]["status"].ToString();
                art.DateSubmitted = Convert.ToDateTime(data.Rows[i]["date_submitted"].ToString());
                art.Image = data.Rows[i]["image"].ToString();
                art.Views = Int32.Parse(data.Rows[i]["views"].ToString());
                art.TrangThaiXoa = Int32.Parse(data.Rows[i]["trangThaiXoa"].ToString());

                list.Add(art);
            }
            return Ok(new { result = true, data = list });
        }

        [HttpGet]
        [Route("getTopView")]
        public IActionResult getTopView()
        {
            var data = ArticlesDAO.getTopView();
            List<Article> list = new List<Article>();
            for (int i = 0; i < data.Rows.Count; i++)
            {
                Article art = new Article();
                art.IdArticles = Int32.Parse(data.Rows[i]["id_articles"].ToString());
                art.Title = data.Rows[i]["title"].ToString();
                art.ContentArticles = data.Rows[i]["content_articles"].ToString();
                art.IdUser = Int32.Parse(data.Rows[i]["id_user"].ToString());
                art.Status = data.Rows[i]["status"].ToString();
                art.DateSubmitted = Convert.ToDateTime(data.Rows[i]["date_submitted"].ToString());
                art.Image = data.Rows[i]["image"].ToString();
                art.Views = Int32.Parse(data.Rows[i]["views"].ToString());
                art.TrangThaiXoa = Int32.Parse(data.Rows[i]["trangThaiXoa"].ToString());

                list.Add(art);
            }
            return Ok(new { result = true, data = list });
        }

        [HttpGet("get/{id}")]
        public IActionResult getByID(int id)
        {
            try
            {
                // linkQ[Object] query
                var dt = _db.Articles.SingleOrDefault(dt => dt.IdArticles == id);
                if (dt == null)
                {
                    return Ok(new { result = false, message = "Không tìm thấy bài viết" });
                }
                return Ok(new { result = true, data = dt });
            }
            catch
            {
                return Ok(new { result = false, message = "Truy xuất thông tin bài viết theo id thất bại" });
            }

        }

        [HttpPost]
        [Route ("create")]
        public IActionResult create(ArticlesModel model)
        {
            var useTemp = _db.Users.SingleOrDefault(user => user.IdUser == model.IdUser && user.Role == 1);
            if (useTemp != null)
            {
                Article art = new Article();
                art.IdArticles = ArticlesDAO.taoMa();
                art.Title = model.Title;
                art.ContentArticles = model.ContentArticles;
                art.IdUser = model.IdUser;
                art.Status = model.Status;
                art.Image = model.Image;
                art.Views = 0;
                art.TrangThaiXoa = 0;
                _db.Articles.Add(art);
                _db.SaveChanges();
                return Ok(new { result = true, message = "Thêm bài viết thành công" });
            }
            return Ok(new { result = true, message = "User không phải tác giả" });
        }

        [HttpDelete]
        public IActionResult delete(int id_art)
        {
            try
            {
                // linkQ[Object] query
                var art = _db.Articles.SingleOrDefault(ar => ar.IdArticles == id_art);
                if (art == null)
                {
                    return Ok(new { result = false, message = "Không tìm thấy bài viết" });
                }

                // delete

                art.TrangThaiXoa = 1;
                _db.SaveChanges();
                return Ok(new { result = true, message = "Đánh dấu Xoá bài viết thành công" });
            }
            catch
            {
                return Ok(new { result = false, message = "Đánh dấu xoá bài viết thất bại" });
            }

        }

        [HttpPut("Edit/{id}")]
        public IActionResult edit(int id, ArticlesModelEdit model)
        {
            try
            {
                // linkQ[Object] query
                var art = _db.Articles.SingleOrDefault(art => art.IdArticles == id);
                if (art == null)
                {
                    return Ok(new { result = false, message = "Không tìm thấy thông tin bài viết" });
                }
                if (id != art.IdArticles)
                {
                    return Ok(new { result = false, message = "Id bài viết không thể sửa đổi" });
                }
                // update

                art.Title = model.Title;
                art.ContentArticles = model.ContentArticles;
                art.Status = model.Status;
                art.Image = model.Image;
                
                _db.SaveChanges();
                return Ok(new { result = true, message = "Chỉnh sửa thông tin bài viết thành công" });
            }
            catch
            {
                return Ok(new { result = false, message = "Chỉnh sửa bài viết thất bại" });
            }
        }

    }
}
