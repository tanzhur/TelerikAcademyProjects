using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using ManagementSystem.Models;
using System.Web.Mvc;

namespace ManagementSystem.Areas.Adminstration.ViewModels
{
    public class TodoViewModel
    {
        public static Expression<Func<Todo, TodoViewModel>> FromTodo
        {
            get
            {
                return t => new TodoViewModel()
                {
                    Id = t.Id,
                    Description = t.Description,
                    PlanId = t.Plan.Id,
                    PlanName = t.Plan.Title,
                    Title = t.Title,
                    Priority = t.Priority,
                    State = t.State
                };
            }
        }

        [ScaffoldColumn(false)]
        public int Id { get; set; }

        [AllowHtml]
        [Required]
        public string Title { get; set; }

        [AllowHtml]
        [DataType(DataType.MultilineText)]
        [Required]
        public string Description { get; set; }

        [ScaffoldColumn(false)]
        public byte PriorityCode { get; set; }

        [UIHint("PriorityDropDown")]
        public Priority Priority
        {
            get { return (Priority)PriorityCode; }
            set { PriorityCode = (byte)value; }
        }

        [ScaffoldColumn(false)]
        public byte StateCode { get; set; }

        [UIHint("StateDropDown")]
        public State State
        {
            get { return (State)StateCode; }
            set { StateCode = (byte)value; }
        }

        [ScaffoldColumn(false)]
        [UIHint("PlansDropDown")]
        public int PlanId { get; set; }

        [UIHint("PlansDropDown")]
        public string PlanName { get; set; }

        public virtual Plan Plan { get; set; }
    }
}