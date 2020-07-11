using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cameo.API.ViewModels;
using Cameo.Models;
using Cameo.Models.Enums;
using Cameo.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cameo.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TalentsController : BaseController
    {
        private readonly ITalentService TalentService;
        private readonly ITalentSearchService TalentSearchService;
        private readonly ICategoryService CategoryService;
        private readonly IVideoRequestTypeService VideoRequestTypeService;
        private readonly IVideoRequestService VideoRequestService;

        public TalentsController(
            ITalentService talentService,
            ITalentSearchService talentSearchService,
            ICategoryService categoryService,
            IVideoRequestTypeService videoRequestTypeService,
            IVideoRequestService videoRequestService)
        {
            TalentService = talentService;
            TalentSearchService = talentSearchService;
            CategoryService = categoryService;
            VideoRequestTypeService = videoRequestTypeService;
            VideoRequestService = videoRequestService;
        }

        [HttpGet("GetCategorized")]
        public ActionResult<IEnumerable<TalentsCategorizedVM>> GetCategorized()
        {
            try
            {
                List<TalentsCategorizedVM> talentsCategorizedVM = new List<TalentsCategorizedVM>();
                List<Category> categories = new List<Category>();

                var featuredTalents = TalentSearchService.GetFeatured(null, 6);
                if (featuredTalents.Count() > 0)
                {
                    var featuredTalentsVM = new TalentsCategorizedVM(
                        new Category()
                        {
                            ID = (int)CategoryEnum.featured,
                            Name = "Популярные"
                        },
                        featuredTalents.ToList());
                    talentsCategorizedVM.Add(featuredTalentsVM);
                }

                var newTalents = TalentSearchService.GetNew(null, 6);
                if (newTalents.Count() > 0)
                {
                    var newTalentsVM = new TalentsCategorizedVM(
                        new Category()
                        {
                            ID = (int)CategoryEnum.neW,
                            Name = "Новые"
                        },
                        newTalents.ToList());
                    talentsCategorizedVM.Add(newTalentsVM);
                }

                categories.AddRange(CategoryService.GetAllActive());
                foreach (var category in categories)
                {
                    var talentsCategorized = TalentSearchService.Search(category.ID, SortTypeEnum.def, 6);
                    if (talentsCategorized.Count() > 0)
                    {
                        var categoryTalentsVM = new TalentsCategorizedVM(category, talentsCategorized.ToList());

                        talentsCategorizedVM.Add(categoryTalentsVM);
                    }
                }

                foreach (var categoryTalentsVM in talentsCategorizedVM)
                {
                    foreach (var talent in categoryTalentsVM.talents)
                    {
                        if (talent.avatar == null || talent.avatar.id == 0)
                        {
                            talent.avatar = new AttachmentDetailsVM();
                            talent.avatar.url = TalentService.GetRandomPhotoUrl();
                        }
                    }
                }

                return talentsCategorizedVM;
            }
            catch (Exception ex)
            {
                return CustomBadRequest(ex);
            }
        }

        [HttpGet("GetByCategory")]
        public ActionResult<IEnumerable<TalentsCategorizedVM>> GetByCategory(int category_id)
        {
            try
            {
                //1. if category = new then
                //  - all in new
                //2. if category = featured then:
                //  - featured
                //  - new in featured
                //  - all in featured
                //3. else (if normal category)
                //  - featured in category
                //  - new in category
                //  - all in category

                List<TalentsCategorizedVM> talentsCategorizedVM = new List<TalentsCategorizedVM>();

                if (category_id == (int)CategoryEnum.neW)
                {
                    var newTalents = TalentSearchService.GetNew(null);
                    if (newTalents.Count() > 0)
                    {
                        var newTalentsVM = new TalentsCategorizedVM(
                            new Category()
                            {
                                ID = (int)CategoryEnum.neW,
                                Name = "Все новые"
                            },
                            newTalents.ToList());
                        talentsCategorizedVM.Add(newTalentsVM);
                    }
                }
                else if (category_id == (int)CategoryEnum.featured)
                {
                    //DO NOT DELETE THIS CODE!!!
                    //must be uncommented later when number of telants will be large

                    //var featuredTalents = TalentService.GetFeatured(null, 6);
                    //if (featuredTalents.Count() > 0)
                    //{
                    //    var featuredTalentsVM = new TalentsCategorizedVM(
                    //        new Category()
                    //        {
                    //            ID = (int)CategoryEnum.featured,
                    //            Name = "Популярные"
                    //        },
                    //        featuredTalents.ToList());
                    //    talentsCategorizedVM.Add(featuredTalentsVM);
                    //}

                    //var newTalents = TalentService.GetNewInFeatured(6);
                    //if (newTalents.Count() > 0)
                    //{
                    //    var newTalentsVM = new TalentsCategorizedVM(
                    //        new Category()
                    //        {
                    //            ID = (int)CategoryEnum.neW,
                    //            Name = "Новые"
                    //        },
                    //        newTalents.ToList());
                    //    talentsCategorizedVM.Add(newTalentsVM);
                    //}

                    var allTalents = TalentSearchService.GetFeatured(null);
                    if (allTalents.Count() > 0)
                    {
                        var allTalentsVM = new TalentsCategorizedVM(
                            new Category()
                            {
                                ID = (int)CategoryEnum.neW,
                                Name = "Все Популярные"
                            },
                            allTalents.ToList());
                        talentsCategorizedVM.Add(allTalentsVM);
                    }
                }
                else
                {
                    var categoryDB = CategoryService.GetActiveByID(category_id);
                    if (categoryDB == null)
                        return NotFound("Category not found");

                    //DO NOT DELETE THIS CODE!!!
                    //must be uncommented later when number of telants will be large

                    //var featuredTalents = TalentService.GetFeatured(categoryID, 6);
                    //if (featuredTalents.Count() > 0)
                    //{
                    //    var featuredTalentsVM = new TalentsCategorizedVM(
                    //        new Category()
                    //        {
                    //            ID = (int)CategoryEnum.featured,
                    //            Name = "Популярные"
                    //        },
                    //        featuredTalents.ToList());
                    //    talentsCategorizedVM.Add(featuredTalentsVM);
                    //}

                    //var newTalents = TalentService.GetNew(categoryID, 6);
                    //if (newTalents.Count() > 0)
                    //{
                    //    var newTalentsVM = new TalentsCategorizedVM(
                    //        new Category()
                    //        {
                    //            ID = (int)CategoryEnum.neW,
                    //            Name = "Новые"
                    //        },
                    //        newTalents.ToList());
                    //    talentsCategorizedVM.Add(newTalentsVM);
                    //}

                    var allTalents = TalentSearchService.Search(category_id, SortTypeEnum.def);
                    if (allTalents.Count() > 0)
                    {
                        var allTalentsVM = new TalentsCategorizedVM(
                            new Category()
                            {
                                ID = (int)CategoryEnum.neW,
                                Name = "Все"
                            },
                            allTalents.ToList());
                        talentsCategorizedVM.Add(allTalentsVM);
                    }
                }

                foreach (var categoryTalentsVM in talentsCategorizedVM)
                {
                    foreach (var talent in categoryTalentsVM.talents)
                    {
                        if (talent.avatar == null || talent.avatar.id == 0)
                        {
                            talent.avatar = new AttachmentDetailsVM();
                            talent.avatar.url = TalentService.GetRandomPhotoUrl();
                        }
                    }
                }

                return talentsCategorizedVM;
            }
            catch (Exception ex)
            {
                return CustomBadRequest(ex);
            }
        }

        [HttpGet("GetBySearchText")]
        public ActionResult<IEnumerable<TalentShortInfoVM>> GetBySearchText(string search_text)
        {
            try
            {
                var talents = TalentSearchService.SearchBySearchText(search_text)
                .Select(m => new TalentShortInfoVM(m))
                .ToList();

                foreach (var item in talents)
                {
                    if (item.avatar == null || item.avatar.id == 0)
                    {
                        item.avatar = new AttachmentDetailsVM();
                        item.avatar.url = TalentService.GetRandomPhotoUrl();
                    }
                }

                return talents;
            }
            catch (Exception ex)
            {
                return CustomBadRequest(ex);
            }
        }

        [HttpGet("{id}")]
        public ActionResult<TalentDetailsVM> Get(int id)
        {
            try
            {
                var curUser = accountUtil.GetCurrentUser(User);
                Talent model = TalentService.GetActiveSingleDetailsWithRelatedDataByID(id);
                if (model == null)
                    return CustomBadRequest("Талант не найден");

                TalentDetailsVM modelVM = new TalentDetailsVM(model);

                if (modelVM.avatar == null || modelVM.avatar.id == 0)
                {
                    modelVM.avatar = new AttachmentDetailsVM();
                    modelVM.avatar.url = TalentService.GetRandomPhotoUrl();
                }

                //VideoRequest videoRequest = VideoRequestService.GetRandomSinglePublishedByTalent(model, curUser.ID);
                //if (videoRequest != null)
                //{
                //    modelVM.Video = new AttachmentDetailsVM(videoRequest.Video);
                //    modelVM.RequestID = videoRequest.ID;
                //}

                return modelVM;
            }
            catch (Exception ex)
            {
                return CustomBadRequest(ex);
            }
        }

        [HttpGet("GetLatestVideosForTalent/{id}")]
        public ActionResult<IEnumerable<VideoDetailsVM>> GetLatestVideosForTalent(int id)
        {
            try
            {
                Talent talent = TalentService.GetActiveByID(id);
                if (talent == null)
                    return CustomBadRequest("Талант не найден");

                List<VideoRequest> videos = VideoRequestService.GetPublicByTalent(talent, 0)
                    .ToList();

                List<VideoDetailsVM> videosVM = new List<VideoDetailsVM>();
                foreach (var item in videos)
                {
                    videosVM.Add(new VideoDetailsVM(item));
                }

                return videosVM;
            }
            catch (Exception ex)
            {
                return CustomBadRequest(ex);
            }
        }

        [HttpGet("GetRelated/{id}")]
        public ActionResult<IEnumerable<TalentGridViewItem>> GetRelated(int id, int? count = null)
        {
            try
            {
                Talent talent = TalentService.GetActiveByID(id);
                if (talent == null)
                    return CustomBadRequest("Талант не найден");

                List<TalentGridViewItem> relatedTalents = TalentSearchService.GetRelated(talent, count)
                    .Select(m => new TalentGridViewItem(m))
                    .ToList();

                foreach (var talentItem in relatedTalents)
                {
                    if (talentItem.avatar == null || talentItem.avatar.id == 0)
                    {
                        talentItem.avatar = new AttachmentDetailsVM();
                        talentItem.avatar.url = TalentService.GetRandomPhotoUrl();
                    }
                }

                return relatedTalents;
            }
            catch (Exception ex)
            {
                return CustomBadRequest(ex);
            }
        }
    }
}
