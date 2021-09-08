using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using MrCMS.Entities.Documents.Media;
using MrCMS.Entities.Multisite;
using MrCMS.Helpers;
using MrCMS.Services;
using MrCMS.Web.Admin.Models;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Linq;
using X.PagedList;

namespace MrCMS.Web.Admin.Services
{
    public class MediaSelectorService : IMediaSelectorService
    {
        private readonly ISession _session;
        private readonly IFileService _fileService;
        private readonly IImageProcessor _imageProcessor;
        private readonly ICurrentSiteLocator _siteLocator;

        public MediaSelectorService(ISession session, IFileService fileService, IImageProcessor imageProcessor,
            ICurrentSiteLocator siteLocator)
        {
            _session = session;
            _fileService = fileService;
            _imageProcessor = imageProcessor;
            _siteLocator = siteLocator;
        }

        public async Task<IPagedList<MediaFile>> Search(MediaSelectorSearchQuery searchQuery)
        {
            var site = _siteLocator.GetCurrentSite();
            var queryOver = _session.QueryOver<MediaFile>().Where(file => file.Site.Id == site.Id);
            if (searchQuery.CategoryId.HasValue)
                queryOver = queryOver.Where(file => file.MediaCategory.Id == searchQuery.CategoryId);
            if (!string.IsNullOrWhiteSpace(searchQuery.Query))
            {
                var term = searchQuery.Query.Trim();
                queryOver =
                    queryOver.Where(
                        file =>
                            file.FileName.IsLike(term, MatchMode.Anywhere) ||
                            file.Title.IsLike(term, MatchMode.Anywhere) ||
                            file.Description.IsLike(term, MatchMode.Anywhere));
            }

            return await queryOver.OrderBy(file => file.CreatedOn).Desc.PagedAsync(searchQuery.Page);
        }

        public async Task<List<SelectListItem>> GetCategories()
        {
            var site = _siteLocator.GetCurrentSite();
            var listAsync = await _session.Query<MediaCategory>()
                .Where(category => category.Site.Id == site.Id)
                .Where(category => category.HideInAdminNav != true)
                .WithOptions(options => options.SetCacheable(true))
                .Select(x => new { x.Id, x.Name })
                .ToListAsync();
            return
                listAsync
                    .BuildSelectItemList(category => category.Name, category => category.Id.ToString(),
                        emptyItemText: "All categories");
        }

        public async Task<SelectedItemInfo> GetFileInfo(string value)
        {
            var file = await _fileService.GetFile(value);
            if (file == null)
            {
                return null;
            }

            return new SelectedItemInfo
            {
                Url = await _fileService.GetFileUrl(file, value),
                Alt = file.Title,
                Description = file.Description,
            };
        }

        public async Task<string> GetAlt(string url)
        {
            var mediaFile = await GetImage(url);
            return mediaFile != null ? mediaFile.Title : string.Empty;
        }

        public async Task<string> GetDescription(string url)
        {
            var mediaFile = await GetImage(url);
            return mediaFile != null ? mediaFile.Description : string.Empty;
        }

        private async Task<MediaFile> GetImage(string url)
        {
            return await _imageProcessor.GetImage(url);
        }

        public async Task<bool> UpdateAlt(UpdateMediaParams updateMediaParams, CancellationToken token)
        {
            var mediaFile = await GetImage(updateMediaParams.Url);
            if (mediaFile == null)
                return false;
            mediaFile.Title = updateMediaParams.Value;
            await _session.TransactAsync((session, ct) => session.UpdateAsync(mediaFile, ct), token);
            return true;
        }

        public async Task<bool> UpdateDescription(UpdateMediaParams updateMediaParams, CancellationToken token)
        {
            var mediaFile = await GetImage(updateMediaParams.Url);
            if (mediaFile == null)
                return false;
            mediaFile.Description = updateMediaParams.Value;
            await _session.TransactAsync((session, ct) => session.UpdateAsync(mediaFile, ct), token);
            return true;
        }
    }
}