﻿using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MrCMS.Web.Admin.Infrastructure.BaseControllers;
using MrCMS.Web.Admin.Infrastructure.Models;
using MrCMS.Web.Admin.Infrastructure.Services;
using MrCMS.Web.Apps.Articles.Areas.Admin.Models;
using MrCMS.Web.Apps.Articles.Entities;

namespace MrCMS.Web.Apps.Articles.Areas.Admin.Controllers
{
    public class AuthorInfoController : MrCMSAppAdminController<MrCMSArticlesApp>
    {
        private readonly IUserProfileAdminService _adminService;

        public AuthorInfoController(IUserProfileAdminService adminService)
        {
            _adminService = adminService;
        }

        [HttpGet]
        public PartialViewResult Add(int id)
        {
            return PartialView(new AddAuthorInfoModel { UserId = id });
        }

        [HttpPost]
        public async Task<RedirectToActionResult> Add(AddAuthorInfoModel info)
        {
            await _adminService.Add<AuthorInfo, AddAuthorInfoModel>(info);
            return RedirectToAction("Edit", "User", new { id = info.UserId });
        }

        [HttpGet]
        public async Task<PartialViewResult> Edit(int id)
        {
            return PartialView(await _adminService.GetEditModel<AuthorInfo, EditAuthorInfoModel>(id));
        }

        [HttpPost]
        [ActionName("Edit")]
        public async Task<RedirectToActionResult> Edit_POST(EditAuthorInfoModel model)
        {
            var info = await _adminService.Update<AuthorInfo, EditAuthorInfoModel>(model);
            return RedirectToAction("Edit", "User", new { id = info?.User.Id });
        }

        //public PartialViewResult Show(User user)
        //{
        //    return PartialView(user);
        //}
    }

    public class EditAuthorInfoModel : IHaveId
    {
        int? IHaveId.Id => Id;
        public int Id { get; set; }
        public string Bio { get; set; }
    }
}
