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
    [ApiController]
    public class AnswerController : BaseApiController
    {
        #region Private Fields
        #endregion
        #region Constructor
        public AnswerController(ApplicationDbContext context)
            :base(context)
        {
        }
        #endregion

        [HttpGet]
        public IActionResult Get(int id)
        {
            var answer = DbContext.Answers.Where(i => i.Id == id).FirstOrDefault();
            if (answer == null)
            {
                return NotFound(new
                {
                    Error = String.Format("Answer ID {0} has not been found", id)
                });
            }
            return new JsonResult(
            answer.Adapt<AnswerViewModel>(),
            JsonSettings);
        }


        [HttpPut]
        public IActionResult Put([FromBody]AnswerViewModel model)
        {
            if (model == null) return new StatusCodeResult(500);
            var answer = model.Adapt<Answer>();
            answer.QuestionId = model.QuestionId;
            answer.Text = model.Text;
            answer.Notes = model.Notes;
            answer.CreatedDate = DateTime.Now;
            answer.LastModifiedDate = answer.CreatedDate;
            DbContext.Answers.Add(answer);
            DbContext.SaveChanges();

            return new JsonResult(answer.Adapt<AnswerViewModel>(),
            JsonSettings);
        }

        [HttpPost]
        public IActionResult Post([FromBody]AnswerViewModel model)
        {
            if (model == null) return new StatusCodeResult(500);
            var answer = DbContext.Answers.Where(q => q.Id == model.Id).FirstOrDefault();
            if (answer == null)
            {
                return NotFound(new
                {
                    Error = String.Format("Answer ID {0} has not been found", model.Id)
                });
            }
            answer.QuestionId = model.QuestionId;
            answer.Text = model.Text;
            answer.Value = model.Value;
            answer.Notes = model.Notes;
            answer.LastModifiedDate = answer.CreatedDate;
            DbContext.SaveChanges();

            return new JsonResult(answer.Adapt<AnswerViewModel>(),
            JsonSettings);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var answer = DbContext.Answers.Where(i => i.Id == id)
            .FirstOrDefault();
            if (answer == null)
            {
                return NotFound(new
                {
                    Error = String.Format("Answer ID {0} has not been  found", id)
                });
            }
            DbContext.Answers.Remove(answer);
            DbContext.SaveChanges();
            return new OkResult();
        }



        [HttpGet("All/{questionId}")]
        public IActionResult All(int questionId)
        {
            var answers = DbContext.Answers
 .Where(q => q.QuestionId == questionId)
 .ToArray();
            return new JsonResult(
            answers.Adapt<AnswerViewModel[]>(),
            JsonSettings);

        }
    }
}