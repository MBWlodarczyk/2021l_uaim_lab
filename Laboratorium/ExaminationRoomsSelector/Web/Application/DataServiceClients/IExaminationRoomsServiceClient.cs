﻿namespace ExaminationRoomsSelector.Web.Application.DataServiceClients
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Dtos;

    public interface IExaminationRoomsServiceClient
    {
        Task<IEnumerable<ExaminationRoomDto>> GetAllExaminationRoomsAsync();
        
        void AddRoom(ExaminationRoomDto examinationRoomDto);

    }
}