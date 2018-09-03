using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using MeetingTrackManagement.Core;
using Microsoft.AspNetCore.Mvc;
using MeetingTrackManagement.Web.Mvc.Models;

namespace MeetingTrackManagement.Web.Mvc.Controllers
{
    public class HomeController : Controller
    {
        private readonly IMeeting _meeting;

        public HomeController(IMeeting meeting)
        {
            _meeting = meeting;
        }

        public IActionResult Index()
        {
            var model = new MeetingViewModel()
            {
                Input = $"Writing Fast Tests Against Enterprise Rails: 60min\r\nOverdoing it in Python: 45min\r\nLua for the Masses: 30min\r\nRuby Errors from Mismatched Gem Versions: 45min\r\nCommon Ruby Errors: 45min\r\nRails for Python Developers lightning: 35min\r\nCommunicating Over Distance: 60min\r\nAccounting-Driven Development: 45min\r\nWoah: 30min\r\nSit Down and Write: 30min\r\nPair Programming vs Noise: 45min\r\nRails Magic: 60min\r\nRuby on Rails: Why We Should Move On?: 60min\r\nClojure Ate Scala (on my project): 45min\r\nProgramming in the Boondocks of Seattle: 30min\r\nRuby vs. Clojure for Back-End Development: 30min\r\nRuby on Rails Legacy App Maintenance: 60min\r\nA World Without HackerNews: 30min\r\nUser Interface CSS in Rails Apps: 30min",
                Tracks = 2
            };
            return View(model);
        }

        public IActionResult Meeting(MeetingViewModel data)
        {
            if (string.IsNullOrEmpty(data.Input) || data.Tracks < 1)
            {
                var output = new MeetingViewModel()
                {
                    Output = "No data was passed!"
                };
                return View(output);
            }
            var input = data.Input;
            var tracks = data.Tracks;
            _meeting.InitializeTracks(tracks);
            var inputList = _meeting.Read(input);
            _meeting.RegisterTalks(inputList);
            _meeting.Schedule();
            string result = _meeting.GetSchedule();
            data.Output = result;
            return View(data);
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
