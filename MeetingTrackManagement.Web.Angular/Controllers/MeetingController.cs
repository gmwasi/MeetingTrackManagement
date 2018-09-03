using System;
using MeetingTrackManagement.Core;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace MeetingTrackManagement.Web.Angular.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MeetingController : ControllerBase
    {
        private readonly IMeeting _meeting;

        public MeetingController(IMeeting meeting)
        {
            _meeting = meeting;
        }

        // POST: api/Meeting
        [HttpPost]
        public string Post([FromBody] JObject data)
        {
            var input = data.GetValue("input").ToString();
            var tracks = Convert.ToInt32(data.GetValue("tracks").ToString());
            _meeting.InitializeTracks(tracks);
            var inputList = _meeting.Read(input);
            _meeting.RegisterTalks(inputList);
            _meeting.Schedule();
            string result = _meeting.GetSchedule();
            return result;
        }

    }
}