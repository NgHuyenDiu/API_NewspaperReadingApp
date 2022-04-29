using api_test.DAO;
using api_test.EF;
using api_test.Model;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace api_test.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private NewspaperReadingAppContext _db;
        public UserController(NewspaperReadingAppContext db)
        {
            _db = db;
        }
        [HttpGet]
        public IActionResult getAll()
        {
            var data = UserDAO.getAll();
            List<User> list = new List<User>();
            for (int i = 0; i < data.Rows.Count; i++)
            {
                User u = new User();
                u.IdUser = Int32.Parse(data.Rows[i]["id_user"].ToString());
                u.Name = data.Rows[i]["name"].ToString();
                u.Gender = Int32.Parse(data.Rows[i]["gender"].ToString());
                u.Phone = data.Rows[i]["phone"].ToString();
                u.Email = data.Rows[i]["email"].ToString();
                u.Avata = data.Rows[i]["avata"].ToString();
                u.Username = data.Rows[i]["username"].ToString();
                u.Password = data.Rows[i]["password"].ToString();
                u.Role = Int32.Parse(data.Rows[i]["role"].ToString());
                list.Add(u);
            }
            return Ok(new { result = true, data = list });
        }

        [HttpGet("get/{id}")]
        public IActionResult getUserByID(int id)
        {
            try
            {
                // linkQ[Object] query
                var user = _db.Users.SingleOrDefault(user => user.IdUser == id);
                if (user == null)
                {
                    return Ok(new { result = false, message = "Không tồn tại user" });
                }
                return Ok(new { result = true, data = user });
            }
            catch
            {
                return Ok(new { result = false, message = "Truy xuất thông tin user theo id thất bại" });
            }

        }

        // dang nhap
        [Route("logIn")]
        [HttpPost]
        public IActionResult logIn(String usename, String password)
        {
            var useTemp = _db.Users.SingleOrDefault(user => user.Username == usename && user.Status == 0);
            if (useTemp != null)
            {
                if (useTemp.Password.Equals(password))
                {
                    return Ok(new { result = true, message = "Đăng nhập thành công" });
                }
            }
            return Ok(new { result = false, message = "Đăng nhập thất bại" });
        }
        // them moi user
        [HttpPost]
        [Route("CreateUser")]
        public IActionResult create(UserModel model)
        {

            var useTemp = _db.Users.SingleOrDefault(user => user.Username == model.Username);

            if (useTemp == null)
            {
                var phoneTemp = _db.Users.SingleOrDefault(user => user.Phone == model.Phone || user.Email == model.Email);

                if (phoneTemp == null)
                {
                    User user = new User();
                    user.IdUser = UserDAO.taoMa();
                    user.Name = model.Name;
                    user.Gender = model.Gender;
                    user.Phone = model.Phone;
                    user.Email = model.Email;
                    user.Avata = model.Avata;
                    user.Username = model.Username;
                    user.Password = model.Password;
                    user.Role = model.Role;
                    user.Status = 0;
                    _db.Users.Add(user);
                    _db.SaveChanges();
                    return Ok(new { result = true, message = "Thêm thông tin user thành công" });
                }
                return Ok(new { result = false, message = "Số điện thoại hoặc email đã tồn tại" });
            }
            return Ok(new { result = false, message = "Username đã tồn tại" });
        }

        // chỉnh sửa thông tin cá nhân
        [HttpPut("EditAccount/{id}")]
        public IActionResult edit(int id, User userEdit)
        {
            try
            {
                // linkQ[Object] query
                var user = _db.Users.SingleOrDefault(user => user.IdUser == id);
                if (user == null)
                {
                    return Ok(new { result = false, message = "Không tồn tại user" });
                }
                if (id != user.IdUser)
                {
                    return Ok(new { result = false, message = "Id user không thể sửa đổi" });
                }
                // update
                var useTemp = _db.Users.SingleOrDefault(user => user.Username == userEdit.Username && user.IdUser != userEdit.IdUser);
                if (useTemp == null)
                {
                    user.Name = userEdit.Name;
                    user.Gender = userEdit.Gender;
                    user.Phone = userEdit.Phone;
                    user.Email = userEdit.Email;
                    user.Avata = userEdit.Avata;
                    user.Username = userEdit.Username;
                    user.Password = userEdit.Password;
                    _db.SaveChanges();
                    return Ok(new { result = true, message = "Chỉnh sửa thông tin thành công" });
                }
                else
                {
                    return Ok(new { result = false, message = "Username đã tồn tại" });
                }


            }
            catch
            {
                return Ok(new { result = false, message = "Chỉnh sửa thất bại" });
            }
        }

        // khoá tài khoản
        [HttpDelete("LockAccount/{id}")]
        public IActionResult delete(int id)
        {
            try
            {
                // linkQ[Object] query
                var user = _db.Users.SingleOrDefault(user => user.IdUser == id);
                if (user == null)
                {
                    return Ok(new { result = false, message = "Không tìm thấy tài khoản người dùng" });
                }
                if(user.Status == 1)
                {
                    return Ok(new { result = false, message = "Tài khoản người dùng đã khoá" });
                }

                // delete

                user.Status = 1;
                _db.SaveChanges();
                return Ok(new { result = true, message = "Đánh dấu xoá thành công" });
            }
            catch
            {
                return Ok(new { result = false, message = "Đánh dấu xoá không thành công" });
            }
        }

        [HttpPut("OpenAccount/{id}")]

        public IActionResult openAcc(int id)
        {
            try
            {
                // linkQ[Object] query
                var user = _db.Users.SingleOrDefault(user => user.IdUser == id );
                if (user == null)
                {
                    return Ok(new { result = false, message = "Không tìm thấy tài khoản người dùng" });
                }
                if(user.Status == 0)
                {
                    return Ok(new { result = false, message = "Tài khoản người dùng đang hoạt động" });
                }

                // update

                user.Status = 0;
                _db.SaveChanges();
                return Ok(new { result = true, message = "Mở khoá tài khoản thành công" });
            }
            catch
            {
                return Ok(new { result = false, message = "Mở tài khoản không thành công" });
            }
        }
        //***********HISTORY***********************

        // lịch sử xem bài viết
        [HttpGet("histoty/{id}")]
        public IActionResult getHistory(int id)
        {
            var data = UserDAO.getListHistory(id);

            List<UserHistory> list = new List<UserHistory>();
            for (int i = 0; i < data.Rows.Count; i++)
            {
                UserHistory u = new UserHistory();
                u.Title = data.Rows[i]["title"].ToString();
                u.DatetimeSeen = Convert.ToDateTime(data.Rows[i]["datetime_seen"].ToString());

                list.Add(u);
            }
            return Ok(new { result = true, data = list });

        }

        //***********AUTHORFAVORITE***********************

       

    }
}

