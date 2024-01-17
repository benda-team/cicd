using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using ODTradePromotion.API.Models.Base;
using ODTradePromotion.API.Models.Paging;
using ODTradePromotion.API.Models.Promotion;
using ODTradePromotion.API.Models.Settlement;
using ODTradePromotion.API.Services.Principals;
using ODTradePromotion.API.Services.Promotion;
using ODTradePromotion.API.Services.User;
using RestSharp;
using Sys.Common.Constants;
using Sys.Common.JWT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Security.Claims;
using System.Threading.Tasks;
using static Sys.Common.Constants.ErrorCodes;

namespace ODTradePromotion.API.Controllers
{
    [Authorize]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    //[AllowAnonymous]
    [ApiVersion("1.0")]
    public class TpPromotionController : BaseController
    {
        private readonly IPromotionService _promotionService;
        private readonly IUserService _userService;
        private readonly IPrincipalService principalService;
        private readonly IPromotionExternalService promotionExternalService;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly string token;

        public TpPromotionController(
            IPromotionService promotionService,
            IUserService userService,
            IPrincipalService principalService,
            IPromotionExternalService promotionExternalService,
            IHttpContextAccessor httpContextAccessor
            )
        {
            _promotionService = promotionService;
            _userService = userService;
            this.principalService = principalService;
            this.promotionExternalService = promotionExternalService;
            this.httpContextAccessor = httpContextAccessor;
            token = httpContextAccessor.HttpContext.Request.Headers["Authorization"];
        }

        /// <summary>
        /// Get List Promotion
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetListPromotion")]
        [MapToApiVersion("1.0")]
        public IActionResult GetListPromotion(EcoParameters parameters)
        {
            try
            {
                var featureListTemp = _promotionService.GetListTpPromotion();
                // check searching
                if (parameters.Filter != null && parameters.Filter.Trim() != string.Empty && parameters.Filter.Trim() != "NA_EMPTY")
                {
                    var optionsAssembly = ScriptOptions.Default.AddReferences(typeof(PromotionDiscountModel).Assembly);
                    var filterExpressionTemp = CSharpScript.EvaluateAsync<Func<PromotionDiscountModel, bool>>(($"s=> {parameters.Filter}"), optionsAssembly);
                    Func<PromotionDiscountModel, bool> filterExpression = filterExpressionTemp.Result;

                    var checkCondition = featureListTemp.Where(filterExpression);
                    featureListTemp = checkCondition.AsQueryable();
                }

                // Check Orderby
                if (parameters.OrderBy != null && parameters.OrderBy.Trim() != string.Empty && parameters.OrderBy.Trim() != "NA_EMPTY")
                {
                    featureListTemp = featureListTemp.OrderBy(parameters.OrderBy);
                }
                else
                {
                    featureListTemp = featureListTemp.OrderBy(x => x.Code);
                }

                // Check Dropdown
                if (parameters.IsDropdown)
                {
                    var featureListTempPagged1 = PagedList<PromotionDiscountModel>.ToPagedList(featureListTemp.ToList(), 0, featureListTemp.Count());

                    return Ok(new PromotionDiscountModelListModel { Items = featureListTempPagged1 });
                }

                int totalCount = featureListTemp.Count();
                int skip = parameters.Skip ?? 0;
                int top = parameters.Top ?? parameters.PageSize;
                var items = featureListTemp.Skip(skip).Take(top).ToList();
                var result = new PagedList<PromotionDiscountModel>(items, totalCount, (skip / top) + 1, top);
                return Ok(new PromotionDiscountModelListModel { Items = result, MetaData = result.MetaData });
            }
            catch (Exception ex)
            {
                return Ok(new BaseResultModel
                {
                    IsSuccess = false,
                    Code = Convert.ToInt32(PromotionError.ListPromotionDiscountFailed),
                    Message = ex.InnerException?.Message ?? ex.Message
                });
            }
        }

        /// <summary>
        /// Get List Promotion By Dapper
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetListPromotionByDapper")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetListPromotionByDapper(PromotionDiscountSearchModel parameters)
        {
            try
            {
                var featureListTemp = await _promotionService.GetListTpPromotionDapper(parameters);

                // Check Dropdown
                if (parameters.IsDropdown)
                {
                    var featureListTempPagged1 = PagedList<PromotionDiscountModel>.ToPagedList(featureListTemp.ToList(), 0, featureListTemp.Count);

                    return Ok(new PromotionDiscountModelListModel { Items = featureListTempPagged1 });
                }

                int totalCount = featureListTemp.Count;
                int skip = parameters.Skip ?? 0;
                int top = parameters.Top ?? parameters.PageSize;
                var items = featureListTemp.Skip(skip).Take(top).ToList();
                var result = new PagedList<PromotionDiscountModel>(items, totalCount, (skip / top) + 1, top);
                return Ok(new PromotionDiscountModelListModel { Items = result, MetaData = result.MetaData });
            }
            catch (Exception ex)
            {
                return Ok(new BaseResultModel
                {
                    IsSuccess = false,
                    Code = Convert.ToInt32(PromotionError.ListPromotionDiscountFailed),
                    Message = ex.InnerException?.Message ?? ex.Message
                });
            }
        }

