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
            List<ArticlesModelView> list = new List<ArticlesModelView>();
            for (int i = 0; i < data.Rows.Count; i++)
            {
                ArticlesModelView art = new ArticlesModelView();
                int ma = Int32.Parse(data.Rows[i]["id_articles"].ToString());
                art.IdArticles = ma;
                art.Title = data.Rows[i]["title"].ToString();
                art.ContentArticles = data.Rows[i]["content_articles"].ToString();
                int id_user = Int32.Parse(data.Rows[i]["id_user"].ToString());
                art.IdUser = id_user;
                art.Status= data.Rows[i]["status"].ToString();
                art.DateSubmitted = Convert.ToDateTime(data.Rows[i]["date_submitted"].ToString());
                art.Image = data.Rows[i]["image"].ToString();
                art.Views = Int32.Parse(data.Rows[i]["views"].ToString());
          
                // get user
                var user = _db.Users.SingleOrDefault(us => us.IdUser == id_user);
                UserView usv = new UserView();
                usv.IdUser = user.IdUser;
                usv.Name = user.Name;
                usv.Phone = user.Phone;
                usv.Role = user.Role;
                usv.Username = user.Username;
                usv.Gender = user.Gender;
                usv.Email = user.Email;
                usv.Avata = user.Avata;

                art.user = usv;

                // get category of articles
                art.listCategory = new List<int>();
                var data1 = ArticlesDAO.get_QLTLBV_of_Articles(ma);
                List<Qltlbv> list1 = new List<Qltlbv>();
                for (int i1 = 0; i1 < data1.Rows.Count; i1++)
                {
                    Qltlbv ql = new Qltlbv();
                    ql.IdQl = int.Parse(data1.Rows[i1]["id_QL"].ToString());
                    ql.IdArticles = int.Parse(data1.Rows[i1]["id_articles"].ToString());
                    ql.IdCategory = int.Parse(data1.Rows[i1]["id_category"].ToString());
                    list1.Add(ql);

                }

                for (int j= 0; j<list1.Count; j++)
                {
                    int id_cate = int.Parse( list1[j].IdCategory.ToString());
                    art.listCategory.Add(id_cate);
                }

                list.Add(art);
            }
            return Ok(new { result = true, data = list });
        }

        [HttpGet]
        [Route ("search_by_title")]
        public IActionResult search_by_title(String input)
        {
            var data = ArticlesDAO.search_by_title(input);
            List<ArticlesModelView> list = new List<ArticlesModelView>();
            for (int i = 0; i < data.Rows.Count; i++)
            {
                ArticlesModelView art = new ArticlesModelView();
                int ma = Int32.Parse(data.Rows[i]["id_articles"].ToString());
                art.IdArticles = ma;
                art.Title = data.Rows[i]["title"].ToString();
                art.ContentArticles = data.Rows[i]["content_articles"].ToString();
                int id_user = Int32.Parse(data.Rows[i]["id_user"].ToString());
                art.IdUser = id_user;
                art.Status = data.Rows[i]["status"].ToString();
                art.DateSubmitted = Convert.ToDateTime(data.Rows[i]["date_submitted"].ToString());
                art.Image = data.Rows[i]["image"].ToString();
                art.Views = Int32.Parse(data.Rows[i]["views"].ToString());

                // get user
                var user = _db.Users.SingleOrDefault(us => us.IdUser == id_user);
                UserView usv = new UserView();
                usv.IdUser = user.IdUser;
                usv.Name = user.Name;
                usv.Phone = user.Phone;
                usv.Role = user.Role;
                usv.Username = user.Username;
                usv.Gender = user.Gender;
                usv.Email = user.Email;
                usv.Avata = user.Avata;

                art.user = usv;

                // get category of articles
                art.listCategory = new List<int>();
                var data1 = ArticlesDAO.get_QLTLBV_of_Articles(ma);
                List<Qltlbv> list1 = new List<Qltlbv>();
                for (int i1 = 0; i1 < data1.Rows.Count; i1++)
                {
                    Qltlbv ql = new Qltlbv();
                    ql.IdQl = int.Parse(data1.Rows[i1]["id_QL"].ToString());
                    ql.IdArticles = int.Parse(data1.Rows[i1]["id_articles"].ToString());
                    ql.IdCategory = int.Parse(data1.Rows[i1]["id_category"].ToString());
                    list1.Add(ql);

                }

                for (int j = 0; j < list1.Count; j++)
                {
                    int id_cate = int.Parse(list1[j].IdCategory.ToString());
                    art.listCategory.Add(id_cate);
                }

                list.Add(art);
            }
            return Ok(new { result = true, data = list });
        }

        [HttpGet]
        [Route("search_by_id_category")]
        public IActionResult search_by_id_category(int id_category)
        {
            var data = ArticlesDAO.search_by_id_category(id_category);
            List<ArticlesModelView> list = new List<ArticlesModelView>();
            for (int i = 0; i < data.Rows.Count; i++)
            {
                ArticlesModelView art = new ArticlesModelView();
                int ma = Int32.Parse(data.Rows[i]["id_articles"].ToString());
                art.IdArticles = ma;
                art.Title = data.Rows[i]["title"].ToString();
                art.ContentArticles = data.Rows[i]["content_articles"].ToString();
                int id_user = Int32.Parse(data.Rows[i]["id_user"].ToString());
                art.IdUser = id_user;
                art.Status = data.Rows[i]["status"].ToString();
                art.DateSubmitted = Convert.ToDateTime(data.Rows[i]["date_submitted"].ToString());
                art.Image = data.Rows[i]["image"].ToString();
                art.Views = Int32.Parse(data.Rows[i]["views"].ToString());

                // get user
                var user = _db.Users.SingleOrDefault(us => us.IdUser == id_user);
                UserView usv = new UserView();
                usv.IdUser = user.IdUser;
                usv.Name = user.Name;
                usv.Phone = user.Phone;
                usv.Role = user.Role;
                usv.Username = user.Username;
                usv.Gender = user.Gender;
                usv.Email = user.Email;
                usv.Avata = user.Avata;

                art.user = usv;

                // get category of articles
                art.listCategory = new List<int>();
                var data1 = ArticlesDAO.get_QLTLBV_of_Articles(ma);
                List<Qltlbv> list1 = new List<Qltlbv>();
                for (int i1 = 0; i1 < data1.Rows.Count; i1++)
                {
                    Qltlbv ql = new Qltlbv();
                    ql.IdQl = int.Parse(data1.Rows[i1]["id_QL"].ToString());
                    ql.IdArticles = int.Parse(data1.Rows[i1]["id_articles"].ToString());
                    ql.IdCategory = int.Parse(data1.Rows[i1]["id_category"].ToString());
                    list1.Add(ql);

                }

                for (int j = 0; j < list1.Count; j++)
                {
                    int id_cate = int.Parse(list1[j].IdCategory.ToString());
                    art.listCategory.Add(id_cate);
                }

                list.Add(art);
            }
            return Ok(new { result = true, data = list });
        }


        [HttpGet]
        [Route("getTopView")]
       
        public IActionResult getTopView()
        {
            var data = ArticlesDAO.getTopView();
            List<ArticlesModelView> list = new List<ArticlesModelView>();
            for (int i = 0; i < data.Rows.Count; i++)
            {
                ArticlesModelView art = new ArticlesModelView();
                int ma= Int32.Parse(data.Rows[i]["id_articles"].ToString());
                art.IdArticles = ma;
                art.Title = data.Rows[i]["title"].ToString();
                art.ContentArticles = data.Rows[i]["content_articles"].ToString();
                int id_user = Int32.Parse(data.Rows[i]["id_user"].ToString());
                art.IdUser = id_user;
                art.Status = data.Rows[i]["status"].ToString();
                art.DateSubmitted = Convert.ToDateTime(data.Rows[i]["date_submitted"].ToString());
                art.Image = data.Rows[i]["image"].ToString();
                art.Views = Int32.Parse(data.Rows[i]["views"].ToString()); art.listCategory = new List<int>();

                // get info user
                var user = _db.Users.SingleOrDefault(us => us.IdUser == id_user);
                UserView usv = new UserView();
                usv.IdUser = user.IdUser;
                usv.Name = user.Name;
                usv.Phone = user.Phone;
                usv.Role = user.Role;
                usv.Username = user.Username;
                usv.Gender = user.Gender;
                usv.Email = user.Email;
                usv.Avata = user.Avata;
                art.user = usv;

                // get category of articles
                art.listCategory = new List<int>();
                var data1 = ArticlesDAO.get_QLTLBV_of_Articles(ma);
                List<Qltlbv> list1 = new List<Qltlbv>();
                for (int i1 = 0; i1 < data1.Rows.Count; i1++)
                {
                    Qltlbv ql = new Qltlbv();
                    ql.IdQl = int.Parse(data1.Rows[i1]["id_QL"].ToString());
                    ql.IdArticles = int.Parse(data1.Rows[i1]["id_articles"].ToString());
                    ql.IdCategory = int.Parse(data1.Rows[i1]["id_category"].ToString());
                    list1.Add(ql);

                }

                for (int j = 0; j < list1.Count; j++)
                {
                    int id_cate = int.Parse(list1[j].IdCategory.ToString());
                    art.listCategory.Add(id_cate);
                }
                list.Add(art);
            }
            return Ok(new { result = true, data = list });
        }

        [HttpGet]
        [Route("getTopNew")]

        public IActionResult getTopNew()
        {
            var data = ArticlesDAO.getTopNew();
            List<ArticlesModelView> list = new List<ArticlesModelView>();
            for (int i = 0; i < data.Rows.Count; i++)
            {
                ArticlesModelView art = new ArticlesModelView();
                int ma = Int32.Parse(data.Rows[i]["id_articles"].ToString());
                art.IdArticles = ma;
                art.Title = data.Rows[i]["title"].ToString();
                art.ContentArticles = data.Rows[i]["content_articles"].ToString();
                int id_user = Int32.Parse(data.Rows[i]["id_user"].ToString());
                art.IdUser = id_user;
                art.Status = data.Rows[i]["status"].ToString();
                art.DateSubmitted = Convert.ToDateTime(data.Rows[i]["date_submitted"].ToString());
                art.Image = data.Rows[i]["image"].ToString();
                art.Views = Int32.Parse(data.Rows[i]["views"].ToString()); art.listCategory = new List<int>();

                // get info user
                var user = _db.Users.SingleOrDefault(us => us.IdUser == id_user);
                UserView usv = new UserView();
                usv.IdUser = user.IdUser;
                usv.Name = user.Name;
                usv.Phone = user.Phone;
                usv.Role = user.Role;
                usv.Username = user.Username;
                usv.Gender = user.Gender;
                usv.Email = user.Email;
                usv.Avata = user.Avata;
                art.user = usv;

                // get category of articles
                art.listCategory = new List<int>();
                var data1 = ArticlesDAO.get_QLTLBV_of_Articles(ma);
                List<Qltlbv> list1 = new List<Qltlbv>();
                for (int i1 = 0; i1 < data1.Rows.Count; i1++)
                {
                    Qltlbv ql = new Qltlbv();
                    ql.IdQl = int.Parse(data1.Rows[i1]["id_QL"].ToString());
                    ql.IdArticles = int.Parse(data1.Rows[i1]["id_articles"].ToString());
                    ql.IdCategory = int.Parse(data1.Rows[i1]["id_category"].ToString());
                    list1.Add(ql);

                }

                for (int j = 0; j < list1.Count; j++)
                {
                    int id_cate = int.Parse(list1[j].IdCategory.ToString());
                    art.listCategory.Add(id_cate);
                }
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
                if(dt.TrangThaiXoa == 1)
                {
                    return Ok(new { result = false, message = "Bài viết đã bị xoá khỏi hệ thống" });
                }
                dt.Views = dt.Views + 1;
                _db.SaveChanges();

               
                ArticlesModelView art = new ArticlesModelView();

                art.IdArticles = dt.IdArticles;
                art.Title = dt.Title;
                art.ContentArticles = dt.ContentArticles;
                int id_user = dt.IdUser;
                art.IdUser = id_user;
                art.Status = dt.Status;
                art.DateSubmitted = dt.DateSubmitted;
                art.Image = dt.Image;
                art.Views = dt.Views;

                // get info user
                var user = _db.Users.SingleOrDefault(us => us.IdUser == id_user);
                UserView usv = new UserView();
                usv.IdUser = user.IdUser;
                usv.Name = user.Name;
                usv.Phone = user.Phone;
                usv.Role = user.Role;
                usv.Username = user.Username;
                usv.Gender = user.Gender;
                usv.Email = user.Email;
                usv.Avata = user.Avata;
                art.user = usv;

                // get category 
                art.listCategory = new List<int>();
                var data1 = ArticlesDAO.get_QLTLBV_of_Articles(id);

                List<Qltlbv> list1 = new List<Qltlbv>();
                for (int i1 = 0; i1 < data1.Rows.Count; i1++)
                {
                    Qltlbv ql = new Qltlbv();
                    ql.IdQl = int.Parse(data1.Rows[i1]["id_QL"].ToString());
                    ql.IdArticles = int.Parse(data1.Rows[i1]["id_articles"].ToString());
                    ql.IdCategory = int.Parse(data1.Rows[i1]["id_category"].ToString());
                    list1.Add(ql);

                }

                for (int j = 0; j < list1.Count; j++)
                {
                    int id_cate = int.Parse(list1[j].IdCategory.ToString());
                    art.listCategory.Add(id_cate);
                }

                return Ok(new { result = true, data = art });
            }
            catch (Exception a)
            {

                return Ok(new { result = false, message = a });//"Truy xuất thông tin bài viết theo id thất bại" });
            }

        }

        [HttpGet("pageList")]
       
        public IActionResult getPageList(int page, int pagesize)
        {
            var data = ArticlesDAO.getPageList(page, pagesize);
            List<ArticlesModelView> list = new List<ArticlesModelView>();
            for (int i = 0; i < data.Rows.Count; i++)
            {
                ArticlesModelView art = new ArticlesModelView();
                int ma= Int32.Parse(data.Rows[i]["id_articles"].ToString());
                art.IdArticles = ma;
                art.Title = data.Rows[i]["title"].ToString();
                art.ContentArticles = data.Rows[i]["content_articles"].ToString();
                int id_user = Int32.Parse(data.Rows[i]["id_user"].ToString());
                art.IdUser = id_user;
                art.Status = data.Rows[i]["status"].ToString();
                art.DateSubmitted = Convert.ToDateTime(data.Rows[i]["date_submitted"].ToString());
                art.Image = data.Rows[i]["image"].ToString();
                art.Views = Int32.Parse(data.Rows[i]["views"].ToString());

                // get info user
                var user = _db.Users.SingleOrDefault(us => us.IdUser == id_user);
                UserView usv = new UserView();
                usv.IdUser = user.IdUser;
                usv.Name = user.Name;
                usv.Phone = user.Phone;
                usv.Role = user.Role;
                usv.Username = user.Username;
                usv.Gender = user.Gender;
                usv.Email = user.Email;
                usv.Avata = user.Avata;
                art.user = usv;

                // get category
                art.listCategory = new List<int>();
                var data1 = ArticlesDAO.get_QLTLBV_of_Articles(ma);
                List<Qltlbv> list1 = new List<Qltlbv>();
                for (int i1 = 0; i1 < data1.Rows.Count; i1++)
                {
                    Qltlbv ql = new Qltlbv();
                    ql.IdQl = int.Parse(data1.Rows[i1]["id_QL"].ToString());
                    ql.IdArticles = int.Parse(data1.Rows[i1]["id_articles"].ToString());
                    ql.IdCategory = int.Parse(data1.Rows[i1]["id_category"].ToString());
                    list1.Add(ql);

                }

                for (int j = 0; j < list1.Count; j++)
                {
                    int id_cate = int.Parse(list1[j].IdCategory.ToString());
                    art.listCategory.Add(id_cate);
                }

                list.Add(art);
            }
            return Ok(new { result = true, data = list });
        }


        [HttpPost]
        [Route ("create")]
       [Authorize]
        public IActionResult create(ArticlesModel model)
        {
            var useTemp = _db.Users.SingleOrDefault(user => user.IdUser == model.IdUser && user.Role == 1);
            if (useTemp != null)
            {
                Article art = new Article();
                int ma = ArticlesDAO.taoMa();
                art.IdArticles = ma;
                art.Title = model.Title;
                art.ContentArticles = model.ContentArticles;
                art.IdUser = model.IdUser;
                art.Status = model.Status;
                art.Image = model.Image;
                art.Views = 0;
                art.TrangThaiXoa = 0;
                _db.Articles.Add(art);

                for(int i=0; i< model.listCategory.Count; i++)
                {
                    Qltlbv ql = new Qltlbv();
                    ql.IdQl = QLTLBVDAO.taoMa();
                    ql.IdArticles = ma;
                    ql.IdCategory = model.listCategory[i];
                    _db.Qltlbvs.Add(ql);
                    _db.SaveChanges();
                }
               
                _db.SaveChanges();
                return Ok(new { result = true, message = "Thêm bài viết thành công" });
            }
            return Ok(new { result = false, message = "User không phải tác giả" });
        }

       


        [HttpDelete]
        [Authorize]
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
                if(art.TrangThaiXoa == 1)
                {
                    return Ok(new { result = true, message = "Bài viết đã xoá khỏi hệ thống" });
                }

                // delete

                art.TrangThaiXoa = 1;
                _db.SaveChanges();
                return Ok(new { result = true, message = " Xoá bài viết thành công" });
            }
            catch
            {
                return Ok(new { result = false, message = "Đánh dấu xoá bài viết thất bại" });
            }

        }

      

        [HttpPut("Edit/{id}")]
        [Authorize]
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
                if (art.TrangThaiXoa ==1)
                {
                    return Ok(new { result = false, message = "Bài viết không tồn tại trong hệ thống" });
                }
                // update

                art.Title = model.Title;
                art.ContentArticles = model.ContentArticles;
                art.Status = model.Status;
                art.Image = model.Image;

                // delete all QLTLBV of articles
                var data = ArticlesDAO.get_QLTLBV_of_Articles(id);
                List<Qltlbv> list = new List<Qltlbv>();
                for (int i = 0; i < data.Rows.Count; i++)
                {
                    Qltlbv ql = new Qltlbv();
                    ql.IdQl = int.Parse(data.Rows[i]["id_QL"].ToString());
                    ql.IdArticles = int.Parse(data.Rows[i]["id_articles"].ToString());
                    ql.IdCategory = int.Parse(data.Rows[i]["id_category"].ToString());
                    list.Add(ql);
                }

                foreach (Qltlbv ql in list)
                {
                    _db.Qltlbvs.Remove(ql);
                    _db.SaveChanges();
                }

               
                // add category into QLTLBV
                for (int i = 0; i < model.listCategory.Count; i++)
                {
                    Qltlbv ql = new Qltlbv();
                    ql.IdQl = QLTLBVDAO.taoMa();
                    ql.IdArticles = id;
                    ql.IdCategory = model.listCategory[i];
                    _db.Qltlbvs.Add(ql);
                    _db.SaveChanges();
                }

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
