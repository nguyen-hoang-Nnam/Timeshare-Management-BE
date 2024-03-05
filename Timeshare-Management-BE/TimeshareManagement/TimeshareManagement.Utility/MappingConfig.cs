using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeshareManagement.Models.Models;
using TimeshareManagement.Models.Models.DTO;

namespace TimeshareManagement.Utility
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMaps()
        {
            var mappingConfig = new MapperConfiguration(config =>
            {
                config.CreateMap<Timeshare, TimeshareDTO>().ReverseMap();
                config.CreateMap<Place, PlaceDTO>().ReverseMap();
                config.CreateMap<TimeshareStatus, TimeshareStatusDTO>().ReverseMap();
                /*config.CreateMap<Room, RoomDTO>().ReverseMap();*/
                config.CreateMap<TimeshareDetail, TimeshareDetailDTO>().ReverseMap();
                config.CreateMap<RoomAmenities, RoomAmenitiesDTO>().ReverseMap();
                /*config.CreateMap<RoomDetail, RoomDetailDTO>().ReverseMap();*/
                config.CreateMap<BookingRequest, BookingRequestDTO>().ReverseMap();
            });
            return mappingConfig;
        }
    }
}
