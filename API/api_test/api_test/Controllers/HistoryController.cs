using api_test.DAO;
using api_test.EF;
using api_test.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;


namespace api_test.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HistoryController : ControllerBase
    {
        private NewspaperReadingAppContext _db;
        public HistoryController(NewspaperReadingAppContext db)
        {
            _db = db;
        }

        [HttpGet]
        [Authorize]
        public IActionResult getAll()
        {
            return Ok(new { result = true, data= _db.Histories });
        }

        [HttpPost]
        [Route("Create")]
        [Authorize]
        public IActionResult create(HistoryModel model)
        {
            try
            {
                History his = new History();

            his.IdArticles = model.IdArticles;
            his.IdUser = model.IdUser;
            his.DatetimeSeen = DateTime.Now;

            _db.Histories.Add(his);
            _db.SaveChanges();
            return Ok(new { result = true, message="Thêm lịch sử thành công"});
            }
            catch (Exception e)
            {
                return Ok(new { result = false, message = e.Message });
            }
        }

        // delete tat ca lich su nguoi dung

        [HttpDelete("deleteAllHistoryOfUser/{id}")]
        [Authorize]
        public IActionResult deleteAll(int id)
        {
            try
            {
                HistoryDAO.deleteAllHistory(id);
                return Ok(new { result = true, message = "Xoá tất cả lịch sử xem thành công" });
            }
            catch (Exception e)
            {
                return Ok(new { result = false, message = "Xoá tất cả lịch sử xem thất bại" });
            }

        }

        // delete lich su tu chon
       
        [HttpDelete("delete")]
        [Authorize]
        public IActionResult deleteHisByID(History his)
        {
            try
            {
                HistoryDAO.deleteHistoryById(his.IdUser, his.IdArticles, his.DatetimeSeen);
                return Ok(new { result = true, message = "Xoá lịch sử xem thành công" });
            }
            catch (Exception e)
            {
                return Ok(new { result = false, message = "Xoá lịch sử xem thất bại" });
            }

        }
    }

   
}
