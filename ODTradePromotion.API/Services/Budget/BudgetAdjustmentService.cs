using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ODTradePromotion.API.Infrastructure.Tp;
using ODTradePromotion.API.Models.Budget;
using ODTradePromotion.API.Services.Base;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ODTradePromotion.API.Services.Budget
{
    public class BudgetAdjustmentService : IBudgetAdjustmentService
    {
        #region Property
        private readonly ILogger<BudgetAdjustmentService> _logger;
        private readonly IBaseRepository<TpBudgetAdjustment> _serviceBudgetAdjustment;
        private readonly IBaseRepository<TpBudgetAllotmentAdjustment> _serviceBudgetAllotmentAdjustment;
        private readonly IBudgetService _budgetService;
        private readonly IMapper _mapper;
        #endregion

        #region Constructor
        public BudgetAdjustmentService(ILogger<BudgetAdjustmentService> logger,
            IBaseRepository<TpBudgetAdjustment> serviceBudgetAdjustment,
            IBaseRepository<TpBudgetAllotmentAdjustment> serviceBudgetAllotmentAdjustment,
            IBudgetService budgetService,
            IMapper mapper
            )
        {
            _logger = logger;
            _serviceBudgetAdjustment = serviceBudgetAdjustment;
            _serviceBudgetAllotmentAdjustment = serviceBudgetAllotmentAdjustment;
            _budgetService = budgetService;
            _mapper = mapper;
        }
        #endregion

        #region Method
        public bool CreateBudgetAdjustment(TpBudgetAdjustmentModel input, string userlogin)
        {
            // Create budget adjustment
            var budgetAdjustment = _mapper.Map<TpBudgetAdjustment>(input);
            budgetAdjustment.Id = Guid.NewGuid();
            budgetAdjustment.AdjustmentDate = DateTime.Now;
            budgetAdjustment.Account = userlogin;

            // Check count adjustment 
            var dataCheck = _serviceBudgetAdjustment.GetAllQueryable().AsNoTracking().Where(m => m.BudgetCode == input.BudgetCode);
            if (dataCheck == null || !dataCheck.Any())
                budgetAdjustment.CountAdjustment = 1;
            else
                budgetAdjustment.CountAdjustment = dataCheck.Max(x => x.CountAdjustment) + 1;
            
            _serviceBudgetAdjustment.Insert(budgetAdjustment);

            //Create budget allotment adjustment
            var lstBudgetAllotmentAdjustment = CreateDataBudgetAllotmentAdjustments(input.BudgetAllotmentAdjustments, budgetAdjustment.Id);
            _serviceBudgetAllotmentAdjustment.InsertRange(lstBudgetAllotmentAdjustment);

            // Update Budget
            var result = _budgetService.UpdateDataBudget(input.Budget, userlogin);
            if (result)
            {
                // save into db
                _serviceBudgetAdjustment.Save();
                return true;
            }
            return false;
        }

        public TpBudgetAdjustmentModel GetHistoryBudgetAdjustment(string budgetCode, int type, int countAdjustment)
        {
            var returnValue = _mapper.Map<TpBudgetAdjustmentModel>(_serviceBudgetAdjustment.GetAllQueryable().AsNoTracking()
                .Where(m => m.BudgetCode == budgetCode && m.Type == type && m.CountAdjustment == countAdjustment).FirstOrDefault());
            if (returnValue != null)
            {
                returnValue.BudgetAllotmentAdjustments = _serviceBudgetAllotmentAdjustment.GetAllQueryable(x => x.BudgetAdjustmentId == returnValue.Id)
                    .AsNoTracking().ProjectTo<TpBudgetAllotmentAdjustmentModel>(_mapper.ConfigurationProvider).ToList();
            }
            return returnValue;
        }

        public IQueryable<TpBudgetAdjustmentListModel> GetListBudgetAdjustment(int type, string budgetCode)
        {
            var listResult = _serviceBudgetAdjustment.GetAllQueryable().AsNoTracking()
                .Where(x => x.Type == type && x.BudgetCode == budgetCode).ProjectTo<TpBudgetAdjustmentListModel>(_mapper.ConfigurationProvider);
            return listResult;
        }

        private List<TpBudgetAllotmentAdjustment> CreateDataBudgetAllotmentAdjustments(List<TpBudgetAllotmentAdjustmentModel> lstInput, Guid budgetAdjustmentId)
        {
            var lstDataCreateNew = new List<TpBudgetAllotmentAdjustment>();
            foreach (var item in lstInput)
            {
                var entity = _mapper.Map<TpBudgetAllotmentAdjustment>(item);
                entity.Id = Guid.NewGuid();
                entity.BudgetAdjustmentId = budgetAdjustmentId;
                lstDataCreateNew.Add(entity);
            }
            return lstDataCreateNew;
        }
        #endregion
    }
}
