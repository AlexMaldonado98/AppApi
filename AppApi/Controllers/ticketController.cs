using AppApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace AppApi.Controllers
{
    public class ticketController : ApiController
    {
        public IHttpActionResult GetAllTickets()
        {
            IList<classTicket> tickets = null;

            using (var ctx = new ticketsEntities())
            {
                tickets = ctx.tickets
                    .Select(s => new classTicket()
                    {
                        id = s.id,
                        nombre = s.nombre,
                        descripcion = s.descripcion,
                        departamento = s.departamento

                    }).ToList<classTicket>();
            }

            if (tickets == null || tickets.Count == 0)
            {
                return NotFound();
            }

            return Ok(tickets);
        }

        public IHttpActionResult GetTicketById(int id)
        {
            classTicket ticket = null;

            using (var ctx = new ticketsEntities())
            {
                ticket = ctx.tickets
                    .Where(s => s.id == id)
                    .Select(s => new classTicket()
                    {
                        id = s.id,
                        nombre = s.nombre,
                        descripcion = s.descripcion,
                        departamento = s.departamento

                    }).FirstOrDefault<classTicket>();
            }

            if (ticket == null)
            {
                return NotFound();
            }

            return Ok(ticket);
        }

        public IHttpActionResult PostNewStudent(tickets newTicket)
        {

            using (var ctx = new ticketsEntities())
            {
                ctx.tickets.Add(new tickets()
                {
                    id= newTicket.id,
                    nombre = newTicket.nombre,
                    departamento = newTicket.departamento,
                    descripcion = newTicket.descripcion
                });

                ctx.SaveChanges();
            }

            return Ok();
        }

        public IHttpActionResult Put(tickets putTicket)
        {

            using (var ctx = new ticketsEntities())
            {
                var existingTicket = ctx.tickets.Where(s => s.id == putTicket.id).FirstOrDefault<tickets>();

                if (existingTicket != null)
                {
                    existingTicket.id = putTicket.id;
                    existingTicket.nombre = putTicket.nombre;
                    existingTicket.descripcion = putTicket.descripcion;
                    existingTicket.departamento = putTicket.departamento;
                    ctx.SaveChanges();
                }
                else
                {
                    return NotFound();
                }
            }

            return Ok();
        }

        [Authorize(Roles = "user")]
        public IHttpActionResult Delete(int id)
        {
            using (var ctx = new ticketsEntities())
            {
                var ticket = ctx.tickets
                    .Where(s => s.id == id)
                    .FirstOrDefault();

                ctx.Entry(ticket).State = System.Data.Entity.EntityState.Deleted;
                ctx.SaveChanges();
            }
            return Ok();
        }
    }
}
