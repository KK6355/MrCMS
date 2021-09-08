﻿using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MrCMS.Helpers;
using MrCMS.Models;
using MrCMS.Services.ImportExport;
using MrCMS.Web.Admin.Infrastructure.BaseControllers;

namespace MrCMS.Web.Admin.Controllers
{
    public class ImportExportController : MrCMSAdminController
    {
        private readonly IImportExportManager _importExportManager;
        private readonly ILogger<ImportExportController> _logger;

        public ImportExportController(IImportExportManager importExportManager, ILogger<ImportExportController> logger)
        {
            _importExportManager = importExportManager;
            _logger = logger;
        }

        [HttpGet]
        public ActionResult Documents()
        {
            //if (TempData.ContainsKey("messages"))
            ViewBag.Messages = TempData.Get<ImportDocumentsResult>("messages");
            if (TempData.ContainsKey("import-status"))
                ViewBag.ImportStatus = TempData["import-status"];
            if (TempData.ContainsKey("export-status"))
                ViewBag.ExportStatus = TempData["export-status"];
            return View();
        }

        [HttpGet]
        public async Task<ActionResult> ExportDocuments()
        {
            try
            {
                byte[] file = await _importExportManager.ExportDocumentsToExcel();
                TempData["export-status"] = "Documents successfully exported.";
                return File(file, ImportExportManager.XlsxContentType,
                    "MrCMS-Documents-" + DateTime.UtcNow + ".xlsx");
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Error exporting documents");
                TempData["export-status"] =
                    "Documents exporting has failed. Please try again and contact system administration if error continues to appear.";
                return RedirectToAction("Documents");
            }
        }

        [HttpPost]
        public async Task<ActionResult> ImportDocuments(IFormFile document)
        {
            if (document != null && document.Length > 0 &&
                document.ContentType == ImportExportManager.XlsxContentType)
                TempData.Set(await _importExportManager.ImportDocumentsFromExcel(document.OpenReadStream()),
                    "messages");
            else
                TempData["import-status"] = "Please choose non-empty Excel (.xslx) file before uploading.";
            return RedirectToAction("Documents");
        }

        [HttpPost]
        public async Task<ActionResult> ExportDocumentsToEmail(ExportDocumentsModel model)
        {
            try
            {
                await _importExportManager.ExportDocumentsToEmail(model);
                TempData["export-status"] = "Documents successfully exported.";
            }
            catch
            {
                TempData["export-status"] =
                    "Documents exporting has failed. Please try again and contact system administration if error continues to appear.";
            }

            return RedirectToAction("Documents");
        }
    }
}