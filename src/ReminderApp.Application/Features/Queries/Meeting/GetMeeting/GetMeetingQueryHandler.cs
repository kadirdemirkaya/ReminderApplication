﻿using AutoMapper;
using MediatR;
using ReminderApp.Application.Abstractions;
using ReminderApp.Application.Abstractions.Services;
using ReminderApp.Application.Dtos.Meeting;

namespace ReminderApp.Application.Features.Queries.Meeting.GetMeeting
{
    public class GetMeetingQueryHandler : IRequestHandler<GetMeetingQuery, List<GetAllMeetingDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IJwtTokenService _jwtTokenService;
        private List<GetAllMeetingDto> getAllMeetingDtos;
        private readonly IMapper _mapper;
        private readonly IMeetingService _meetingService;
        public GetMeetingQueryHandler(IUnitOfWork unitOfWork, IJwtTokenService jwtTokenService, IMapper mapper, IMeetingService meetingService)
        {
            _unitOfWork = unitOfWork;
            _jwtTokenService = jwtTokenService;
            getAllMeetingDtos = new List<GetAllMeetingDto>();
            _mapper = mapper;
            _meetingService = meetingService;
        }

        public async Task<List<GetAllMeetingDto>> Handle(GetMeetingQuery request, CancellationToken cancellationToken)
        {
            var user = await _jwtTokenService.GetUserWithTokenAsync(request.token);

            var meetings = await _unitOfWork.GetReadRepository<ReminderApp.Domain.Entities.Meeting>().GetAllAsync(m => m.Email == user.Email && m.isActive == true, true, m => m.MeetingItems, m => m.MeetingDetails);

            #region old code
            //foreach (var meeting in meetings)
            //{
            //    GetAllMeetingDto getAllMeetingDto = _mapper.Map<GetAllMeetingDto>(meeting);

            //    foreach (var meetingItem in meeting.MeetingItems)
            //        getAllMeetingDto.GetAllMeetingItemDto.Add(_mapper.Map<GetAllMeetingItemDto>(meetingItem));

            //    foreach (var meetingDetail in meeting.MeetingDetails)
            //    {
            //        GetAllMeetingDetailDto newMeetingDetailDto = _mapper.Map<GetAllMeetingDetailDto>(meetingDetail);

            //        var dicDayAndCount = await _meetingService.GetMeetingVoteCount(meeting.Id);

            //        if (!getAllMeetingDto.GetAllMeetingDetailDtos.Any(x => x.MeetingsDay == newMeetingDetailDto.MeetingsDay))
            //        {
            //            newMeetingDetailDto.VoteCount = meetingDetail.VoteCount;
            //            newMeetingDetailDto.MeetingFinish = meetingDetail.MeetingFinish;
            //            newMeetingDetailDto.MeetingDetailId = meetingDetail.Id;
            //            getAllMeetingDto.GetAllMeetingDetailDtos.Add(newMeetingDetailDto);
            //        }
            //    }
            //    getAllMeetingDtos.Add(getAllMeetingDto);
            //}
            //return getAllMeetingDtos;
            #endregion

            foreach (var meeting in meetings)
            {
                GetAllMeetingDto getAllMeetingDto = _mapper.Map<GetAllMeetingDto>(meeting);
                var userWithToken = await _jwtTokenService.GetUserWithTokenAsync(request.token);
                getAllMeetingDto.UserId = userWithToken.Id;

                foreach (var meetingItem in meeting.MeetingItems)
                {
                    var mapData = _mapper.Map<GetAllMeetingItemDto>(meetingItem);
                    var userInfo = await _unitOfWork.GetReadRepository<ReminderApp.Domain.Entities.User>().GetAsync(u => u.Email == meetingItem.Email);
                    mapData.UserId = userInfo.Id;
                    mapData.IsVoted = meetingItem.Voted;
                    getAllMeetingDto.GetAllMeetingItemDto.Add(mapData);
                }

                foreach (var meetingDetail in meeting.MeetingDetails)
                {
                    GetAllMeetingDetailDto newMeetingDetailDto = _mapper.Map<GetAllMeetingDetailDto>(meetingDetail);

                    var dicDayAndCount = await _meetingService.GetMeetingVoteCount(meeting.Id);

                    if (!getAllMeetingDto.GetAllMeetingDetailDtos.Any(x => x.MeetingDetailId == newMeetingDetailDto.MeetingDetailId))
                    {
                        newMeetingDetailDto.VoteCount = meetingDetail.VoteCount;
                        newMeetingDetailDto.MeetingFinish = meetingDetail.MeetingFinish;
                        newMeetingDetailDto.MeetingDetailId = meetingDetail.Id;
                        getAllMeetingDto.GetAllMeetingDetailDtos.Add(newMeetingDetailDto);
                    }
                }
                getAllMeetingDtos.Add(getAllMeetingDto);
            }
            return getAllMeetingDtos;
        }
    }
}