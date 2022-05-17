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
        [Authorize]
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
        [Authorize]
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
                var pass =  PasswordHelper.Decrypt(user.Password);
                if(pass == model.Password)
                {
                    //cấp token
                    return Ok(new ApiResponse
                    {
                        Result = true,
                        Message = "Authenticate success",
                        Data = GenerateToken(user)
                    });
                }
                else
                {
                    return Ok(new { result = false, message = "Mật khẩu không đúng" });
                }
            }

          

        }

        private TokenModel GenerateToken(User nguoiDung)
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
                Expires = DateTime.UtcNow.AddSeconds(20),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secretKeyBytes), SecurityAlgorithms.HmacSha512Signature)
            };

            var token = jwtTokenHandler.CreateToken(tokenDescription);
            var accessToken = jwtTokenHandler.WriteToken(token);
            var refreshToken = GenerateRefreshToken();

            //Lưu database
            var refreshTokenEntity = new RefreshToken
            {
                Id = Guid.NewGuid(),
                JwtId = token.Id,
                UserId = nguoiDung.IdUser,
                Token = refreshToken,
                IsUsed = false,
                IsRevoked = false,
                IssuedAt = DateTime.UtcNow,
                ExpiredAt = DateTime.UtcNow.AddHours(1)
            };
            _db.Add(refreshTokenEntity);
            _db.SaveChanges();

            return new TokenModel
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };

        }

        private string GenerateRefreshToken()
        {
            var random = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(random);

                return Convert.ToBase64String(random);
            }
        }

        [HttpPost("RefreshToken")]
        [Authorize]
        public IActionResult RenewToken(TokenModel model)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var secretKeyBytes = Encoding.UTF8.GetBytes(_appSettings.SecretKey);
            var tokenValidateParam = new TokenValidationParameters
            {
                //tự cấp token
                ValidateIssuer = false,
                ValidateAudience = false,

                //ký vào token
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(secretKeyBytes),

                ClockSkew = TimeSpan.Zero,

                ValidateLifetime = false //ko kiểm tra token hết hạn
            };
            try
            {
                //check 1: AccessToken valid format
                var tokenInVerification = jwtTokenHandler.ValidateToken(model.AccessToken, tokenValidateParam, out var validatedToken);

                //check 2: Check thuật toán
                if (validatedToken is JwtSecurityToken jwtSecurityToken)
                {
                    var result = jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha512, StringComparison.InvariantCultureIgnoreCase);
                    if (!result)//false
                    {
                        return Ok(new ApiResponse
                        {
                            Result = false,
                            Message = "Invalid token"
                        });
                    }
                }

                //check 3: Check accessToken expire?
                var utcExpireDate = long.Parse(tokenInVerification.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Exp).Value);

                var expireDate = ConvertUnixTimeToDateTime(utcExpireDate);
                if (expireDate > DateTime.UtcNow)
                {
                    return Ok(new ApiResponse
                    {
                        Result = false,
                        Message = "Access token has not yet expired"
                    });
                }

                //check 4: Check refreshtoken exist in DB
                var storedToken = _db.RefreshTokens.FirstOrDefault(x => x.Token == model.RefreshToken);
                if (storedToken == null)
                {
                    return Ok(new ApiResponse
                    {
                        Result = false,
                        Message = "Refresh token does not exist"
                    });
                }

                //check 5: check refreshToken is used/revoked?
                if (storedToken.IsUsed)
                {
                    return Ok(new ApiResponse
                    {
                        Result = false,
                        Message = "Refresh token has been used"
                    });
                }
                if (storedToken.IsRevoked)
                {
                    return Ok(new ApiResponse
                    {
                        Result = false,
                        Message = "Refresh token has been revoked"
                    });
                }

                //check 6: AccessToken id == JwtId in RefreshToken
                var jti = tokenInVerification.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Jti).Value;
                if (storedToken.JwtId != jti)
                {
                    return Ok(new ApiResponse
                    {
                        Result = false,
                        Message = "Token doesn't match"
                    });
                }

                //Update token is used
                storedToken.IsRevoked = true;
                storedToken.IsUsed = true;
                _db.Update(storedToken);
                _db.SaveChanges();

                //create new token
                var user = _db.Users.SingleOrDefault(us => us.IdUser == storedToken.UserId);
                var token = GenerateToken(user);

                return Ok(new ApiResponse
                {
                    Result = true,
                    Message = "Renew token success",
                    Data = token
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse
                {
                    Result = false,
                    Message = "Something went wrong"
                });
            }
        }

        private DateTime ConvertUnixTimeToDateTime(long utcExpireDate)
        {
            var dateTimeInterval = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dateTimeInterval.AddSeconds(utcExpireDate).ToUniversalTime();

            return dateTimeInterval;
        }

        // quên mật khẩu
        [HttpPost]
        [Route("forgotPassword")]
         [Authorize]
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
                string smtpUserName = "minhtan0802@gmail.com";
                string smtpPassword = "wvtpyuhdugdcxjat";
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
        [Authorize]
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
        public IActionResult edit(int id, UserEdit userEdit)
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
                            return Ok(new { result = true, message = "Chỉnh sửa thông tin thành công" });
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
        //  [Authorize]
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