        /// <summary>
        /// Get List Promotion Code
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetListPromotionCode/{status}")]
        [MapToApiVersion("1.0")]
        public IActionResult GetListPromotionCode(string status)
        {
            try
            {
                var data = _promotionService.GetListPromotionCode(status).ToList();
                return Ok(data);
            }
            catch (Exception ex)
            {
                return Ok(new BaseResultModel
                {
                    IsSuccess = false,
                    Code = Convert.ToInt32(PromotionError.GetListPromotionPopupFailed),
                    Message = ex.InnerException?.Message ?? ex.Message
                });
            }
        }

        /// <summary>
        /// Get List Promotion Code Dapper
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetListPromotionCodeDapper/{status}")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetListPromotionCodeDapper(string status)
        {
            try
            {
                var data = await _promotionService.GetListPromotionCodeDapper(status);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return Ok(new BaseResultModel
                {
                    IsSuccess = false,
                    Code = Convert.ToInt32(PromotionError.GetListPromotionCodeDapperFailed),
                    Message = ex.InnerException?.Message ?? ex.Message
                });
            }
        }

        /// <summary>
        /// Get List Promotion Popup
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetListPromotionPopup")]
        [MapToApiVersion("1.0")]
        public IActionResult GetListPromotionPopup(EcoParameters parameters)
        {
            try
            {
                var featureListTemp = _promotionService.GetListPromotionPopup(parameters.UserName, parameters.FeatureCode);
                // check searching
                if (parameters.Filter != null && parameters.Filter.Trim() != string.Empty && parameters.Filter.Trim() != "NA_EMPTY")
                {
                    var optionsAssembly = ScriptOptions.Default.AddReferences(typeof(PromotionPopupModel).Assembly);
                    var filterExpressionTemp = CSharpScript.EvaluateAsync<Func<PromotionPopupModel, bool>>(($"s=> {parameters.Filter}"), optionsAssembly);
                    Func<PromotionPopupModel, bool> filterExpression = filterExpressionTemp.Result;

                    var checkCondition = featureListTemp.Where(filterExpression);
                    featureListTemp = checkCondition.AsQueryable();
                }

                // Check Orderby
                if (parameters.OrderBy != null && parameters.OrderBy.Trim() != string.Empty && parameters.OrderBy.Trim() != "NA_EMPTY")
                {
                    featureListTemp = featureListTemp.OrderBy(parameters.OrderBy);
                }
                else
                {
                    featureListTemp = featureListTemp.OrderBy(x => x.Code);
                }

                // Check Dropdown
                if (parameters.IsDropdown)
                {
                    var featureListTempPagged1 = PagedList<PromotionPopupModel>.ToPagedList(featureListTemp.ToList(), 0, featureListTemp.Count());

                    return Ok(new ListPromotionPopupModel { Items = featureListTempPagged1 });
                }

                int totalCount = featureListTemp.Count();
                int skip = parameters.Skip ?? 0;
                int top = parameters.Top ?? parameters.PageSize;
                var items = featureListTemp.Skip(skip).Take(top).ToList();
                var result = new PagedList<PromotionPopupModel>(items, totalCount, (skip / top) + 1, top);
                return Ok(new ListPromotionPopupModel { Items = result, MetaData = result.MetaData });
            }
            catch (Exception ex)
            {
                return Ok(new BaseResultModel
                {
                    IsSuccess = false,
                    Code = Convert.ToInt32(PromotionError.GetListPromotionPopupFailed),
                    Message = ex.InnerException?.Message ?? ex.Message
                });
            }
        }

