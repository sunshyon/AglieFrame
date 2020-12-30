using System;

namespace AglieFrame.Model
{
	public class ResultRoot<T>
	{
		public T Data { get; set; }
		public CodeType Code { get; set; } = CodeType.Faild;
		public string Msg { get; set; }
	}

	public enum CodeType
	{
		Succeed = 0,
		Faild = 1,
		NoAuth = 2,
		Other = 3
	}

}
