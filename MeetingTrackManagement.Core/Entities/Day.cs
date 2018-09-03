using System.Collections.Generic;

namespace MeetingTrackManagement.Core.Entities
{
    public class Day
    {
        public IEnumerable<Track> Tracks { get; }

        public Day(IEnumerable<Track> tracks)
        {
            Tracks = tracks;
        }
    }
}