        /// <summary>
        /// Get List Promotion General
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetListPromotionGeneral")]
        [MapToApiVersion("1.0")]
        public IActionResult GetListPromotionGeneral(EcoParameters parameters)
        {
            try
            {
                var featureListTemp = _promotionService.GetListPromotionGeneral();

                // check searching
                if (parameters.Filter != null && parameters.Filter.Trim() != string.Empty && parameters.Filter.Trim() != "NA_EMPTY")
                {
                    var optionsAssembly = ScriptOptions.Default.AddReferences(typeof(TpPromotionGeneralModel).Assembly);
                    var filterExpressionTemp = CSharpScript.EvaluateAsync<Func<TpPromotionGeneralModel, bool>>(($"s=> {parameters.Filter}"), optionsAssembly);
                    Func<TpPromotionGeneralModel, bool> filterExpression = filterExpressionTemp.Result;

                    var checkCondition = featureListTemp.Where(filterExpression);
                    featureListTemp = checkCondition.AsQueryable();
                }

                // Check Orderby
                if (parameters.OrderBy != null && parameters.OrderBy.Trim() != string.Empty && parameters.OrderBy.Trim() != "NA_EMPTY")
                {
                    featureListTemp = featureListTemp.OrderBy(parameters.OrderBy);
                }
                else
                {
                    featureListTemp = featureListTemp.OrderBy(x => x.Code);
                }

                // Check Dropdown
                if (parameters.IsDropdown)
                {
                    var featureListTempPagged1 = PagedList<TpPromotionGeneralModel>.ToPagedList(featureListTemp.ToList(), 0, featureListTemp.Count());

                    return Ok(new TpPromotionGeneralListModel { Items = featureListTempPagged1 });
                }

                int totalCount = featureListTemp.Count();
                int skip = parameters.Skip ?? 0;
                int top = parameters.Top ?? parameters.PageSize;
                var items = featureListTemp.Skip(skip).Take(top).ToList();
                var result = new PagedList<TpPromotionGeneralModel>(items, totalCount, (skip / top) + 1, top);
                return Ok(new TpPromotionGeneralListModel { Items = result, MetaData = result.MetaData });
            }
            catch (Exception ex)
            {
                return Ok(new BaseResultModel
                {
                    IsSuccess = false,
                    Code = Convert.ToInt32(PromotionError.GetListPromotionPopupFailed),
                    Message = ex.InnerException?.Message ?? ex.Message
                });
            }
        }

        /// <summary>
        /// Get List Promotion General For Selettlement
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetListPromotionGeneralForSelettlement")]
        [MapToApiVersion("1.0")]
        public IActionResult GetListPromotionGeneralForSelettlement(EcoParameters parameters)
        {
            try
            {
                var featureListTemp = _promotionService.GetListPromotionGeneralForSelettlement(parameters.SearchValue);

                // check searching
                if (parameters.Filter != null && parameters.Filter.Trim() != string.Empty && parameters.Filter.Trim() != "NA_EMPTY")
                {
                    var optionsAssembly = ScriptOptions.Default.AddReferences(typeof(TpPromotionGeneralModel).Assembly);
                    var filterExpressionTemp = CSharpScript.EvaluateAsync<Func<TpPromotionGeneralModel, bool>>(($"s=> {parameters.Filter}"), optionsAssembly);
                    Func<TpPromotionGeneralModel, bool> filterExpression = filterExpressionTemp.Result;

                    var checkCondition = featureListTemp.Where(filterExpression);
                    featureListTemp = checkCondition.AsQueryable();
                }

                // Check Orderby
                if (parameters.OrderBy != null && parameters.OrderBy.Trim() != string.Empty && parameters.OrderBy.Trim() != "NA_EMPTY")
                {
                    featureListTemp = featureListTemp.OrderBy(parameters.OrderBy);
                }
                else
                {
                    featureListTemp = featureListTemp.OrderBy(x => x.Code);
                }

                // Check Dropdown
                if (parameters.IsDropdown)
                {
                    var featureListTempPagged1 = PagedList<TpPromotionGeneralModel>.ToPagedList(featureListTemp.ToList(), 0, featureListTemp.Count());

                    return Ok(new TpPromotionGeneralListModel { Items = featureListTempPagged1 });
                }

                int totalCount = featureListTemp.Count();
                int skip = parameters.Skip ?? 0;
                int top = parameters.Top ?? parameters.PageSize;
                var items = featureListTemp.Skip(skip).Take(top).ToList();
                var result = new PagedList<TpPromotionGeneralModel>(items, totalCount, (skip / top) + 1, top);
                return Ok(new TpPromotionGeneralListModel { Items = result, MetaData = result.MetaData });
            }
            catch (Exception ex)
            {
                return Ok(new BaseResultModel
                {
                    IsSuccess = false,
                    Code = Convert.ToInt32(PromotionError.GetListPromotionPopupFailed),
                    Message = ex.InnerException?.Message ?? ex.Message
                });
            }
        }

