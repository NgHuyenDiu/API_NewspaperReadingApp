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
    public class CategoryController : ControllerBase
    {

        private NewspaperReadingAppContext _db;
        public CategoryController(NewspaperReadingAppContext db)
        {
            _db = db;
        }

        [HttpGet]
       
        public IActionResult getAll()
        {
            var data = CategoryDAO.getAll();
            List<CategoryView> list = new List<CategoryView>();
            for (int i = 0; i < data.Rows.Count; i++)
            {
                CategoryView cmt = new CategoryView();
                int id_category = Int32.Parse(data.Rows[i]["id_category"].ToString());
                cmt.IdCategory = id_category;     
                cmt.Title= data.Rows[i]["title"].ToString();


                var data1 = CategoryDAO.getListIDArticles(id_category);
                List<IDArticles> list1 = new List<IDArticles>();
                for (int i1 = 0; i1 < data1.Rows.Count; i1++)
                {
                    IDArticles id = new IDArticles();
                    id.IdArticles = Int32.Parse(data1.Rows[i1]["id"].ToString());
                    list1.Add(id);
                }

                cmt.listIDArticles = list1;
                list.Add(cmt);
            }
            return Ok(new { result = true, data = list });
        }

        [HttpPost]
        [Route ("create")]
        [Authorize]
        public IActionResult createAuthorFavorite(CategoryModel model)
        {
            Category cat = new Category();

            cat.IdCategory = CategoryDAO.taoMa();
            var cate = _db.Categories.SingleOrDefault(cat => cat.Title == model.Title );
            if (cate == null)
            {
                cat.Title = model.Title;

                _db.Categories.Add(cat);
                _db.SaveChanges();
                return Ok(new { result = true, message = "Thêm thể loại thành công" });

            }
            return Ok(new { result = false, message = "Thể loại đã tồn tại" });
        }

       
        [HttpDelete]
        [Authorize]
        public IActionResult deleteCategory(int id_cat)
        {
            try
            {
                // linkQ[Object] query
                var cate = _db.Categories.SingleOrDefault(cat => cat.IdCategory == id_cat);
                if (cate == null)
                {
                    return Ok(new { result = false, message = "Không tìm thấy thông tin thể loại" });
                }

                // delete

                _db.Categories.Remove(cate);
                _db.SaveChanges();
                return Ok(new { result = true, message = "Xoá thể loại thành công" });
            }
            catch
            {
                return Ok(new { result = false, message = "Xoá thể loại thất bại" });
            }

        }
        
        [HttpPut("EditCategory/{id}")]
        [Authorize]
        public IActionResult edit(int id , CategoryModel model)
        {
            try
            {
                // linkQ[Object] query
                var cate = _db.Categories.SingleOrDefault(cat => cat.IdCategory == id);
                if (cate == null)
                {
                    return Ok(new { result = false, message = "Không tìm thấy thông tin thể loại" });
                }
                if (id != cate.IdCategory)
                {
                    return Ok(new { result = false, message = "Id category không thể sửa đổi" });
                }
                // update

                cate.Title = model.Title;
                    _db.SaveChanges();
                    return Ok(new { result = true, message = "Chỉnh sửa thông tin thành công" });
            }
            catch
            {
                return Ok(new { result = false, message = "Chỉnh sửa thất bại" });
            }
        }

    }
}
