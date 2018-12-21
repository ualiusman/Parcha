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
    public class QuestionController : ControllerBase
    {
        #region Private Fields
        private ApplicationDbContext DbContext;
        #endregion
        #region Constructor
        public QuestionController(ApplicationDbContext context)
        {
            // Instantiate the ApplicationDbContext through DI
            DbContext = context;
        }
        #endregion


        [HttpGet]
        public IActionResult Get(int id)
        {
            var question = DbContext.Questions.Where(i => i.Id == id)
 .FirstOrDefault();
            // handle requests asking for non-existing questions
            if (question == null)
            {
                return NotFound(new
                {
                    Error = String.Format("Question ID {0} has not been found", id)
                });
            }
            return new JsonResult(
                question,
            new JsonSerializerSettings()
            {
                Formatting = Formatting.Indented
            });
        }


        [HttpPut]
        public IActionResult Put([FromBody] QuestionViewModel model)
        {
            // return a generic HTTP Status 500 (Server Error)
            // if the client payload is invalid.
            if (model == null) return new StatusCodeResult(500);
            // map the ViewModel to the Model
            var question = model.Adapt<Question>();
            // override those properties
            // that should be set from the server-side only
            question.QuizId = model.QuizId;
            question.Text = model.Text;
            question.Notes = model.Notes;
            // properties set from server-side
            question.CreatedDate = DateTime.Now;
            question.LastModifiedDate = question.CreatedDate;
            // add the new question
            DbContext.Questions.Add(question);
            // persist the changes into the Database.
            DbContext.SaveChanges();
            // return the newly-created Question to the client.
            return new JsonResult(question.Adapt<QuestionViewModel>(),
            new JsonSerializerSettings()
            {
                Formatting = Formatting.Indented
            });
        }

        [HttpPost]
        public IActionResult Post([FromBody]QuestionViewModel model)
        {
            if (model == null) return new StatusCodeResult(500);
            var question = DbContext.Questions.Where(q => q.Id ==
            model.Id).FirstOrDefault();
            if (question == null)
            {
                return NotFound(new
                {
                    Error = String.Format("Question ID {0} has not been found", model.Id)
                });
            }
            question.QuizId = model.QuizId;
            question.Text = model.Text;
            question.Notes = model.Notes;
            question.LastModifiedDate = question.CreatedDate;
            DbContext.SaveChanges();
            return new JsonResult(question.Adapt<QuestionViewModel>(),
            new JsonSerializerSettings()
            {
                Formatting = Formatting.Indented
            });
        }



        [HttpGet("All/{quizId}")]
        public IActionResult All(int quizId)
        {
            var questions = DbContext.Questions.Where(q => q.QuizId == quizId).ToArray();
            return new JsonResult(
            questions.Adapt<QuestionViewModel[]>(),
            new JsonSerializerSettings()
            {
                Formatting = Formatting.Indented
            });
        }

        public IActionResult Delete(int id)
        {
            var question = DbContext.Questions.Where(i => i.Id == id)
            .FirstOrDefault();
            if (question == null)
            {
                return NotFound(new
                {
                    Error = String.Format("Question ID {0} has not been found", id)
                });
            }
            DbContext.Questions.Remove(question);
            DbContext.SaveChanges();
            return new OkResult();
        }
    }
}