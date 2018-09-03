using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using MeetingTrackManagement.Core.Entities;

namespace MeetingTrackManagement.Core
{
    public interface IMeetingResult
    {
        string Get(IEnumerable<Day> days);
    }


    public class MeetingResult : IMeetingResult
    {
        public string Get(IEnumerable<Day> days)
        {
            var stringBuilder = new StringBuilder();
            foreach (var day in days)
            {
                var tracks = day.Tracks.ToList();
                int count = 0;
                foreach (var track in tracks)
                {
                    count++;
                    if (count > 1)
                        stringBuilder.Append($"\n\n");
                    stringBuilder.Append($"TRACK {count}: \n\n");
                    stringBuilder.Append(track.MorningSession.Title);
                    var currentTime = track.MorningSession.StartTime;

                    foreach (var talk in track.MorningSession.Talks)
                    {
                        stringBuilder.Append($"\n{currentTime}\t{talk.Topic}\t{talk.Duration.Value}{talk.Duration.Unit}");
                        currentTime = currentTime.Add(new TimeSpan(0, talk.Duration.Value * (int)talk.Duration.Unit, 0));
                    }

                    stringBuilder.Append($"\n\n{track.LunchBreak.Title}\t{track.LunchBreak.StartTime}\t{track.LunchBreak.EndTime}");

                    stringBuilder.Append("\n\n" + track.EveningSession.Title);
                    currentTime = track.EveningSession.StartTime;

                    foreach (var talk in track.EveningSession.Talks)
                    {
                        stringBuilder.Append($"\n{currentTime}\t{talk.Topic}\t{talk.Duration.Value}{talk.Duration.Unit}");
                        currentTime =
                            currentTime.Add(new TimeSpan(0, talk.Duration.Value * (int)talk.Duration.Unit, 0));
                    }

                    stringBuilder.Append($"\n{track.Networking.Title}\t{track.Networking.StartTime}");
                }
            }

            return stringBuilder.ToString();
        }
    }

}