        /// <summary>
        /// Get Settlement Promotion By Promotion Code
        /// </summary>
        /// <param name="promotionCode"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetSettlementPromotionByPromotionCode/{promotionCode}")]
        [MapToApiVersion("1.0")]
        public IActionResult GetSettlementPromotionByPromotionCode(string promotionCode)
        {
            try
            {
                var featureListTemp = _promotionService.GetListPromotionGeneralForSelettlement(string.Empty);
                var promotion = featureListTemp.Where(x => x.Code.ToLower().Equals(promotionCode.ToLower())).FirstOrDefault();
                return Ok(promotion);
            }
            catch (Exception ex)
            {
                return Ok(new BaseResultModel
                {
                    IsSuccess = false,
                    Code = Convert.ToInt32(PromotionError.GetSettlementPromotionByPromotionCodeFailed),
                    Message = ex.InnerException?.Message ?? ex.Message
                });
            }
        }

        /// <summary>
        /// Get List Scheme Promotion
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetListSchemePromotion")]
        [MapToApiVersion("1.0")]
        public IActionResult GetListSchemePromotion(EcoParameters parameters)
        {
            try
            {
                var featureListTemp = _promotionService.GetListPromotionGeneral();
                var feature = (from scheme in featureListTemp
                               select new TpSchemePromotionModel()
                               {
                                   Scheme = scheme.Scheme
                               }).Distinct().AsQueryable();

                // check searching
                if (parameters.Filter != null && parameters.Filter.Trim() != string.Empty && parameters.Filter.Trim() != "NA_EMPTY")
                {
                    var optionsAssembly = ScriptOptions.Default.AddReferences(typeof(TpSchemePromotionModel).Assembly);
                    var filterExpressionTemp = CSharpScript.EvaluateAsync<Func<TpSchemePromotionModel, bool>>(($"s=> {parameters.Filter}"), optionsAssembly);
                    Func<TpSchemePromotionModel, bool> filterExpression = filterExpressionTemp.Result;

                    var checkCondition = feature.Where(filterExpression);
                    feature = checkCondition.AsQueryable();
                }

                // Check Orderby
                if (parameters.OrderBy != null && parameters.OrderBy.Trim() != string.Empty && parameters.OrderBy.Trim() != "NA_EMPTY")
                {
                    feature = feature.OrderBy(parameters.OrderBy);
                }
                else
                {
                    feature = feature.OrderBy(x => x.Scheme);
                }

                // Check Dropdown
                if (parameters.IsDropdown)
                {
                    var featureListTempPagged1 = PagedList<TpSchemePromotionModel>.ToPagedList(feature.ToList(), 0, feature.Count());

                    return Ok(new TpSchemePromotionListModel { Items = featureListTempPagged1 });
                }

                int totalCount = feature.Count();
                int skip = parameters.Skip ?? 0;
                int top = parameters.Top ?? parameters.PageSize;
                var items = feature.Skip(skip).Take(top).ToList();
                var result = new PagedList<TpSchemePromotionModel>(items, totalCount, (skip / top) + 1, top);
                return Ok(new TpSchemePromotionListModel { Items = result, MetaData = result.MetaData });
            }
            catch (Exception ex)
            {
                return Ok(new BaseResultModel
                {
                    IsSuccess = false,
                    Code = Convert.ToInt32(PromotionError.GetListPromotionPopupFailed),
                    Message = ex.InnerException?.Message ?? ex.Message
                });
            }
        }

        /// <summary>
        /// Get List Scheme Promotion By Scheme
        /// </summary>
        /// <param name="scheme"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetListSchemePromotionByScheme/{scheme}")]
        [MapToApiVersion("1.0")]
        public IActionResult GetListSchemePromotionByScheme(string scheme)
        {
            try
            {
                var featureListTemp = _promotionService.GetListPromotionGeneral();
                var feature = (from sc in featureListTemp
                               select new TpSchemePromotionModel()
                               {
                                   Scheme = sc.Scheme
                               }).Distinct().AsQueryable();
                return Ok(feature.Where(x => x.Scheme.ToLower().Equals(scheme.ToLower())).FirstOrDefault());
            }
            catch (Exception ex)
            {
                return Ok(new BaseResultModel
                {
                    IsSuccess = false,
                    Code = Convert.ToInt32(PromotionError.GetListSchemePromotionBySchemeFailed),
                    Message = ex.InnerException?.Message ?? ex.Message
                });
            }
        }

