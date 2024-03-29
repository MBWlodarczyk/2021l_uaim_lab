﻿namespace ExaminationRooms.Controllers
{
    using System.Collections.Generic;
    using Application.Dtos;
    using Application.Queries;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;

    [ApiController]
    public class ExaminationRoomsController : ControllerBase
    {
        private readonly ILogger<ExaminationRoomsController> logger;
        private readonly IExaminationRoomQueriesHandler examinationRoomQueriesHandler;

        public ExaminationRoomsController(ILogger<ExaminationRoomsController> logger,
            IExaminationRoomQueriesHandler examinationRoomQueriesHandler)
        {
            this.logger = logger;
            this.examinationRoomQueriesHandler = examinationRoomQueriesHandler;
        }

        [HttpGet("examination-rooms")]
        public IEnumerable<ExaminationRoomDto> GetAll()
        {
            return examinationRoomQueriesHandler.GetAll();
        }

        [HttpGet("examination-room")]
        public IEnumerable<ExaminationRoomDto> GetBySpecialization([FromQuery] int certificationType)
        {
            return examinationRoomQueriesHandler.GetByCertificationType(certificationType);
        }

        [HttpPost("add-room")]
        public void AddDoctor(ExaminationRoomDto examinationRoomDto)
        {
            examinationRoomQueriesHandler.AddRoom(examinationRoomDto);
        }
    }
}