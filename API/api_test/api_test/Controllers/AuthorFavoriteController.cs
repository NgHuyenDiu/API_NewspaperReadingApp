using api_test.DAO;
using api_test.EF;
using api_test.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace api_test.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorFavoriteController : ControllerBase
    {
        private NewspaperReadingAppContext _db;
        public AuthorFavoriteController(NewspaperReadingAppContext db)
        {
            _db = db;
        }

        [HttpGet]
        [Authorize]
        public IActionResult getAll()
        {
            return Ok(new { result = true, data = _db.AuthorFavorites });
        }

    
        [HttpPost]
        [Route ("create")]
        [Authorize]
        public IActionResult createAuthorFavorite(AuthorFavoriteModel model)
        {
            AuthorFavorite his = new AuthorFavorite();
            var useTemp = _db.Users.SingleOrDefault(user => user.IdUser == model.IdAuthor && user.Role == 1);
            if (useTemp != null)
            {
                var auth = _db.AuthorFavorites.SingleOrDefault(auth => auth.IdUser == model.IdUser && auth.IdAuthor == model.IdAuthor);
                if (auth == null)
                {
                    his.IdFavorite = AuthorFavoriteDAO.taoMa();
                    his.IdUser = model.IdUser;
                    his.IdAuthor = model.IdAuthor;
                    _db.AuthorFavorites.Add(his);
                    _db.SaveChanges();
                    return Ok(new { result = true, message = "Thêm thông tin tác giả yêu thích thành công" });
                }
                return Ok(new { result = false, message = "Tác giả đã được yêu thích bởi người dùng" });
            }
            return Ok(new { result = false, message = "mã số tác giả không đúng" });
        }

      
        [HttpDelete]
        [Authorize]
        public IActionResult deleteAuthor(int id_use, int id_author)
        {
            try
            {
                // linkQ[Object] query
                var user = _db.AuthorFavorites.SingleOrDefault(user => user.IdAuthor == id_author && user.IdUser == id_use);
                if (user == null)
                {
                    return Ok(new { result = false, message = "Không tìm thấy thông tin tác giả yêu thích" });
                }

                // delete

                _db.AuthorFavorites.Remove(user);
                _db.SaveChanges();
                return Ok(new { result = true, message = "Xoá tác giả yêu thích thành công" });
            }
            catch
            {
                return Ok(new { result = false, message = "Xoá tác giả yêu thích không thành công" });
            }

        }
    }
}
