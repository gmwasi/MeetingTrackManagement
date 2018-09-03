using System;
using System.Collections.Generic;

namespace MeetingTrackManagement.Core.Entities
{
    public class Slot
    {
        public TimeSpan StartTime { get; set; }

        public TimeSpan EndTime { get; set; }

        public string Title { get; set; }
    }

    public class TalkSession : Slot
    {
        public List<Talk> Talks { get; set; }

        public int _duration;
        public int Duration
        {
            get { return _duration; }
            private set { _duration = (int)EndTime.Subtract(StartTime).TotalMinutes; }
        }

        public TimeSpan TimeRemaining { get; set; }
    }

    public class NetworkingEvent : Slot
    {
        public TimeSpan StartTimeFrom { get; set; }

        public TimeSpan StartTimeTo { get; set; }
    }

    public class Break : Slot
    {
    }
}