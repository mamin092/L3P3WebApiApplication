using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using L3P3WebApiApplication;

namespace L3P3WebApiApplication.Controllers
{
    public class TraineesController : ApiController
    {
        private VirtualTraineeDbEntities db = new VirtualTraineeDbEntities();

        // GET: api/Trainees
        public List<Trainee> GetTrainees()
        {
            IEnumerable<Trainee> trainees = db.Trainees.AsEnumerable();
            List<Trainee> list = trainees.Select(
                x => new Trainee()
                {
                    Id = x.Id,
                    Name = x.Name,
                    Address = x.Address,
                    Department = new Department()
                    {
                        Id =x.Department.Id,
                        Name= x.Department.Name,
                        EntryAt= x.Department.EntryAt
                    },
                    DepartmentId = x.DepartmentId,
                    Phone = x.Phone
                }).ToList();
            return list;
        }

        // GET: api/Trainees/5
        [ResponseType(typeof(Trainee))]
        public IHttpActionResult GetTrainee(int id)
        {
            Trainee trainee = db.Trainees.Find(id);
            if (trainee == null)
            {
                return NotFound();
            }

            return Ok(trainee);
        }

        // PUT: api/Trainees/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutTrainee(int id, Trainee trainee)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != trainee.Id)
            {
                return BadRequest();
            }

            db.Entry(trainee).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TraineeExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Trainees
        [ResponseType(typeof(Trainee))]
        public IHttpActionResult PostTrainee(Trainee trainee)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Trainees.Add(trainee);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = trainee.Id }, trainee);
        }

        // DELETE: api/Trainees/5
        [ResponseType(typeof(Trainee))]
        public IHttpActionResult DeleteTrainee(int id)
        {
            Trainee trainee = db.Trainees.Find(id);
            if (trainee == null)
            {
                return NotFound();
            }

            db.Trainees.Remove(trainee);
            db.SaveChanges();

            return Ok(trainee);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool TraineeExists(int id)
        {
            return db.Trainees.Count(e => e.Id == id) > 0;
        }
    }
}