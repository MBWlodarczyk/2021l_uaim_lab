﻿using System;

namespace ExaminationRoomsSelector.Web.Application.Queries
{
    using System.Collections.Generic;
    using ExaminationRoomsSelector.Web.Application.Dtos;
    using ExaminationRoomsSelector.Web.Application.DataServiceClients;
    using System.Linq;
    using System.Threading.Tasks;

    public class ExaminationRoomsSelectorQueryHandler : IExaminationRoomsSelectorHandler
    {
        private readonly IExaminationRoomsServiceClient examinationRoomsServiceClient;
        private readonly IDoctorsServiceClient doctorsServiceClient;
        private List<DoctorDto> doctorList;
        private List<ExaminationRoomDto> roomList;
        private readonly List<int> commonSetOfDisease;

        public ExaminationRoomsSelectorQueryHandler(IExaminationRoomsServiceClient examinationRoomsServiceClient, IDoctorsServiceClient doctorsServiceClient)
        {
            this.examinationRoomsServiceClient = examinationRoomsServiceClient;
            this.doctorsServiceClient = doctorsServiceClient;
            doctorList = new List<DoctorDto>();
            roomList = new List<ExaminationRoomDto>();
            commonSetOfDisease = new List<int>();
        }
        
        public async Task<List<DoctorRoomDto>> GetExaminationRoomsSelectionAsync()
        {
            doctorList = (await doctorsServiceClient.GetAllDoctorsAsync()).ToList();
            roomList = (await examinationRoomsServiceClient.GetAllExaminationRoomsAsync()).ToList();

            await GetCommonSet();
            await MakeList();

            return await MatchDoctorsWithRooms();
        }

        private async Task<List<DoctorRoomDto>> MatchDoctorsWithRooms()
        {
            var l = new List<DoctorRoomDto>();

            while (doctorList.Any() && roomList.Any())
            {
                var bestDoctor = doctorList[0];
                var bestRoom = roomList[0];
                var rank = GetRank(bestDoctor,bestRoom);
                
                foreach (var doctor in doctorList)
                {
                    foreach (var room in roomList)
                    {
                        var x = GetRank(doctor, room);
                        if (x > rank)
                        {
                            rank = x;
                            bestDoctor = doctor;
                            bestRoom = room;
                        }
                    }
                }

                if (GetRank(bestDoctor, bestRoom) == 0)
                    break;
                
                l.Add(new DoctorRoomDto(bestDoctor,bestRoom));
                doctorList.Remove(bestDoctor);
                roomList.Remove(bestRoom);
            }
        
            return l;
        }

        //return number of specialization that are same
        private int GetRank(DoctorDto d, ExaminationRoomDto e)
        {
            return d.Specializations.ToList().Intersect(e.Certifications.ToList()).Count();
        }
        
        
        //remake list od doctors and rooms that they contain only specializations witch are common part of all rooms and all doctors
        private async Task MakeList()
        {
            foreach (var doctor in doctorList)
            {
                if (doctor.Specializations.Any(n => commonSetOfDisease.Contains(n)))
                {
                    var list = (doctor.Specializations.ToList()).Intersect(commonSetOfDisease).ToList();
                    doctor.Specializations = list;
                }
            }

            for (var i = 0; i < roomList.Count; i++)
            {
                var room = roomList[i];
                if (room.Certifications.Any(n => commonSetOfDisease.Contains(n)))
                {
                    var list = (room.Certifications.ToList()).Intersect(commonSetOfDisease).ToList();
                    room.Certifications = list;
                }

                //TODO remove rooms with no certifications
                if (!room.Certifications.Any())
                {
                    roomList.Remove(room);
                    i++;
                }
            }
        }
        
        //Get all specializations that are common part of all rooms and all doctors
        private async Task GetCommonSet()
        {
            commonSetOfDisease.Clear();
            
            var commonDoctors = new List<int>();
            var commonRooms = new List<int>();
            var hashSet = new HashSet<int>();
            
            foreach (var n in doctorList)
                commonDoctors.AddRange(n.Specializations.Where(x => hashSet.Add(x)));

            hashSet.Clear();
            
            foreach (var n in roomList)
                commonRooms.AddRange(n.Certifications.Where(x => hashSet.Add(x)));
            
            foreach (var x in commonDoctors.Intersect(commonRooms))
                    commonSetOfDisease.Add(x);
        }
    }
}
