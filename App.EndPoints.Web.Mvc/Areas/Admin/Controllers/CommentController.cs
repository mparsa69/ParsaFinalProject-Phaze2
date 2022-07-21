﻿using App.Domain.Core.OrderAgg.Contracts.IAppServices;
using Microsoft.AspNetCore.Mvc;

namespace App.EndPoints.Web.Mvc.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CommentController : Controller
    {
        private readonly ICommentAppService _commentAppService;

        public CommentController(ICommentAppService commentAppService)
        {
            _commentAppService = commentAppService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult OrderComments(int orderId)
        {
            var comments = _commentAppService.OrderComments(orderId);
            return View(comments);
        }
        public IActionResult Details(int id)
        {
            //var comment = _dbContext.Comments.FirstOrDefault(x => x.Id == id);
            var comment = _commentAppService.Details(id);
            return View(comment);
        }


        [HttpPost]
        public IActionResult Delete(int id)
        {
            _commentAppService.Delete(id);

            return RedirectToAction("Index","Order");
        }

        [HttpPost]
        public IActionResult EnableShow(int id)
        {
            _commentAppService.EnableShow(id);
            /*return RedirectToAction("OrderComments", "Order", new {id});*/
            return RedirectToAction("Index", "Order");
        }

        [HttpPost]
        public IActionResult DisableShow(int id)
        {
            _commentAppService.DisableShow(id);
            return RedirectToAction("Index", "Order");
        }


    }
}
