using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using ManagementSystem.Data;
using ManagementSystem.Models;

namespace ManagementSystem.Areas.Adminstration.ViewModels
{
    public class ParticipantViewModel
    {
        private static IUowData db = new UowData();

        public ParticipantViewModel()
        {
            this.Plans = new HashSet<Plan>();
            this.Todos = new HashSet<Todo>();
        }

        public static Expression<Func<Participant, ParticipantViewModel>> FromParticipant
        {
            get
            {
                return a => new ParticipantViewModel()
                {
                    ParticipantId = a.Id,
                    ParticipantUsername = db.AppUsers.All().FirstOrDefault(x => x.Id == a.Id).UserName,
                    Plans = a.Plans,
                    Todos = a.Todos
                };
            }
        }

        public string ParticipantId { get; set; }

        public string ParticipantUsername { get; set; }

        public virtual ICollection<Plan> Plans { get; set; }

        public virtual ICollection<Todo> Todos { get; set; } 
    }
}