        /// <summary>
        /// Create Promotion
        /// </summary>
        /// <param name="input">TpPromotionModel</param>
        /// <returns></returns>
        [HttpPost]
        [Route("CreatePromotion")]
        [MapToApiVersion("1.0")]
        public IActionResult CreatePromotion(TpPromotionModel input)
        {
            try
            {
                var existItem = _promotionService.GetGeneralPromotionByCode(input.Code);
                if (existItem == null)
                {
                    return Ok(_promotionService.CreatePromotion(input, input.UserName));
                }
                else
                {
                    return Ok(new BaseResultModel
                    {
                        IsSuccess = false,
                        Code = Convert.ToInt32(PromotionError.PromotionCodeIsExist),
                        Message = "PromotionCodeIsExist"
                    });
                }
            }
            catch (Exception ex)
            {
                return Ok(new BaseResultModel
                {
                    IsSuccess = false,
                    Code = Convert.ToInt32(PromotionError.CreatePromotionFailed),
                    Message = ex.InnerException?.Message ?? ex.Message
                });
            }
        }
        /// <summary>
        /// Create Promotion
        /// </summary>
        /// <param name="input">TpPromotionModel</param>
        /// <returns></returns>
        [HttpPost]
        [Route("Sync/CreatePromotion")]
        [MapToApiVersion("1.0")]
        public IActionResult SyncCreatePromotion(TpPromotionModel input)
        {
            try
            {
                var existItem = _promotionService.GetGeneralPromotionByCode(input.Code);
                if (existItem == null)
                {
                    return Ok(_promotionService.CreatePromotion(input, input.UserName, true));
                }
                else
                {
                    return Ok(new BaseResultModel
                    {
                        IsSuccess = false,
                        Code = Convert.ToInt32(PromotionError.PromotionCodeIsExist),
                        Message = "PromotionCodeIsExist"
                    });
                }
            }
            catch (Exception ex)
            {
                return Ok(new BaseResultModel
                {
                    IsSuccess = false,
                    Code = Convert.ToInt32(PromotionError.CreatePromotionFailed),
                    Message = ex.InnerException?.Message ?? ex.Message
                });
            }
        }

        /// <summary>
        /// Get General Promotion By Code By User Create
        /// </summary>
        /// <param name="code">code</param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetGeneralPromotionByCode/{code}")]
        [MapToApiVersion("1.0")]
        public IActionResult GetGeneralPromotionByCode(string code)
        {
            try
            {
                var data = _promotionService.GetGeneralPromotionByCode(code);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return Ok(new BaseResultModel
                {
                    IsSuccess = false,
                    Code = Convert.ToInt32(PromotionError.GetGeneralPromotionByCodeFailed),
                    Message = ex.InnerException?.Message ?? ex.Message
                });
            }
        }

        /// <summary>
        /// Get Detail Promotion By Code
        /// </summary>
        /// <param name="code">code</param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetDetailPromotionByCode/{code}")]
        [MapToApiVersion("1.0")]
        public IActionResult GetDetailPromotionByCode(string code)
        {
            try
            {
                var data = _promotionService.GetDetailPromotionByCode(code);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return Ok(new BaseResultModel
                {
                    IsSuccess = false,
                    Code = Convert.ToInt32(PromotionError.GetDetailPromotionByCodeFailed),
                    Message = ex.InnerException?.Message ?? ex.Message
                });
            }
        }

        /// <summary>
        /// Get Detail Promotion By Code Using Dapper
        /// </summary>
        /// <param name="code">code</param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetDetailPromotionByCodeDapper/{code}")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetDetailPromotionByCodeDapper(string code)
        {
            try
            {
                var data = await _promotionService.GetDetailPromotionByCodeDapper(code);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return Ok(new BaseResultModel
                {
                    IsSuccess = false,
                    Code = Convert.ToInt32(PromotionError.GetDetailPromotionByCodeFailed),
                    Message = ex.InnerException?.Message ?? ex.Message
                });
            }
        }

        /// <summary>
        /// Get Detail Promotion By Code By User Approve By FeatureCode
        /// </summary>
        /// <param name="code">code</param>
        /// <param name="username">username</param>
        /// <param name="featurecode">featurecode</param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetDetailPromotionByCodeByUserApprove")]
        [MapToApiVersion("1.0")]
        public IActionResult GetDetailPromotionByCodeByUserApprove(string code, string username, string featurecode)
        {
            try
            {
                var allRoles = _userService.GetListRoleInfoByUserName(username);
                var actions = new List<string>();
                var highScores = allRoles.Select(m => new
                {
                    Features = m.FeatureActions.Where(u => u.Code.ToLower() == featurecode.ToLower()).ToList()
                }).ToList();

                foreach (var item in highScores)
                {
                    foreach (var function in item.Features)
                    {
                        foreach (var action in function.Actions)
                        {
                            actions.Add(action.Code);
                        }
                    }
                }

                var data = _promotionService.GetDetailPromotionByCode(code);
                if (data != null && data.UserName.ToLower().Equals(username.ToLower()))
                {
                    return Ok(data);
                }
                return Ok(new BaseResultModel
                {
                    IsSuccess = false,
                    Code = Convert.ToInt32(PromotionError.PromotionCodeIsNotExist),
                    Message = "PromotionCodeIsNotExist"
                });
            }
            catch (Exception ex)
            {
                return Ok(new BaseResultModel
                {
                    IsSuccess = false,
                    Code = Convert.ToInt32(PromotionError.GetDetailPromotionByCodeFailed),
                    Message = ex.InnerException?.Message ?? ex.Message
                });
            }
        }

