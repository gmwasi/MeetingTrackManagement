using MeetingTrackManagement.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MeetingTrackManagement.Core
{
    public interface IScheduler
    {
        void Schedule(IEnumerable<Day> days, IEnumerable<Talk> talks);
    }

    public class MeetingScheduler : IScheduler
    {
        private List<Day> _days;
        private List<Talk> _talks;

        public MeetingScheduler()
        {
            _days = new List<Day>();
            _talks = new List<Talk>();
        }

        public void Schedule(IEnumerable<Day> days, IEnumerable<Talk> talks)
        {
            _days = days.ToList();
            _talks = talks.ToList();

            SortTalks();
            InitializeTracks();

            foreach (var talk in _talks)
            {
                foreach (var day in _days)
                {
                    var isScheduledInMorning = ScheduleInMorning(talk, day);

                    if (!isScheduledInMorning)
                    {
                        ScheduleInEvening(talk, day);
                    }
                    ScheduleNetworkingEvent(day);
                }
            }
        }

        private void ScheduleNetworkingEvent(Day day)
        {
            foreach (var track in day.Tracks)
                track.Networking.StartTime = track.EveningSession.EndTime.Subtract(track.EveningSession.TimeRemaining);
        }

        private void InitializeTracks()
        {
            foreach (var day in _days)
            {
                foreach (var track in day.Tracks)
                {
                    track.MorningSession.Talks = new List<Talk>();
                    track.MorningSession.TimeRemaining =
                        track.MorningSession.EndTime.Subtract(track.MorningSession.StartTime);

                    track.EveningSession.Talks = new List<Talk>();
                    track.EveningSession.TimeRemaining =
                        track.EveningSession.EndTime.Subtract(track.EveningSession.StartTime);
                }
            }
        }

        private bool ScheduleInMorning(Talk talk, Day day)
        {
            foreach (var track in day.Tracks)
            {
                var duration = talk.Duration.Value * (int)(talk.Duration.Unit);
                if (TalkCanBeScheduledInMorning(duration, track))
                {
                    track.MorningSession.Talks.Add(talk);
                    track.MorningSession.TimeRemaining = track.MorningSession
                                                            .TimeRemaining.Subtract(new TimeSpan(0, duration, 0));
                    return true;
                }
            }
            return false;
        }

        private bool TalkCanBeScheduledInMorning(int duration, Track track)
        {
            return (duration <= track.MorningSession.TimeRemaining.TotalMinutes);
        }

        private bool ScheduleInEvening(Talk talk, Day day)
        {
            foreach (var track in day.Tracks)
            {
                var duration = talk.Duration.Value * (int)(talk.Duration.Unit);
                if (TalkCanBeScheduledInEvening(duration, track))
                {
                    track.EveningSession.Talks.Add(talk);
                    track.EveningSession.TimeRemaining = track.EveningSession
                                                              .TimeRemaining.Subtract(new TimeSpan(0, duration, 0));
                    return true;
                }
            }
            return false;
        }

        private bool TalkCanBeScheduledInEvening(int duration, Track track)
        {
            return (duration <= track.EveningSession.TimeRemaining.TotalMinutes);
        }

        private void SortTalks()
        {
            _talks = _talks.OrderByDescending(t => (t.Duration.Value * (int)(t.Duration.Unit))).ToList();
        }
    }
}