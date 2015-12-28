using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Http;
using System.Web.Http.Description;
using BugTracker.Data.Models;
using BugTracker.Data.UnitOfWork;
using BugTracker.RestServices.Infrastructure;
using BugTracker.RestServices.Models.BindingModels;
using BugTracker.RestServices.Models.ViewModels;
using Microsoft.AspNet.Identity;

namespace BugTracker.RestServices.Controllers
{
    [RoutePrefix("api/bugs")]
    public class BugsController : BaseApiController
    {
        public BugsController(IBugsUnitOfWork data)
            : base(data)
        {
        }

        public BugsController()
        {
        }

        // GET: api/Bugs
        [HttpGet]
        public IHttpActionResult GetBugs()
        {
            var bugs = this.Data.Bugs.All()
                .OrderByDescending(b => b.DateCreated)
                .ThenByDescending(b => b.Id)
                .Select(BugViewModel.Create());

            return this.Ok(bugs);
        }

        // GET: api/Bugs/5
        [ResponseType(typeof(Bug))]
        [Route("{id}")]
        [HttpGet]
        public IHttpActionResult GetBug(int id)
        {

            var bug = this.Data.Bugs.All()
                .Where(b => b.Id == id)
                .Select(b => new BugByIdViewModel
                {
                    Id = b.Id,
                    Title = b.Title,
                    Description = b.Description,
                    Status = b.Status.ToString(),
                    Author = b.Author == null ? null : b.Author.UserName,
                    DateCreated = b.DateCreated,
                    Comments = b.Comments
                        .OrderByDescending(c => c.DateCreated)
                        .ThenByDescending(c => c.Id)
                        .Select(c => new CommentViewModel()
                        {
                            Id = c.Id,
                            Text = c.Text,
                            Author = c.Author == null ? null : c.Author.UserName,
                            DateCreated = c.DateCreated
                        })
                })
                .FirstOrDefault();

            if (bug == null)
            {
                return NotFound();
            }

            return Ok(bug);
        }

        // PATCH: api/Bugs/{id}
        [ResponseType(typeof(void))]
        [HttpPatch]
        [Route("{id}")]
        public IHttpActionResult PatchBug(int id, EditBugBindingModel bugData)
        {
            if (bugData == null)
            {
                return this.BadRequest("Missing bug data to patch.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var bug = this.Data.Bugs.Find(id);

            if (bug == null)
            {
                return this.NotFound();
            }

            if (bugData.Title != null)
            {
                bug.Title = bugData.Title;
            }

            if (bugData.Description != null)
            {
                bug.Description = bugData.Description;
            }

            if (bugData.Status != null)
            {
                BugStatus newStatus;
                bool isSuccessful = Enum.TryParse(bugData.Status, out newStatus);

                if (!isSuccessful)
                {
                    return this.BadRequest("Invalid bug status.");
                }

                bug.Status = newStatus;
            }

            this.Data.SaveChanges();

            return this.Ok(new
            {
                Message = "Bug #" + id + " patched."
            });
        }

        // POST: api/Bugs
        [ResponseType(typeof(Bug))]
        public IHttpActionResult PostBug(BugBindingModel model)
        {

            if (model == null)
            {
                return this.BadRequest("Missing model data.");
            }

            if (!ModelState.IsValid)
            {
                return this.BadRequest(ModelState);
            }

            var currentUserId = this.User.Identity.GetUserId();
            var user = this.Data.Users.Find(currentUserId);

            var bug = new Bug()
            {
                Title = model.Title,
                Description = model.Description,
                Status = BugStatus.Open,
                DateCreated = DateTime.Now,
                Author = user
            };

            this.Data.Bugs.Add(bug);
            this.Data.SaveChanges();

            if (user != null)
            {
                return CreatedAtRoute("DefaultApi", new { id = bug.Id }, new { bug.Id, Author = user.UserName, Message = "User bug submitted." });
            }
            return CreatedAtRoute("DefaultApi", new { id = bug.Id }, new { bug.Id, Message = "Anonymous bug submitted." });
        }

        // DELETE: api/Bugs/5
        [ResponseType(typeof(Bug))]
        [HttpDelete]
        [Route("{id}")]
        public IHttpActionResult DeleteBug(int id)
        {
            var bug = this.Data.Bugs.Find(id);

            if (bug == null)
            {
                return NotFound();
            }

            this.Data.Bugs.Remove(bug);
            this.Data.SaveChanges();

            return Ok(new
            {
                Message = "Bug #" + bug.Id + " deleted."
            });
        }

        [HttpGet]
        [Route("filter")]
        public IHttpActionResult FilterBugs([FromUri]FilterBugsBindingModel model)
        {
            var bugs = this.Data.Bugs.All();

            if (model != null)
            {
                bugs = BuildingFilter(model, bugs);
            }

            var data = bugs.Select(BugViewModel.Create());

            return this.Ok(data);
        }

        private static IQueryable<Bug> BuildingFilter(FilterBugsBindingModel model, IQueryable<Bug> bugs)
        {
            if (model.Keyword != null)
            {
                bugs = bugs.Where(b => b.Title.Contains(model.Keyword));
            }

            if (model.Author != null)
            {
                bugs = bugs.Where(b => b.Author.UserName == model.Author);
            }

            if (model.Statuses != null)
            {
                var statuses = model.Statuses.Split('|');
                var bugStatuses = new List<BugStatus>();

                for (int i = 0; i < statuses.Length; i++)
                {
                    BugStatus parsedStatus;
                    bool isSuccessful = Enum.TryParse(statuses[i], out parsedStatus);

                    if (isSuccessful)
                    {
                        bugStatuses.Add(parsedStatus);
                    }
                }

                bugs = bugs.Where(b => bugStatuses.Contains(b.Status));
            }
            return bugs;
        }
    }
}