        /// <summary>
        /// UpdatePromotion
        /// </summary>
        /// <param name="input">TpPromotionModel</param>
        /// <returns></returns>
        [HttpPut]
        [Route("UpdatePromotion")]
        [MapToApiVersion("1.0")]
        public IActionResult UpdatePromotion(TpPromotionModel input)
        {
            try
            {
                DateTime dt = DateTime.Now;
                if (input.EffectiveDateFrom.Date == dt.Date)
                {
                    input.EffectiveDateFrom = input.EffectiveDateFrom + dt.TimeOfDay;
                }
                else
                {
                    TimeSpan time = new TimeSpan(00, 00, 00);
                    input.EffectiveDateFrom = input.EffectiveDateFrom.Date + time;
                }
                TimeSpan ts = new TimeSpan(23, 59, 59);
                input.ValidUntil = input.ValidUntil + ts;
                // Check Exist Function Code
                var existItemCode = _promotionService.GetGeneralPromotionByCode(input.Code);
                if (existItemCode != null && _promotionService.UpdatePromotion(input, input.UserName))
                {
                    return Ok(new BaseResultModel
                    {
                        ObjectGuidId = existItemCode.Id,
                        IsSuccess = true
                    });
                }

                return Ok(new BaseResultModel
                {
                    IsSuccess = false,
                    Code = Convert.ToInt32(PromotionError.PromotionCodeIsNotExist),
                    Message = "PromotionCodeIsNotExist" //"Function code not exist, please use code exist."
                });
            }
            catch (Exception ex)
            {
                return Ok(new BaseResultModel
                {
                    IsSuccess = false,
                    Code = Convert.ToInt32(PromotionError.UpdatePromotionFailed),
                    Message = ex.InnerException?.Message ?? ex.Message
                });
            }
        }

