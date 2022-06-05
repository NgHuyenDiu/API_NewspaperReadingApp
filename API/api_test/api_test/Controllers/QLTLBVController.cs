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
    public class QLTLBVController : ControllerBase
    {
        private NewspaperReadingAppContext _db;
        public QLTLBVController(NewspaperReadingAppContext db)
        {
            _db = db;
        }

        [HttpGet]
       
        public IActionResult getAll()
        {
            return Ok(new { result = true, data = _db.Qltlbvs });
        }

        
        [HttpPost]
        [Route("Create")]
        [Authorize]

        public IActionResult create(QLTLBVModel model)
        {
            try
            {
               
                var catTemp = _db.Categories.SingleOrDefault(cat => cat.IdCategory == model.IdCategory );
                if (catTemp != null)
                {
                    var artTemp = _db.Articles.SingleOrDefault(art => art.IdArticles == model.IdArticles);
                    if (artTemp != null)
                    {
                        Qltlbv ql = new Qltlbv();
                        ql.IdQl = QLTLBVDAO.taoMa();
                        ql.IdCategory = model.IdCategory;
                        ql.IdArticles = model.IdArticles;

                        _db.Qltlbvs.Add(ql);
                        _db.SaveChanges();
                        return Ok(new { result = true, message = "Thêm quản lý thể loại bài viết thành công" });
                    }
                }
                return Ok(new { result = false, message="Thêm quản lý thể loại bài viết thất bại" });
            }
            catch (Exception e)
            {
                return Ok(new { result = false, message = e.Message });
            }
        }

        
        [HttpDelete]
        [Authorize]

        public IActionResult delete(int id)
        {
            try
            {
                // linkQ[Object] query
                var ql = _db.Qltlbvs.SingleOrDefault(cat => cat.IdQl == id);
                if (ql == null)
                {
                    return Ok(new { result = false, message = "Không tìm thấy thông tin quản lý" });
                }

                // delete

                _db.Qltlbvs.Remove(ql);
                _db.SaveChanges();
                return Ok(new { result = true, message = "Xoá quản lý thể loại bài viết thành công" });
            }
            catch
            {
                return Ok(new { result = false, message = "Xoá thể loại bài viết thất bại" });
            }

        }
    }
}
