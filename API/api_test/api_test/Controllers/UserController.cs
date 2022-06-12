using api_test.DAO;
using api_test.EF;
using api_test.helper;
using api_test.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using test.Models;

namespace api_test.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private NewspaperReadingAppContext _db;

        private readonly Appsetting _appSettings;
        public UserController(NewspaperReadingAppContext db, IOptionsMonitor<Appsetting> optionsMonitor)
        {
            _db = db;
            _appSettings = optionsMonitor.CurrentValue;
        }
        [HttpGet]
      
        public IActionResult getAll()
        {
            var data = UserDAO.getAll();
            List<UserView> list = new List<UserView>();
            for (int i = 0; i < data.Rows.Count; i++)
            {
                UserView u = new UserView();
                u.IdUser = Int32.Parse(data.Rows[i]["id_user"].ToString());
                u.Name = data.Rows[i]["name"].ToString();
                u.Gender = Int32.Parse(data.Rows[i]["gender"].ToString());
                u.Phone = data.Rows[i]["phone"].ToString();
                u.Email = data.Rows[i]["email"].ToString();
                u.Avata = data.Rows[i]["avata"].ToString();
                u.Username = data.Rows[i]["username"].ToString();
                u.Role = Int32.Parse(data.Rows[i]["role"].ToString());
                list.Add(u);
            }
            return Ok(new { result = true, data = list });
        }

        [HttpGet("pageList")]
        
        public IActionResult getPageList(int page, int pagesize)
        {
            var data = UserDAO.getPageList(page, pagesize);
            List<UserView> list = new List<UserView>();
            for (int i = 0; i < data.Rows.Count; i++)
            {
                UserView u = new UserView();
                u.IdUser = Int32.Parse(data.Rows[i]["id_user"].ToString());
                u.Name = data.Rows[i]["name"].ToString();
                u.Gender = Int32.Parse(data.Rows[i]["gender"].ToString());
                u.Phone = data.Rows[i]["phone"].ToString();
                u.Email = data.Rows[i]["email"].ToString();
                u.Avata = data.Rows[i]["avata"].ToString();
                u.Username = data.Rows[i]["username"].ToString();
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
                UserView userView = new UserView();
                userView.Name = user.Name;
                userView.Phone = user.Phone;
                userView.Email = user.Email;
                userView.Avata = user.Avata;
                userView.Gender = user.Gender;
                userView.Username = user.Username;

                return Ok(new { result = true, data = userView });
            }
            catch
            {
                return Ok(new { result = false, message = "Truy xuất thông tin user theo id thất bại" });
            }

        }


        [HttpPost("Login")]

        public IActionResult validate(LoginModel model)
        {
            var user = _db.Users.SingleOrDefault(user => user.Username == model.Username && user.Status == 0);
            if (user == null) //không đúng
            {
                return Ok(new { result = false, message = "User không tồn tại hoặc user đã bị khoá tài khoản" });
            }
            else
            {
                UserView userView = new UserView();
                userView.IdUser = user.IdUser;
                userView.Name = user.Name;
                userView.Phone = user.Phone;
                userView.Email = user.Email;
                userView.Avata = user.Avata;
                userView.Gender = user.Gender;
                userView.Username = user.Username;
                userView.Role = user.Role;

                // GET COUNT FAVOURITE 
                var data = AuthorFavoriteDAO.getCountNumber(user.IdUser);
                List<CountNumber> list = new List<CountNumber>();
                for (int i = 0; i < data.Rows.Count; i++)
                {
                    CountNumber u = new CountNumber();
                    u.number_of_authors_I_follow = Int32.Parse(data.Rows[i]["number_of_authors_I_follow"].ToString());    
                    u.number_of_people_watching = Int32.Parse(data.Rows[i]["number_of_people_watching"].ToString()); 
                    list.Add(u);
                }

                // GET LIST AUTHOR FAVOURITE

                var data1 = UserDAO.getListAuthorFavorite(user.IdUser);
                List<UserView> list1 = new List<UserView>();
                for (int i = 0; i < data1.Rows.Count; i++)
                {
                    UserView u = new UserView();
                    u.IdUser = Int32.Parse(data1.Rows[i]["id_user"].ToString());
                    u.Name = data1.Rows[i]["name"].ToString();
                    u.Gender = Int32.Parse(data1.Rows[i]["gender"].ToString());
                    u.Phone = data1.Rows[i]["phone"].ToString();
                    u.Email = data1.Rows[i]["email"].ToString();
                    u.Avata = data1.Rows[i]["avata"].ToString();
                    u.Username = data1.Rows[i]["username"].ToString();
                    u.Role = Int32.Parse(data1.Rows[i]["role"].ToString());
                    list1.Add(u);
                }


                var pass =  PasswordHelper.Decrypt(user.Password);
                if(pass == model.Password)
                {
                    //cấp token
                    return Ok(new 
                    {
                        Result = true,
                        Message = "Authenticate success",
                        DataToken = GenerateToken(user),
                        DataUser = userView,
                        countFavourite = list,
                        listAuthorFavourite = list1
                    });
                }
                else
                {
                    return Ok(new { result = false, message = "Mật khẩu không đúng" });
                }
            }

          

        }

        private String GenerateToken(User nguoiDung)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();

            var secretKeyBytes = Encoding.UTF8.GetBytes(_appSettings.SecretKey);

            var tokenDescription = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] {
                    new Claim(ClaimTypes.Name, nguoiDung.Name),
                    new Claim(JwtRegisteredClaimNames.Email, nguoiDung.Email),
                    new Claim(JwtRegisteredClaimNames.Sub, nguoiDung.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()), // id của token
                    new Claim("UserName", nguoiDung.Username),
                    new Claim("Id", nguoiDung.IdUser.ToString()),

                    //roles
                }),
                Expires = DateTime.UtcNow.AddDays(10),  //AddHours(1), //.AddSeconds(20),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secretKeyBytes), SecurityAlgorithms.HmacSha512Signature)
            };

            var token = jwtTokenHandler.CreateToken(tokenDescription);
            var accessToken = jwtTokenHandler.WriteToken(token);
            return accessToken;
       

        }

  

        // quên mật khẩu
        [HttpPost]
        [Route("forgotPassword")]
       // [Authorize]
        public IActionResult forgot(String email)
        {
            try
            {
                var useTemp = _db.Users.SingleOrDefault(user => user.Email == email && user.Status == 0);

                if (useTemp == null)
                {
                    return Ok(new { result = false, message = "user chưa được đăng ký tài khoản hoặc đã bị khoá tài khoản" });
                }
                
                String newpass =PasswordHelper.CreatePassword(8);
                String encryptPass = PasswordHelper.Encrypt(newpass);
                Console.WriteLine("New pass:****" + newpass);
                useTemp.Password = encryptPass;


                // gửi mail cho user
                string smtpUserName = "huyendiusmilef5@gmail.com";
                string smtpPassword = "xynignkmcdlfkvdr";
                string smtpHost = "smtp.gmail.com";
                int smtpPort = 587;

                string emailTo = email;
                string subject = "Lấy lại mật khẩu";
                string body = string.Format("Mật khẩu mới của bạn là: <b></b><br/><br/>{0} </br>", newpass);

                EmailService service = new EmailService();
                bool kq = service.Send(smtpUserName, smtpPassword, smtpHost, smtpPort, emailTo, subject, body);
                if (kq == true)
                {
                    _db.SaveChanges();
                    return Ok(new { result = true, message = "Mật khẩu được chuyển đến mail thành công" });
                }
                else
                {
                    return Ok(new { result = false, message = "Không gửi được mail" });
                }
            }
            catch (Exception e)
            {
                return Ok(new { result = false, data = e.Message });
            }


        }

        [HttpPost]
        [Route("CreateUser")]
       // [Authorize]
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
                    user.Password = PasswordHelper.Encrypt(model.Password);
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
          [Authorize]
        public IActionResult edit(int id, UserEdit userEdit
            )
        {
            try
            {
                // linkQ[Object] query
                var user = _db.Users.SingleOrDefault(user => user.IdUser == id);
                if (user == null)
                {
                    return Ok(new { result = false, message = "Không tồn tại user" });
                }

                // update
                var useTemp = _db.Users.SingleOrDefault(user => user.Username == userEdit.Username && user.IdUser != id);
                if (useTemp == null)
                {
                    var useEmail = _db.Users.SingleOrDefault(user => user.Email == userEdit.Email && user.IdUser != id);
                    if (useEmail == null)
                    {
                        var usePhone = _db.Users.SingleOrDefault(user => user.Phone == userEdit.Phone && user.IdUser != id);
                        if (usePhone == null)
                        {
                            user.Name = userEdit.Name;
                            user.Gender = userEdit.Gender;
                            user.Phone = userEdit.Phone;
                            user.Email = userEdit.Email;
                            user.Avata = userEdit.Avata;
                            user.Username = userEdit.Username;

                            _db.SaveChanges();

                            var user1 = _db.Users.SingleOrDefault(user => user.IdUser == id);
                            UserView userView = new UserView();
                            userView.IdUser = user1.IdUser;
                            userView.Name = user1.Name;
                            userView.Phone = user1.Phone;
                            userView.Email = user1.Email;
                            userView.Avata = user1.Avata;
                            userView.Gender = user1.Gender;
                            userView.Username = user1.Username;
                            userView.Role = user1.Role;
                            return Ok(new { result = true, message = "Chỉnh sửa thông tin thành công", data = userView });
                        }
                        else
                        {
                            return Ok(new { result = false, message = "Số điện thoại đã tồn tại " });
                        }
                    }
                    else
                    {
                        return Ok(new { result = false, message = "Email đã tồn tại " });
                    }
                }
                else
                {
                    return Ok(new { result = false, message = "Username đã tồn tại " });
                }
            }
            catch
            {
                return Ok(new { result = false, message = "Chỉnh sửa thất bại" });
            }
        }

        // thay đổi mật khẩu
        [HttpPut("EditPassword")]
          [Authorize]
        public IActionResult editPassword(String username, String oldPass, String newPass)
        {
            try
            {
                // linkQ[Object] query
               
                var user = _db.Users.SingleOrDefault(user => user.Username == username );
                if (user == null)
                {
                    return Ok(new { result = false, message = "User không tồn tại" });
                }
                else
                {
                    var pass = PasswordHelper.Decrypt(user.Password);
                    if (pass == oldPass)
                    {
                        // update
                        user.Password = PasswordHelper.Encrypt(newPass);
                        _db.SaveChanges();
                        return Ok(new { result = true, message = "Chỉnh sửa mật khẩu thành công" });
                    }
                    else
                    {
                        return Ok(new { result = false, message = "Mật khẩu cũ không đúng" });
                    }
                }
               
            }
            catch(Exception ex)
            {
                return Ok(new { result = false, message = ex.Message });
            }
        }


        // thay đổi role user
        [HttpPut("EditRole")]
        [Authorize]
        public IActionResult editRole(int id_admin, int id_user, int role)
        {
            try
            {
                // linkQ[Object] query

                var user_admin = _db.Users.SingleOrDefault(user => user.IdUser == id_admin && user.Role == 0);
                if (user_admin == null)
                {
                    return Ok(new { result = false, message = "Không tìm thấy user hoặc user không có quyền chỉnh sửa" });
                }
                else
                {
                    var user = _db.Users.SingleOrDefault(user => user.IdUser == id_user );
                    if (user == null)
                    {
                        return Ok(new { result = false, message = "Không tìm thấy user" });
                    }
                    else
                    {
                        user.Role = role;
                        _db.SaveChanges();
                        return Ok(new { result = true, message = "Chỉnh sửa quyền người dùng thành công" });
                    }
                 
                   
                }

            }
            catch (Exception ex)
            {
                return Ok(new { result = false, message = ex.Message });
            }
        }


        // khoá tài khoản
        [HttpDelete("LockAccount/{id}")]
        [Authorize]
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
                if (user.Status == 1)
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
        [Authorize]
        public IActionResult openAcc(int id)
        {
            try
            {
                // linkQ[Object] query
                var user = _db.Users.SingleOrDefault(user => user.IdUser == id);
                if (user == null)
                {
                    return Ok(new { result = false, message = "Không tìm thấy tài khoản người dùng" });
                }
                if (user.Status == 0)
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
        [Authorize]
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


    }
}

