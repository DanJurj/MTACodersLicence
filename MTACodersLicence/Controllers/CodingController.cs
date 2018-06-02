using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MTACodersLicence.Data;
using MTACodersLicence.Models;
using MTACodersLicence.Models.ChallengeModels;
using MTACodersLicence.Models.SolutionModels;
using Newtonsoft.Json.Linq;

namespace MTACodersLicence.Controllers
{
    [Authorize]
    public class CodingController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public CodingController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IActionResult Index(int? id, string stdout, string stderr, string error)
        {
            if (id == null)
            {
                return NotFound();
            }
            var challenge = _context.Challenges.FirstOrDefault(m => m.Id == id);
            if (challenge == null)
            {
                return NotFound();
            }
            var userId = _userManager.GetUserId(User);
            var codingSession = _context.CodingSessions
                                        .FirstOrDefault(s => s.ChallengeId == id && s.ApplicationUserId == userId);

            var challengeViewModel = new ChallengeViewModel
            {
                Challenge = challenge,
                CodingSession = codingSession,
                HasPreviousSave = codingSession != null
            };

            if (codingSession != null)
            {
                var passedTime = DateTime.Now - codingSession.StartTime;
                var passedTimeMinutes = passedTime.TotalMinutes;
                if (passedTimeMinutes > challenge.Time)
                {
                    _context.CodingSessions.Remove(codingSession);
                    _context.SaveChanges();
                    var newCodingSession = new CodingSessionModel
                    {
                        ChallengeId = challenge.Id,
                        StartTime = DateTime.Now,
                        ApplicationUserId = userId,
                    };
                    _context.CodingSessions.Add(newCodingSession);
                    _context.SaveChanges();
                    challengeViewModel.RemainingTime = challenge.Time;
                }
                else
                {
                    challengeViewModel.RemainingTime = challenge.Time - (int)passedTimeMinutes;
                }
            }
            else
            {
                var newCodingSession = new CodingSessionModel
                {
                    ChallengeId = challenge.Id,
                    StartTime = DateTime.Now,
                    ApplicationUserId = userId,
                };
                _context.CodingSessions.Add(newCodingSession);
                _context.SaveChanges();
                challengeViewModel.RemainingTime = challenge.Time;
            }
            challengeViewModel.Stdout = stdout;
            challengeViewModel.Stderr = stderr;
            challengeViewModel.Error = error;
            if (error != null)
            {
                challengeViewModel.HasError = error.Length > 3;
            }
            return View(challengeViewModel);
        }


        public async Task<IActionResult> SaveCode(string savedCode, int? challengeId)
        {
            var userId = _userManager.GetUserId(User);
            var codingSession = _context.CodingSessions
                                        .FirstOrDefault(s => s.ChallengeId == challengeId && s.ApplicationUserId == userId);
            if (codingSession == null)
            {
                return NotFound();
            }
            codingSession.Code = savedCode;
            _context.CodingSessions.Update(codingSession);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", new { id = challengeId });
        }

        [HttpPost]
        public async Task<IActionResult> Code([Bind("Code,ChallengeId")] SolutionModel solution, string code, string input, int? language, string codeButton, int? challengeId)
        {
            if (codeButton.Equals("Submit Code"))
            {
                if (ModelState.IsValid)
                {
                    solution.ApplicationUserId = _userManager.GetUserId(User);
                    solution.ReceiveDateTime = DateTime.Now;
                    switch (language)
                    {
                        case 0:
                            solution.Language = "C++";
                            break;
                        case 1:
                            solution.Language = "Java";
                            break;
                        case 2:
                            solution.Language = "Python";
                            break;
                    }
                    _context.Add(solution);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index), new { id = solution.ChallengeId });
                }
            }
            else if (codeButton.Equals("Run Input"))
            {
                // salvare cod 
                var userId = _userManager.GetUserId(User);
                var codingSession = _context.CodingSessions
                    .FirstOrDefault(s => s.ChallengeId == challengeId && s.ApplicationUserId == userId);
                if (codingSession == null)
                {
                    return NotFound();
                }
                codingSession.Code = code.Replace("\\\"","\"");
                _context.CodingSessions.Update(codingSession);
                await _context.SaveChangesAsync();

                string filename = null, type = null;
                switch (language)
                {
                    case 0:
                        filename = "source.cpp";
                        type = "cpp";
                        break;
                    case 1:
                        filename = "Solution.java";
                        type = "java";
                        break;
                    case 2:
                        filename = "source.py";
                        type = "python";
                        break;
                }
                string url = "https://run.glot.io/languages/" + type + "/latest";
                string data;
                if (input != null)
                {
                    data = "{\"stdin\": \"" + input + "\" , \"files\": [{\"name\": \"" + filename + "\", \"content\": \"" + code + "\"}]}";
                }
                else
                {
                    data = "{\"files\": [{\"name\": \"" + filename + "\", \"content\": \"" + code + "\"}]}";
                }
                WebRequest myReq = WebRequest.Create(url);
                myReq.Method = "POST";
                myReq.ContentLength = data.Length;
                myReq.ContentType = "application/json; charset=UTF-8";

                UTF8Encoding enc = new UTF8Encoding();

                myReq.Headers.Add("Authorization: Token 17a385d3-6c5e-4b76-8821-8e83b83352ff");


                using (Stream ds = myReq.GetRequestStream())
                {
                    ds.Write(enc.GetBytes(data), 0, data.Length);
                }

                WebResponse wr = myReq.GetResponse();
                Stream receiveStream = wr.GetResponseStream();
                StreamReader reader = new StreamReader(receiveStream, Encoding.UTF8);
                string content = reader.ReadToEnd();
                string stdout = content.Split("stdout\":\"")[1].Split("\"")[0];
                string stderr = content.Split("stderr\":\"")[1].Split(",\"error")[0];
                string error = content.Split("error\":\"")[1].Split("\"}")[0];
                ViewData["response"] = content;
                ViewData["stdout"] = stdout;
                ViewData["stderr"] = stderr;
                ViewData["error"] = error;
                return RedirectToAction(nameof(Index), new { id = solution.ChallengeId, stdout, stderr, error });
            }
            else if (codeButton.Equals("Run Tests"))
            {
                return NotFound();
            }
            return NotFound();
        }
    }
}