        /// <summary>
        /// ConfirmPromotion
        /// </summary>
        /// <param name="request">request</param>
        /// <returns></returns>
        [HttpPut]
        [Route("ConfirmPromotion")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> ConfirmPromotion(ConfirmPromotionReq request)
        {
            string userName = User.Claims.Where(x => x.Type == CustomClaimType.UserName).Select(d => d.Value).FirstOrDefault();
            try
            {
                // Check Exist Function Code
                var existItemCode = _promotionService.GetGeneralPromotionByCode(request.Code);
                if (existItemCode != null && _promotionService.ConfirmPromotion(request))
                {
                    bool isPrincipalSystem = await principalService.IsPrincipalSystem();
                    if (existItemCode.PromotionType == CommonData.PromotionSetting.PromotionProgram &&
                        isPrincipalSystem &&
                        existItemCode.ScopeType == CommonData.PromotionSetting.ScopeNationwide)
                    {
                        await promotionExternalService.HandleSyncToODAsync(request.Code, userName, token);
                        await _promotionService.UpdateSyncedFlagAsync(request.Code, "");
                    }
                    return Ok(new BaseResultModel
                    {
                        ObjectGuidId = existItemCode.Id,
                        IsSuccess = true
                    });
                }

                return Ok(new BaseResultModel
                {
                    IsSuccess = false,
                    Code = Convert.ToInt32(PromotionError.PromotionCodeIsNotExist),
                    Message = "PromotionCodeIsNotExist" //"Function code not exist, please use code exist."
                });
            }
            catch (Exception ex)
            {
                return Ok(new BaseResultModel
                {
                    IsSuccess = false,
                    Code = Convert.ToInt32(PromotionError.UpdatePromotionFailed),
                    Message = ex.InnerException?.Message ?? ex.Message
                });
            }
        }

        /// <summary>
        /// DeleteBudgetByCode
        /// </summary>
        /// <param name="code">code</param>
        /// <returns></returns>
        [HttpDelete]
        [Route("DeletePromotionByCode/{code}")]
        [MapToApiVersion("1.0")]
        public IActionResult DeletePromotionByCode(string code)
        {
            try
            {
                // Check Exist Function Code
                var existItem = _promotionService.GetGeneralPromotionByCode(code);
                if (existItem != null && _promotionService.DeletePromotionByCode(code, existItem.UserName))
                {
                    return Ok(new BaseResultModel
                    {
                        ObjectGuidId = existItem.Id,
                        IsSuccess = true
                    });
                }
                return Ok(new BaseResultModel
                {
                    IsSuccess = false,
                    Code = Convert.ToInt32(PromotionError.PromotionCodeIsNotExist),
                    Message = "PromotionCodeIsNotExist"
                });
            }
            catch (Exception ex)
            {
                return Ok(new BaseResultModel
                {
                    IsSuccess = false,
                    Code = Convert.ToInt32(PromotionError.DeletePromotionByCodeFailed),
                    Message = ex.InnerException?.Message ?? ex.Message
                });
            }
        }

        /// <summary>
        /// Get List Product For Gift By Promotion Code
        /// </summary>
        /// <param name="promotionCode"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetListProductForGiftByPromotionCode/{promotionCode}")]
        [MapToApiVersion("1.0")]
        public IActionResult GetListProductForGiftByPromotionCode(string promotionCode)
        {
            try
            {
                var data = _promotionService.GetListProductForGiftByPromotionCode(promotionCode);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return Ok(new BaseResultModel
                {
                    IsSuccess = false,
                    Code = Convert.ToInt32(PromotionError.GetListProductForGiftByPromotionCodeFailed),
                    Message = ex.InnerException?.Message ?? ex.Message
                });
            }
        }

        /// <summary>
        /// Get List Budget By Promotion Code
        /// </summary>
        /// <param name="promotionCode"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetListBudgetByPromotionCode/{promotionCode}")]
        [MapToApiVersion("1.0")]
        public IActionResult GetListBudgetByPromotionCode(string promotionCode)
        {
            try
            {
                var data = _promotionService.GetListBudgetByPromotionCode(promotionCode);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return Ok(new BaseResultModel
                {
                    IsSuccess = false,
                    Code = Convert.ToInt32(PromotionError.GetListBudgetByPromotionCodeFailed),
                    Message = ex.InnerException?.Message ?? ex.Message
                });
            }
        }

        /// <summary>
        /// Get Promotion Definition For Settlement By Promotion Code
        /// </summary>
        /// <param name="promotionCode"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetPromotionDefinitionForSettlement/{promotionCode}")]
        [MapToApiVersion("1.0")]
        public IActionResult GetPromotionDefinitionForSettlement(string promotionCode)
        {
            try
            {
                return Ok(_promotionService.GetPromotionDefinitionForSettlement(promotionCode));
            }
            catch (Exception ex)
            {
                return Ok(new BaseResultModel
                {
                    IsSuccess = false,
                    Code = Convert.ToInt32(PromotionError.GetDataDefinitionStructureByPromotionCodeFailed),
                    Message = ex.InnerException?.Message ?? ex.Message
                });
            }
        }

        /// <summary>
        /// Get Data Initial Promotion Main
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetDataInitialPromotionMain")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetDataInitialPromotionMain()
        {
            try
            {
                return Ok(await _promotionService.GetDataInitialPromotionMain());
            }
            catch (Exception ex)
            {
                return Ok(new BaseResultModel
                {
                    IsSuccess = false,
                    Code = Convert.ToInt32(PromotionError.GetDataInitialPromotionMainFailed),
                    Message = ex.InnerException?.Message ?? ex.Message
                });
            }
        }

        /// <summary>
        /// Get Data Initial Promotion Main By Dapper
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetDataInitialPromotionMainByDapper")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetDataInitialPromotionMainByDapper()
        {
            try
            {
                return Ok(await _promotionService.GetDataInitialPromotionMainByDapper());
            }
            catch (Exception ex)
            {
                return Ok(new BaseResultModel
                {
                    IsSuccess = false,
                    Code = Convert.ToInt32(PromotionError.GetDataInitialPromotionMainFailed),
                    Message = ex.InnerException?.Message ?? ex.Message
                });
            }
        }

        #region External
        /// <summary>
        /// External API Get List Promotion By Customer
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [MapToApiVersion("1.0")]
        [Route("GetListPromotionByCustomer")]
        public async Task<IActionResult> GetListPromotionByCustomer(ListPromotionAndDiscountRequestModel request)
        {
            try
            {
                var result = await _promotionService.GetListPromotionByCustomer(request);
                return Ok(new BaseResultModel
                {
                    IsSuccess = true,
                    Data = result
                });
            }
            catch (Exception ex)
            {
                return Ok(new BaseResultModel
                {
                    IsSuccess = false,
                    Code = Convert.ToInt32(PromotionError.GetListPromotionByCustomerFailed),
                    Message = ex.InnerException?.Message ?? ex.Message
                });
            }
        }

        /// <summary>
        /// External API Get Promotion Details by Promotion Code
        /// </summary>
        /// <param name="Code"></param>
        /// <returns></returns>
        [HttpGet]
        [MapToApiVersion("1.0")]
        [Route("GetDetailPromotionExternalByCode/{Code}")]
        public async Task<IActionResult> GetDetailPromotionExternalByCode([FromRoute] string Code)
        {
            try
            {
                var result = await _promotionService.GetDetailPromotionExternalByCode(Code);
                return Ok(new BaseResultModel
                {
                    IsSuccess = true,
                    Data = result
                });
            }
            catch (Exception ex)
            {
                return Ok(new BaseResultModel
                {
                    IsSuccess = false,
                    Code = Convert.ToInt32(PromotionError.GetDetailPromotionExternalByCodeFailed),
                    Message = ex.InnerException?.Message ?? ex.Message
                });
            }
        }

        /// <summary>
        /// External API Get List Item Group By ItemHierarchy
        /// </summary>
        /// <param name="ItemHierarchyLevel"></param>
        /// <param name="ItemHierarchyCode"></param>
        /// <returns></returns>
        [HttpGet]
        [MapToApiVersion("1.0")]
        [Route("ExternalGetListItemGroupByItemHierarchy/{ItemHierarchyLevel}/{ItemHierarchyCode}")]
        public IActionResult ExternalGetListItemGroupByItemHierarchy(string ItemHierarchyLevel, string ItemHierarchyCode)
        {
            try
            {
                var result = _promotionService.ExternalGetListItemGroupByItemHierarchy(ItemHierarchyLevel, ItemHierarchyCode);
                return Ok(new BaseResultModel
                {
                    IsSuccess = true,
                    Data = result
                });
            }
            catch (Exception ex)
            {
                return Ok(new BaseResultModel
                {
                    IsSuccess = false,
                    Code = Convert.ToInt32(PromotionError.ExternalGetListItemGroupByItemHierarchyFailed),
                    Message = ex.InnerException?.Message ?? ex.Message
                });
            }
        }

        /// <summary>
        /// External API Get Promotion Result 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [MapToApiVersion("1.0")]
        [Route("GetPromotionResult")]
        public async Task<IActionResult> GetPromotionResult(PromotionResultRequestModel request)
        {
            try
            {
                var result = await _promotionService.GetPromotionResult(request);
                return Ok(new BaseResultModel
                {
                    IsSuccess = true,
                    Data = result
                });
            }
            catch (Exception ex)
            {
                return Ok(new BaseResultModel
                {
                    IsSuccess = false,
                    Code = Convert.ToInt32(PromotionError.GetPromotionResultFailed),
                    Message = ex.InnerException?.Message ?? ex.Message
                });
            }
        }

        /// <summary>
        /// External API Check Budget Info Promotion
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [MapToApiVersion("1.0")]
        [Route("ExtenalApiCheckBudgetInfoPromotion")]
        public async Task<IActionResult> ExtenalApiCheckBudgetInfoPromotion(PromotionBudgetRequest request)
        {
            try
            {
                var result = await _promotionService.ExtenalApiCheckBudgetInfoPromotion(request);
                return Ok(new BaseResultModel
                {
                    IsSuccess = true,
                    Data = result
                });
            }
            catch (Exception ex)
            {
                return Ok(new BaseResultModel
                {
                    IsSuccess = false,
                    Code = Convert.ToInt32(PromotionError.ExtenalApiCheckBudgetInfoPromotionFailed),
                    Message = ex.InnerException?.Message ?? ex.Message
                });
            }
        }


        /// <summary>
        /// External API Get List Promotion And Discount
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [MapToApiVersion("1.0")]
        [Route("ExternalGetListPromotionAndDiscountByCustomer")]
        public async Task<IActionResult> ExternalGetListPromotionAndDiscountByCustomer(ListPromotionAndDiscountRequestModel request)
        {
            try
            {
                var result = await _promotionService.ExternalApiGetListPromotionAndDiscount(request);
                return Ok(new BaseResultModel
                {
                    IsSuccess = true,
                    Data = result
                });
            }
            catch (Exception ex)
            {
                return Ok(new BaseResultModel
                {
                    IsSuccess = false,
                    Code = Convert.ToInt32(PromotionError.ExtenalApiCheckBudgetInfoPromotionFailed),
                    Message = ex.InnerException?.Message ?? ex.Message
                });
            }
        }

        #endregion
    }
}
