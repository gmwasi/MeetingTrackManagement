using MeetingTrackManagement.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MeetingTrackManagement.Core
{
    public interface IMeeting
    {
        List<string> Read(string input);
        void Schedule();
        string GetSchedule();
        void RegisterTalks(List<string> talkList);
        Talk GetTalkByName(string topic);
        void InitializeTracks(int number);
    }
    public class Meeting : IMeeting
    {
        public Meeting(ITalksProcessor talksProcessor, IScheduler scheduler, IMeetingResult meetingResult)
        {
            Days = new List<Day>();
            SelectedTalks = new List<Talk>();
            TalksProcessor = talksProcessor;
            Scheduler = scheduler;
            MeetingResult = meetingResult;
        }

        public List<Talk> SelectedTalks { get; }

        public ITalksProcessor TalksProcessor { get; set; }

        public IScheduler Scheduler { get; }

        public IMeetingResult MeetingResult { get; set; }

        public List<Day> Days { get; }

        public int TotalTalks => SelectedTalks.Count;

        private int _remainingTime;

        public void Schedule()
        {
            Scheduler.Schedule(Days, SelectedTalks);
        }

        public string GetSchedule()
        {
            return MeetingResult.Get(Days);
        }

        public List<string> Read(string input)
        {
            string[] lines = input.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
            return lines.ToList();
        }

        public void RegisterTalks(List<string> talkList)
        {
            var newTalks = TalksProcessor.Process(talkList).ToList();

            if (CannotBeRegistered(newTalks))
                throw new ArgumentException("Exceeding Time Limit");

            SelectedTalks.InsertRange(SelectedTalks.Count, newTalks);
        }

        public Talk GetTalkByName(string topic)
        {
            return SelectedTalks.FirstOrDefault(talk => string.Equals(talk.Topic, topic, StringComparison.OrdinalIgnoreCase));
        }

        public void InitializeTracks(int number)
        {
            var tracks = new List<Track>();
            for (var i = 0; i < number; i++)
            {
                tracks.Add(GetNewTrack());
            }
            Days.Add(new Day(tracks));
            CalculateRemainingTime();
        }

        private  Track GetNewTrack()
        {
            return new Track()
            {
                MorningSession = new TalkSession()
                {
                    Title = "Morning Session",
                    StartTime = new TimeSpan(09, 00, 00),
                    EndTime = new TimeSpan(12, 00, 00)
                },
                EveningSession = new TalkSession()
                {
                    Title = "Evening Session",
                    StartTime = new TimeSpan(01, 00, 00),
                    EndTime = new TimeSpan(5, 00, 00)
                },
                Networking = new NetworkingEvent()
                {
                    Title = "Networking Event",
                    StartTimeFrom = new TimeSpan(04, 00, 00),
                    StartTimeTo = new TimeSpan(05, 00, 00)
                },
                LunchBreak = new Break()
                {
                    Title = "Lunch Break",
                    StartTime = new TimeSpan(12, 00, 00),
                    EndTime = new TimeSpan(1, 00, 00)
                }
            };
        }

        private bool CannotBeRegistered(IEnumerable<Talk> newTalks)
        {
            var timeTaken = newTalks.Sum(newTalk => newTalk.Duration.Value * (int)newTalk.Duration.Unit);
            if (timeTaken > _remainingTime)
                return true;
            _remainingTime = _remainingTime - timeTaken;
            return false;
        }

        private void CalculateRemainingTime()
        {
            foreach (var day in Days)
            {
                foreach (var track in day.Tracks)
                {
                    _remainingTime +=
                        (int)track.MorningSession.EndTime.Subtract(track.MorningSession.StartTime).TotalMinutes;
                    _remainingTime +=
                        (int)track.EveningSession.EndTime.Subtract(track.EveningSession.StartTime).TotalMinutes;
                }
            }
        }
    }
}