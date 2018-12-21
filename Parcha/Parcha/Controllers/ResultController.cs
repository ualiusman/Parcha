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
    public class ResultController : ControllerBase
    {
        #region Private Fields
        private ApplicationDbContext DbContext;
        #endregion
        #region Constructor
        public ResultController(ApplicationDbContext context)
        {
            // Instantiate the ApplicationDbContext through DI
            DbContext = context;
        }
        #endregion

        [HttpGet]
        public IActionResult Get(int id)
        {
            var result = DbContext.Results.Where(i => i.Id == id)
 .FirstOrDefault();
            // handle requests asking for non-existing results
            if (result == null)
            {
                return NotFound(new
                {
                    Error = String.Format("Result ID {0} has not been found", id)
                });
            }
            return new JsonResult(
            result.Adapt<ResultViewModel>(),
            new JsonSerializerSettings()
            {
                Formatting = Formatting.Indented
            });
        }


        [HttpPut]
        public IActionResult Put([FromBody]ResultViewModel model)
        {
            if (model == null) return new StatusCodeResult(500);
            var result = model.Adapt<Result>();
            result.CreatedDate = DateTime.Now;
            result.LastModifiedDate = result.CreatedDate;
            // add the new result
            DbContext.Results.Add(result);
            DbContext.SaveChanges();
            return new JsonResult(result.Adapt<ResultViewModel>(),
            new JsonSerializerSettings()
            {
                Formatting = Formatting.Indented
            });
        }

        [HttpPost]
        public IActionResult Post([FromBody]ResultViewModel model)
        {
            // return a generic HTTP Status 500 (Server Error)
            // if the client payload is invalid.
            if (model == null) return new StatusCodeResult(500);
            // retrieve the result to edit
            var result = DbContext.Results.Where(q => q.Id ==
            model.Id).FirstOrDefault();
            // handle requests asking for non-existing results
            if (result == null)
            {
                return NotFound(new
                {
                    Error = String.Format("Result ID {0} has not been found", model.Id)
                });
            }
            result.QuizId = model.QuizId;
            result.Text = model.Text;
            result.MinValue = model.MinValue;
            result.MaxValue = model.MaxValue;
            result.Notes = model.Notes;
            result.LastModifiedDate = result.CreatedDate;
            DbContext.SaveChanges();
            return new JsonResult(result.Adapt<ResultViewModel>(),
            new JsonSerializerSettings()
            {
                Formatting = Formatting.Indented
            });
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var result = DbContext.Results.Where(i => i.Id == id)
            .FirstOrDefault();
            if (result == null)
            {
                return NotFound(new
                {
                    Error = String.Format("Result ID {0} has not been found", id)
                });
            }
            DbContext.Results.Remove(result);
            DbContext.SaveChanges();
            return new OkResult();
        }

        [HttpGet("All/{quizId}")]
        public IActionResult All(int quizId)
        {
            var results = DbContext.Results
  .Where(q => q.QuizId == quizId)
  .ToArray();
            return new JsonResult(
            results.Adapt<ResultViewModel[]>(),
            new JsonSerializerSettings()
            {
                Formatting = Formatting.Indented
            });

        }
    }
}