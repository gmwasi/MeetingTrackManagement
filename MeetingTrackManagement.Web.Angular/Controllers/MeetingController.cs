using System;
using MeetingTrackManagement.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.TagHelpers;
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
        public Result Post([FromBody] JObject data)
        {
            var input = data.GetValue("input").ToString();
            var tracks = Convert.ToInt32(data.GetValue("tracks").ToString());
            if (string.IsNullOrEmpty(input) || tracks < 1)
            {
                var r = new Result()
                {
                    Output = "No data was passed"
                };
                return r;
            }
            
            _meeting.InitializeTracks(tracks);
            var inputList = _meeting.Read(input);
            _meeting.RegisterTalks(inputList);
            _meeting.Schedule();
            string output = _meeting.GetSchedule();
            var result = new Result()
            {
                Output = output
            };
            return result;
        }

        public class Result
        {
           public string Output { get; set; }
        }

    }
}