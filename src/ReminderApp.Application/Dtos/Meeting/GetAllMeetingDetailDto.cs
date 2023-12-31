﻿namespace ReminderApp.Application.Dtos.Meeting
{
    public class GetAllMeetingDetailDto
    {
        public string MeetingsDay { get; set; }
        public string MeetingStart { get; set; }
        public Guid MeetingId { get; set; }
        public DateTime CreatedDate { get; set; }
        public string MeetingFinish { get; set; } = default;


        public Guid MeetingDetailId { get; set; }
        public int VoteCount { get; set; } = 0;
    }
}
