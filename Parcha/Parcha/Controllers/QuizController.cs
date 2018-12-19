using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Parcha.Data;
using Parcha.Data.Models;
using Parcha.ViewModels;

namespace Parcha.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuizController : ControllerBase
    {


        #region Private Fields
        private ApplicationDbContext DbContext;
        #endregion
        #region Constructor
        public QuizController(ApplicationDbContext context)
        {
            DbContext = context;
        }
        #endregion Constructor

        [HttpGet("{id}")]

        public IActionResult Get(int id)
        {
            var quiz = DbContext.Quizzes.Where(i => i.Id ==
 id).FirstOrDefault();

            // handle requests asking for non-existing quizzes
            if (quiz == null)
            {
                return NotFound(new
                {
                    Error = String.Format("Quiz ID {0} has not been found", id)
                });
            }
            return new JsonResult(
            quiz,
            new JsonSerializerSettings()
            {
                Formatting = Formatting.Indented
            });
        }


        [HttpPut]
        public IActionResult Put([FromBody] QuizViewModel model)
        {
            if (model == null) return new StatusCodeResult(500);
            var quiz = new Quiz();
            quiz.Title = model.Title;
            quiz.Description = model.Description;
            quiz.Text = model.Text;
            quiz.Notes = model.Notes;
            quiz.CreatedDate = DateTime.Now;
            quiz.LastModifiedDate = quiz.CreatedDate;
            quiz.UserId = DbContext.Users.Where(u => u.UserName == "Admin")
            .FirstOrDefault().Id;
            DbContext.Quizzes.Add(quiz);
            DbContext.SaveChanges();


            return new JsonResult(quiz.Adapt<QuizViewModel>(),
            new JsonSerializerSettings()
            {
                Formatting = Formatting.Indented
            });
        }

        [HttpPost]
        public IActionResult Post(QuizViewModel model)
        {
            if (model == null) return new StatusCodeResult(500);
            var quiz = DbContext.Quizzes.Where(q => q.Id ==
 model.Id).FirstOrDefault();
            if (quiz == null)
            {
                return NotFound(new
                {
                    Error = String.Format("Quiz ID {0} has not been found",
                model.Id)
                });
            }
            quiz.Title = model.Title;
            quiz.Description = model.Description;
            quiz.Text = model.Text;
            quiz.Notes = model.Notes;
            quiz.LastModifiedDate = quiz.CreatedDate;
            DbContext.SaveChanges();
            return new JsonResult(quiz.Adapt<QuizViewModel>(),
            new JsonSerializerSettings()
            {
                Formatting = Formatting.Indented
            });
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var quiz = DbContext.Quizzes.Where(i => i.Id == id)
            .FirstOrDefault();
            if (quiz == null)
            {
                return NotFound(new
                {
                    Error = String.Format("Quiz ID {0} has not been found", id)
                });
            }
            DbContext.Quizzes.Remove(quiz);
            DbContext.SaveChanges();
            return new OkResult();
        }


        [HttpGet("Latest/{num:int?}")]
        public IActionResult Latest(int num = 10)
        {
            var latest = DbContext.Quizzes
                 .OrderByDescending(q => q.CreatedDate)
                 .Take(num)
                 .ToArray();

            //var j = latest.Adapt<QuizViewModel[]>();
            return new JsonResult(
            latest, new JsonSerializerSettings()
            {
                Formatting = Formatting.Indented
            });
        }


        [HttpGet("ByTitle/{num:int?}")]
        public IActionResult ByTitle(int num = 10)
        {
            var byTitle = DbContext.Quizzes
               .OrderBy(q => q.Title)
               .Take(num)
               .ToArray();
            //var j = byTitle.Adapt<QuizViewModel[]>();
            return new JsonResult(
            byTitle,
                new JsonSerializerSettings()
                {
                    Formatting = Formatting.Indented
                });
        }

        [HttpGet("Random/{num:int?}")]
        public IActionResult Random(int num = 10)
        {
            var random = DbContext.Quizzes
                 .OrderBy(q => Guid.NewGuid())
                 .Take(num)
                 .ToArray();
            return new JsonResult(
            random,
                new JsonSerializerSettings()
                {
                    Formatting = Formatting.Indented
                });
        }
    }
}