using AglieFrame.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AglieFrame.Service.Filter
{
	public class ActionFilter : ActionFilterAttribute
	{
		public override void OnActionExecuted(ActionExecutedContext context)
		{
			var nullRes = new  EmptyResult();//返回null
			if (context.Result.GetType() != nullRes.GetType())
			{
				var result = ((ObjectResult)context.Result).Value;

				context.Result = new JsonResult(new ResultRoot<object>()
				{
					//Code = CodeType.Succeed,
					Data = result
				});
			}
		}
		public override void OnResultExecuted(ResultExecutedContext context)
		{
		}
	}
}
