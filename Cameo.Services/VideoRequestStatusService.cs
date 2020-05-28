using Cameo.Data.Infrastructure;
using Cameo.Data.Repository.Interfaces;
using Cameo.Models;
using Cameo.Models.Enums;
using Cameo.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace Cameo.Services
{
    public class VideoRequestStatusService : BaseDropdownableService<VideoRequestStatus>, IVideoRequestStatusService
    {
        public VideoRequestStatusService(IVideoRequestStatusRepository repository,
                           IUnitOfWork unitOfWork)
            : base(repository, unitOfWork)
        {
        }

        public List<BaseModelDropdownable> GetAsSelectListForFilter()
        {
            var statuses = new List<BaseModelDropdownable>()
            {
                new BaseModelDropdownable()
                {
                    ID = (int)VideoRequestStatusEnum.all,
                    Name = "Все"
                },
                new BaseModelDropdownable()
                {
                    ID = (int)VideoRequestStatusEnum.waitingForResponse,
                    Name = "Ожидающие ответа таланта"
                },
                new BaseModelDropdownable()
                {
                    ID = (int)VideoRequestStatusEnum.requestAcceptedAndWaitingForVideo,
                    Name = "Ожидающие видео"
                },
                new BaseModelDropdownable()
                {
                    ID = (int)VideoRequestStatusEnum.videoCompleted,
                    Name = "Видео загружено, следует оплатить"
                },
                new BaseModelDropdownable()
                {
                    ID = (int)VideoRequestStatusEnum.paymentScreenshotUploaded,
                    Name = "Ожидающие подтверждения оплаты"
                },
                new BaseModelDropdownable()
                {
                    ID = (int)VideoRequestStatusEnum.paymentConfirmed,
                    Name = "Видео готово!"
                },
                new BaseModelDropdownable()
                {
                    ID = (int)VideoRequestStatusEnum.notCompleted,
                    Name = "Не выполненные"
                },
            };

            return statuses;
        }
    }
}