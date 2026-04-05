using AutoMapper;
using KMCEventPlatform.Models;
using KMCEventPlatform.Services.DTOs;

namespace KMCEventPlatform.Services.Mappings
{
    /// <summary>
    /// AutoMapper profile for entity to DTO mappings
    /// </summary>
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Event mappings
            CreateMap<Event, EventDto>().ReverseMap();

            // Participant mappings
            CreateMap<Participant, ParticipantDto>().ReverseMap();

            // Registration mappings
            CreateMap<Registration, RegistrationDto>().ReverseMap();
        }
    }
}
