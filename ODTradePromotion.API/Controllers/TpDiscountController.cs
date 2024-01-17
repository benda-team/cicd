using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using ODTradePromotion.API.Models;
using ODTradePromotion.API.Models.Paging;
using ODTradePromotion.API.Services.TpDiscount;
using System;
using System.Linq;
using static Sys.Common.Constants.ErrorCodes;
using System.Linq.Dynamic.Core;
using ODTradePromotion.API.Models.Discount;
using System.Threading.Tasks;
using AutoMapper;
using ODTradePromotion.API.Models.Promotion;
using Sys.Common.JWT;

namespace ODTradePromotion.API.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    //[AllowAnonymous]
    [Authorize]
    [ApiVersion("1.0")]
    public class TpDiscountController : BaseController
    {
        private readonly IDiscountService _discountService;
        private readonly IMapper _mapper;
        public TpDiscountController(
            IDiscountService discountService,
            IMapper mapper
            )
        {
            _mapper = mapper;
            _discountService = discountService;
        }

        /// <summary>
        /// Get List Discount
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetListTpDiscount")]
        [MapToApiVersion("1.0")]
        public IActionResult GetListTpDiscount(EcoParameters parameters)
        {
            try
            {
                var featureListTemp = _discountService.GetListTpDiscount();
                // check searching
                if (parameters.Filter != null && parameters.Filter.Trim() != string.Empty && parameters.Filter.Trim() != "NA_EMPTY")
                {
                    var optionsAssembly = ScriptOptions.Default.AddReferences(typeof(TpDiscountModel).Assembly);
                    var filterExpressionTemp = CSharpScript.EvaluateAsync<Func<TpDiscountModel, bool>>(($"s=> {parameters.Filter}"), optionsAssembly);
                    Func<TpDiscountModel, bool> discountFilterExpression = filterExpressionTemp.Result;

                    var checkCondition = featureListTemp.Where(discountFilterExpression);
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
                    var featureListTempPagged1 = PagedList<TpDiscountModel>.ToPagedList(featureListTemp.ToList(), 0, featureListTemp.Count());

                    return Ok(new TpDiscountListModel { Items = featureListTempPagged1 });
                }

                int totalCount = featureListTemp.Count();
                int skip = parameters.Skip ?? 0;
                int top = parameters.Top ?? parameters.PageSize;
                var items = featureListTemp.Skip(skip).Take(top).ToList();
                var result = new PagedList<TpDiscountModel>(items, totalCount, (skip / top) + 1, top);
                return Ok(new TpDiscountListModel { Items = result, MetaData = result.MetaData });
            }
            catch (Exception ex)
            {
                return Ok(new BaseResultModel
                {
                    IsSuccess = false,
                    Code = Convert.ToInt32(TpDiscountError.ListTpDiscountFailed),
                    Message = ex.InnerException?.Message ?? ex.Message
                });
            }
        }

        /// <summary>
        /// Get List Discount For Settlement
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetListTpDiscountForSettlement")]
        [MapToApiVersion("1.0")]
        public IActionResult GetListTpDiscountForSettlement(EcoParameters parameters)
        {
            try
            {
                var featureListTemp = _discountService.GetListTpDiscountForSettlement();

                // check searching
                if (parameters.Filter != null && parameters.Filter.Trim() != string.Empty && parameters.Filter.Trim() != "NA_EMPTY")
                {
                    var optionsAssembly = ScriptOptions.Default.AddReferences(typeof(TpDiscountModel).Assembly);
                    var filterExpressionTemp = CSharpScript.EvaluateAsync<Func<TpDiscountModel, bool>>(($"s=> {parameters.Filter}"), optionsAssembly);
                    Func<TpDiscountModel, bool> filterExpression = filterExpressionTemp.Result;

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
                    var featureListTempPagged1 = PagedList<TpDiscountModel>.ToPagedList(featureListTemp.ToList(), 0, featureListTemp.Count());

                    return Ok(new TpDiscountListModel { Items = featureListTempPagged1 });
                }

                int totalCount = featureListTemp.Count();
                int skip = parameters.Skip ?? 0;
                int top = parameters.Top ?? parameters.PageSize;
                var items = featureListTemp.Skip(skip).Take(top).ToList();
                var result = new PagedList<TpDiscountModel>(items, totalCount, (skip / top) + 1, top);
                return Ok(new TpDiscountListModel { Items = result, MetaData = result.MetaData });
            }
            catch (Exception ex)
            {
                return Ok(new BaseResultModel
                {
                    IsSuccess = false,
                    Code = Convert.ToInt32(TpDiscountError.ListTpDiscountFailed),
                    Message = ex.InnerException?.Message ?? ex.Message
                });
            }
        }

        /// <summary>
        /// Get Discount For Settlement By Discount Code
        /// </summary>
        /// <param name="discountCode"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetTpDiscountForSettlementByDiscountCode/{discountCode}")]
        [MapToApiVersion("1.0")]
        public IActionResult GetTpDiscountForSettlementByDiscountCode(string discountCode)
        {
            try
            {
                var featureListTemp = _discountService.GetListTpDiscountForSettlement();

                var discount = featureListTemp.FirstOrDefault(x => x.Code.ToLower().Equals(discountCode.ToLower()));
                return Ok(discount);
            }
            catch (Exception ex)
            {
                return Ok(new BaseResultModel
                {
                    IsSuccess = false,
                    Code = Convert.ToInt32(TpDiscountError.GetTpDiscountForSettlementByDiscountCodeFailed),
                    Message = ex.InnerException?.Message ?? ex.Message
                });
            }
        }

        /// <summary>
        /// Get Discount By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetTpDiscountById/{id}")]
        [MapToApiVersion("1.0")]
        public IActionResult GetTpDiscountById(Guid id)
        {
            try
            {
                var data = _discountService.GetTpDiscountDetailById(id);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return Ok(new BaseResultModel
                {
                    IsSuccess = false,
                    Code = Convert.ToInt32(TpDiscountError.GetTpDiscountFailed),
                    Message = ex.InnerException?.Message ?? ex.Message
                });
            }
        }

        /// <summary>
        /// Get Discount By Code
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetTpDiscountDetailByCode/{code}")]
        [MapToApiVersion("1.0")]
        public IActionResult GetTpDiscountDetailByCode(string code)
        {
            try
            {
                return Ok(_discountService.GetTpDiscountDetailByCode(code));
            }
            catch (Exception ex)
            {
                return Ok(new BaseResultModel
                {
                    IsSuccess = false,
                    Code = Convert.ToInt32(TpDiscountError.GetTpDiscountFailed),
                    Message = ex.InnerException?.Message ?? ex.Message
                });
            }
        }

        /// <summary>
        /// Get General Discount By Code
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetGeneralTpDiscountByCode/{code}")]
        [MapToApiVersion("1.0")]
        public IActionResult GetGeneralTpDiscountByCode(string code)
        {
            try
            {
                var data = _discountService.GetGeneralTpDiscountByCode(code);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return Ok(new BaseResultModel
                {
                    IsSuccess = false,
                    Code = Convert.ToInt32(TpDiscountError.GetTpDiscountFailed),
                    Message = ex.InnerException?.Message ?? ex.Message
                });
            }
        }

        /// <summary>
        /// Create Discount
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("CreateTpDiscount")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> CreateTpDiscount(TpDiscountModel input)
        {
            try
            {
                string userLogin = input.UserName;
                var existItem = await _discountService.GetGeneralDiscountByCode(input.Code);
                if (existItem == null)
                {
                    return Ok(await _discountService.CreateTpDiscount(input, userLogin));
                }
                else
                {
                    return Ok(new BaseResultModel
                    {
                        IsSuccess = false,
                        Code = Convert.ToInt32(TpDiscountError.CreateTpDiscountFailedTpDiscountCodeExist),
                        Message = "TpDiscountCodeExist" //"Function code already exist, please use new code."
                    });
                }

            }
            catch (Exception ex)
            {
                return Ok(new BaseResultModel
                {
                    IsSuccess = false,
                    Code = Convert.ToInt32(TpDiscountError.CreateTpDiscountFailed),
                    Message = ex.InnerException?.Message ?? ex.Message
                });
            }
        }

        /// <summary>
        /// Update Discount
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("UpdateTpDiscount")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> UpdateTpDiscount(TpDiscountModel input)
        {
            try
            {
                string userLogin = input.UserName;
                var existItem = await _discountService.GetGeneralDiscountByCode(input.Code);
                if (existItem != null)
                {
                    return Ok(await _discountService.UpdateTpDiscount(input, userLogin));
                }
                else
                {
                    return Ok(new BaseResultModel
                    {
                        IsSuccess = false,
                        Code = Convert.ToInt32(TpDiscountError.UpdateTpDiscountFailedTpDiscountNotExist),
                        Message = "TpDiscountCodeExist" //"Function code already exist, please use new code."
                    });
                }
            }
            catch (Exception ex)
            {
                return Ok(new BaseResultModel
                {
                    IsSuccess = false,
                    Code = Convert.ToInt32(TpDiscountError.UpdateTpDiscountFailed),
                    Message = ex.InnerException?.Message ?? ex.Message
                });
            }
        }

        /// <summary>
        /// Delete Discount By Code
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("DeleteTpDiscountByCode/{code}")]
        [MapToApiVersion("1.0")]
        public IActionResult DeleteTpDiscountByCode(string code)
        {
            try
            {
                var discount = _discountService.GetGeneralTpDiscountByCode(code);
                if (discount != null && !string.IsNullOrEmpty(discount.Code) && _discountService.DeleteTpDiscountByCode(code, discount.UserName))
                {
                    return Ok(new BaseResultModel
                    {
                        IsSuccess = true
                    });
                }

                return Ok(new BaseResultModel
                {
                    IsSuccess = false,
                    Code = Convert.ToInt32(TpDiscountError.DeleteTpDiscountFailed),
                    Message = "DiscountNotExist"
                });
            }
            catch (Exception ex)
            {
                return Ok(new BaseResultModel
                {
                    IsSuccess = false,
                    Code = Convert.ToInt32(TpDiscountError.DeleteTpDiscountFailed),
                    Message = ex.InnerException?.Message ?? ex.Message
                });
            }
        }

        /// <summary>
        /// Check Exist Scope Object Discount
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("CheckExistScopeObjectDiscount")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> CheckExistScopeObjectDiscount(TpDiscountModel input)
        {
            try
            {
                return Ok(await _discountService.CheckExistScopeObjectDiscount(input));
            }
            catch (Exception ex)
            {
                return Ok(new BaseResultModel
                {
                    IsSuccess = false,
                    Code = Convert.ToInt32(TpDiscountError.UpdateTpDiscountFailed),
                    Message = ex.InnerException?.Message ?? ex.Message
                });
            }
        }

        #region External API
        /// <summary>
        /// External API Get List Discount By Customer
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [MapToApiVersion("1.0")]
        [Route("GetDiscountByCustomer")]
        public async Task<IActionResult> GetDiscountByCustomer(ListPromotionAndDiscountRequestModel request)
        {
            try
            {
                var result = await _discountService.GetDiscountByCustomer(request);
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
                    Code = Convert.ToInt32(TpDiscountError.GetListDiscountByCustomerCodeShiptoCodeFailed),
                    Message = ex.InnerException?.Message ?? ex.Message
                });
            }
        }



        /// <summary>
        /// Discount Result
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("DiscountResult")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> DiscountResult(DiscountResultParameters input)
        {
            try
            {
                var existItem = await _discountService.GetGeneralDiscountByCode(input.DiscountCode);
                if (existItem != null)
                {
                    return Ok(await _discountService.DiscountResult(input));
                }
                else
                {
                    return Ok(new BaseResultModel
                    {
                        IsSuccess = false,
                        Code = Convert.ToInt32(TpDiscountError.GetTpDiscountFailed),
                        Message = "DiscountCodeNotExist"
                    });
                }
            }
            catch (Exception ex)
            {
                return Ok(new BaseResultModel
                {
                    IsSuccess = false,
                    Code = Convert.ToInt32(TpDiscountError.CreateTpDiscountFailed),
                    Message = ex.InnerException?.Message ?? ex.Message
                });
            }
        }
        #endregion
    }
}

