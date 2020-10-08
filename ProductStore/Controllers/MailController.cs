using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ProductStore.Models;
using ProductStore.Services;

namespace ProductStore.Controllers
{
    [Authorize(Roles = "Admin, SuperAdmin")]
    public class MailController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IMailService _mailService;
        public MailController(IMailService mailService, ILogger<HomeController> logger)
        {
            _mailService = mailService;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult SendMail()
        {
            return View();
        }

        public bool IsValidEmail(string emailaddress)
        {
            try
            {
                MailAddress m = new MailAddress(emailaddress);
                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }

        [HttpPost]
        public async Task<IActionResult> SendMail([FromForm] MailRequest request)
        {
            try
            {
                if (IsValidEmail(request.ToEmail))
                {
                    await _mailService.SendEmailAsync(request);
                    _logger.LogInformation("Sending email... Correct.");
                    return RedirectToAction("SendMail");
                }
                _logger.LogWarning("Incorrect email address in variable ToEmail");
                return RedirectToAction("SendMail");


            }
            catch (Exception ex)
            {
                _logger.LogWarning("Email doesn't send. Error: " + ex.Message);
                return RedirectToAction("SendMail");
            }

        }

        [HttpGet]
        public IActionResult SendWelcomeMail()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SendWelcomeMail([FromForm] WelcomeRequest request)
        {
            request.ToEmail = "shevchenya.i@gmail.com";
            request.UserName = "Sheva";
            try
            {
                await _mailService.SendWelcomeEmailAsync(request);
                _logger.LogInformation("Sending email... Correct.");
                return RedirectToAction("SendWelcomeMail");
            }
            catch (Exception ex)
            {
                _logger.LogWarning("Email doesn't send. Error: " + ex.Message);
                return RedirectToAction("SendWelcomeMail");
            }
        }
    }
}
