using ODTradePromotion.API.Infrastructure;
using ODTradePromotion.API.Infrastructure.Tp;
using ODTradePromotion.API.Models.Budget;
using ODTradePromotion.API.Services.Base;
using Sys.Common.Models;
using System;
using System.Linq;

namespace ODTradePromotion.API.Services.RegisterPromotion
{
    public class ResgiterPromotion : IResgiterPromotion
    {
        private readonly IBaseRepository<TpBudgetUsed> _baseTpBudgetUsedRepository;
        private readonly IBaseRepository<RegistrationQueue> _baseRegistrationQueueRepository;
        public ResgiterPromotion(IBaseRepository<TpBudgetUsed> baseTpBudgetUsedRepository, IBaseRepository<RegistrationQueue> baseRegistrationQueueRepository)
        {
            _baseTpBudgetUsedRepository = baseTpBudgetUsedRepository;
            _baseRegistrationQueueRepository = baseRegistrationQueueRepository;
        }

        public Result<bool> CancelPromotion(string key)
        {
            var result = new Result<bool>
            {
                Success = true,
                Data = true,
            };
            try
            {
                var checkExistQueue = _baseRegistrationQueueRepository.Find(x => x.Key == key).FirstOrDefault();
                if(checkExistQueue != null)
                {
                    _baseRegistrationQueueRepository.Delete(checkExistQueue.QueueNumber);
                    return result;
                }
                var checkExsitPromotion = _baseTpBudgetUsedRepository.Find(x=>x.Key == key).FirstOrDefault();
                if(checkExsitPromotion != null)
                {
                    _baseTpBudgetUsedRepository.Delete(checkExsitPromotion.Id);
                    return result;
                }
                return result;
            }
            catch (Exception ex)
            {
                result.Data = false;
                result.Success = false;
                result.Messages.Add(ex.Message);
                return result;
            }
        }

        public Result<string> CheckRegistrationForSuccessfulPromotion(string key)
        {
            var result = new Result<string>
            {
                Success = true,
            };
            try
            {
                var checkExistQueue = _baseRegistrationQueueRepository.Find(x => x.Key == key).FirstOrDefault();
                var checkExsitPromotion = _baseTpBudgetUsedRepository.Find(x => x.Key == key).FirstOrDefault();
                if(checkExistQueue != null)
                {
                    result.Data = "Waiting";
                    return result;
                }
                if(checkExistQueue == null && checkExsitPromotion != null)
                {
                    result.Data = "Success";
                    return result;
                }
                result.Data = "Failed";
                return result;
            }
            catch (Exception ex)
            {
                result.Data = "Error";
                result.Success = false;
                result.Messages.Add(ex.Message);
                return result;
            }
        }

        Result<bool> IResgiterPromotion.ResgiterPromotion(TpBudgetUsed request)
        {
            var result = new Result<bool>
            {
                Success = true,
                Data = true
            };
            try
            {
                var data = _baseTpBudgetUsedRepository.Insert(request);
                return result;
            }
            catch (Exception ex)
            {
                result.Data = false;
                result.Success = false;
                result.Messages.Add(ex.Message);
                return result;
            }
        }
    }
}
