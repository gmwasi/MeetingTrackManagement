using System;
using System.Text.RegularExpressions;

namespace MeetingTrackManagement.Core.Entities
{
    public class Talk
    {
        public string Topic { get; private set; }

        public TalkDuration Duration { get; private set; }

        public Talk(string topic, TalkDuration duration)
        {
            Duration = duration;
            if (IsValidTitle(topic))
                throw new Exception("Title should not have numeric values");
            Topic = topic;
        }

        private bool IsValidTitle(string title)
        {
            return Regex.IsMatch(title, @"[0-9]+$");
        }
    }

    public class TalkDuration
    {
        public int Value { get; }

        public TimeUnit Unit { get; }

        public TalkDuration(TimeUnit unit, int duration)
        {
            if (!IsValidDuration(duration))
                throw new Exception("Invalid time value");
            Value = duration;
            Unit = unit;
        }

        private bool IsValidDuration(int duration)
        {
            return duration > 0;
        }
    }

    public enum TimeUnit
    {
        Lightning = 5,
        Min = 1
    }
}