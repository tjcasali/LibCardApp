using LibCardApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Configuration;
using System.Web.Http;
using LibCardApp.Dtos;
using AutoMapper;

namespace LibCardApp.Controllers.Api
{
    public class PatronsController : ApiController
    {
        private ApplicationDbContext _context;

        public PatronsController()
        {
            _context = new ApplicationDbContext();
        }

        // GET /api/patrons
        public IEnumerable<PatronDto> GetPatrons()
        {
            return _context.Patrons.ToList().Select(Mapper.Map<Patron, PatronDto>);
        }

        // GET /api/patrons/1
        public IHttpActionResult GetPatron(int id)
        {
            var patron = _context.Patrons.SingleOrDefault(c => c.Id == id);

            if (patron == null)
                return NotFound();

            return Ok(Mapper.Map<Patron, PatronDto>(patron));
        }

        //POST /api/patrons
        [Authorize(Roles = RoleName.CanManagePatrons)]
        [HttpPost]
        public IHttpActionResult CreatePatron(PatronDto patronDto)
        {
            if (!ModelState.IsValid)
                return BadRequest();
            var patron = Mapper.Map<PatronDto, Patron>(patronDto);
            _context.Patrons.Add(patron);
            _context.SaveChanges();

            patronDto.Id = patron.Id;

            return Created(new Uri(Request.RequestUri + "/" + patron.Id), patronDto);
        }

        // PUT /api/patrons/1
        [Authorize(Roles = RoleName.CanManagePatrons)]
        [HttpPut]
        public void UpdatePatron(int id, PatronDto patronDto)
        {
            if (!ModelState.IsValid)
                throw new HttpResponseException(HttpStatusCode.BadRequest);

            var patronInDb = _context.Patrons.SingleOrDefault(c => c.Id == id);

            if (patronInDb == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            Mapper.Map(patronDto, patronInDb);

            _context.SaveChanges();
        }

        // DELETE /api/patrons/1
        [Authorize(Roles = RoleName.CanManagePatrons)]
        [HttpDelete]
        public void DeletePatrons(int id)
        {
            var patronInDb = _context.Patrons.SingleOrDefault(c => c.Id == id);

            if (patronInDb == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);

            _context.Patrons.Remove(patronInDb);
            _context.SaveChanges();
        }
    }
}
