using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc.Rendering;
using MrCMS.Data;
using MrCMS.Entities.Documents.Layout;
using MrCMS.Helpers;
using MrCMS.Models;
using MrCMS.Services;
using MrCMS.Web.Admin.Models;
using NHibernate.Linq;

namespace MrCMS.Web.Admin.Services
{
    public class LayoutAdminService : ILayoutAdminService
    {
        private readonly IRepository<Layout> _layoutRepository;
        private readonly IGetDocumentsByParent<Layout> _getDocumentsByParent;
        private readonly IUrlValidationService _urlValidationService;
        private readonly IMapper _mapper;

        public LayoutAdminService(IRepository<Layout> layoutRepository,
            IGetDocumentsByParent<Layout> getDocumentsByParent, IUrlValidationService urlValidationService,
            IMapper mapper)
        {
            _layoutRepository = layoutRepository;
            _getDocumentsByParent = getDocumentsByParent;
            _urlValidationService = urlValidationService;
            _mapper = mapper;
        }

        public AddLayoutModel GetAddLayoutModel(int? id)
        {
            return new AddLayoutModel()
            {
                ParentId = id
            };
        }

        public async Task<Layout> GetLayout(int? id) => id.HasValue ? await _layoutRepository.Get(id.Value) : null;

        public async Task<Layout> Add(AddLayoutModel model)
        {
            var layout = _mapper.Map<Layout>(model);
            await _layoutRepository.Add(layout);
            return layout;
        }

        public async Task<UpdateLayoutModel> GetEditModel(int id)
        {
            var layout = await GetLayout(id);
            return _mapper.Map<UpdateLayoutModel>(layout);
        }

        public async Task<List<LayoutArea>> GetLayoutAreas(int id)
        {
            var layout = await GetLayout(id);
            return layout.GetLayoutAreas().Distinct().ToList();
        }

        public async Task Update(UpdateLayoutModel model)
        {
            var layout = await GetLayout(model.Id);
            _mapper.Map(model, layout);
            await _layoutRepository.Update(layout);
        }

        public async Task<Layout> Delete(int id)
        {
            var layout = await GetLayout(id);
            await _layoutRepository.Delete(layout);
            return layout;
        }

        public async Task<List<SortItem>> GetSortItems(int? parent)
        {
            var layout = await GetLayout(parent);
            var documents = await _getDocumentsByParent.GetDocuments(layout);
            return documents
                .Select(
                    arg => new SortItem {Order = arg.DisplayOrder, Id = arg.Id, Name = arg.Name})
                .OrderBy(x => x.Order)
                .ToList();
        }

        public async Task SetOrders(List<SortItem> items)
        {
            await _layoutRepository.TransactAsync(async repository =>
            {
                foreach (var item in items)
                {
                    var mediaFile = await repository.Get(item.Id);
                    mediaFile.DisplayOrder = item.Order;
                    await repository.Update(mediaFile);
                }
            });
        }

        public async Task<bool> UrlIsValidForLayout(string urlSegment, int? id)
        {
            return await _urlValidationService.UrlIsValidForLayout(urlSegment, id);
        }

        public async Task<IEnumerable<SelectListItem>> GetValidParents(int id)
        {
            var layout = await GetLayout(id);

            IList<Layout> potentialParents = await _layoutRepository.Query().ToListAsync();
            List<SelectListItem> result = potentialParents.Distinct()
                .Where(page => page.Id != layout.Id)
                .OrderBy(x => x.Name).BuildSelectItemList(page => page.Name, page => page.Id.ToString(),
                    webpage1 => layout.Parent != null && layout.ParentId == webpage1.Id, emptyItem: null);

            return result.Prepend(new SelectListItem
                {Value = string.Empty, Text = "Root", Selected = layout.Parent == null});
        }

        public async Task SetParent(int id, int? parentId)
        {
            var layout = await GetLayout(id);
            if (layout == null) return;

            Layout parent = parentId.HasValue ? await _layoutRepository.Get(parentId.Value) : null;

            layout.Parent = parent;

            await _layoutRepository.Update(layout);
        }
    }
}